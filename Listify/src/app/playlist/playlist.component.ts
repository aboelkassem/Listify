import { HttpService } from './../services/http.service';
import { ConfirmationmodalComponent } from './../shared/modals/confirmationmodal/confirmationmodal.component';
import { MatDialog } from '@angular/material/dialog';
import { IConfirmationModalData, IGenre, IInformationModalData, IInputModalData } from 'src/app/interfaces';
import { ToastrService } from 'ngx-toastr';
import { Subscription } from 'rxjs';
import { IPlaylistCreateRequest, IPlaylist, ISongPlaylist, IPlaylistGenre } from './../interfaces';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { InputmodalComponent } from '../shared/modals/inputmodal/inputmodal.component';
import { Location } from '@angular/common';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit, OnDestroy {

  @ViewChild('file') file: any;
  files: Set<File> = new Set();

  loading = false;

  id: string;
  playlistName: string;
  isSelected: boolean;
  isPublic: boolean;
  playlist: IPlaylist;
  songsPlaylist: ISongPlaylist[] = [];
  numberOfSongs = this.hubService.applicationUser.playlistSongCount;
  genres: IGenre[] = [];
  playlistGenres: IPlaylistGenre[] = [];
  genreSelectedId: string;
  isOwner: boolean;
  OwnerName: string;
  playlistImageUrl: string;

  $playlistSubscription: Subscription;
  $queuePlaylistInHomeSubscription: Subscription;
  $applicationUserSubscription: Subscription;
  $genresReceivedSubscription: Subscription;
  $songPlaylistSubscription: Subscription;
  $songsPlaylistSubscription: Subscription;
  $addYoutubePlaylistToPlaylistSubscription: Subscription;
  $addSpotifyPlaylistToPlaylistSubscription: Subscription;
  $clearPlaylistImageSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private httpService: HttpService,
    private router: Router,
    private toastrService: ToastrService,
    private modalDialog: MatDialog,
    private location: Location,
    private route: ActivatedRoute) {
    this.$clearPlaylistImageSubscription = this.hubService.getClearPlaylistImage().subscribe(isSuccess => {
      this.loading = false;
      if (isSuccess) {
        this.toastrService.success('You have successfully cleared this playlist image.', 'Delete Success');
      }else {
        this.toastrService.error('There was a problem communicating with the server. please try again', 'Delete Failed');
      }
    });

    this.$genresReceivedSubscription = this.hubService.getGenres().subscribe(genres => {
      this.genres = genres;
    });

    this.$applicationUserSubscription = this.hubService.getApplicationUser().subscribe(applicationUser => {
      this.numberOfSongs = applicationUser.playlistSongCount;
    });

    this.$addSpotifyPlaylistToPlaylistSubscription = this.hubService.getAddSpotifyPlaylistToPlaylist().subscribe(songsPlaylist => {
      this.loading = false;

      if (songsPlaylist && songsPlaylist.length > 0) {
        this.toastrService.success('The Spotify Playlist was successfully added to the playlist.', 'Added Spotify Playlist');
        this.hubService.requestPlaylist(this.id);
      }else {
        this.toastrService.error('The Spotify playlist was invalid, please try again.', 'Invalid Url');
      }
    });

    this.$addYoutubePlaylistToPlaylistSubscription = this.hubService.getAddYoutubePlaylistToPlaylist().subscribe(songsPlaylist => {
      this.loading = false;

      if (songsPlaylist && songsPlaylist.length > 0) {
        this.toastrService.success('The Youtube Playlist was successfully added to the playlist.', 'Added Youtube Playlist');
        this.hubService.requestPlaylist(this.id);
      }else {
        this.toastrService.error('The Youtube playlist was invalid, please try again.', 'Invalid Url');
      }
    });

    this.$playlistSubscription = this.hubService.getPlaylist().subscribe(playlist => {
      this.id = playlist.id;
      this.playlistName = playlist.playlistName;
      this.isSelected = playlist.isSelected;
      this.isPublic = playlist.isPublic;
      this.playlist = playlist;
      this.OwnerName = this.playlist.applicationUser.username;
      this.playlistGenres = playlist?.playlistGenres;
      this.songsPlaylist = playlist?.songsPlaylist;
      this.playlistImageUrl = playlist.playlistImageUrl;

      if (this.playlist.applicationUser.id === this.hubService.applicationUser.id) {
        this.isOwner = true;
      }else {
        this.isOwner = false;
      }
    });

    this.$songPlaylistSubscription = this.hubService.getSongPlaylist().subscribe(songPlaylist => {
      // this.hubService.requestSongsPlaylist(this.playlist.id);
      this.hubService.requestPlaylist(this.id);
    });

    this.$queuePlaylistInHomeSubscription = this.hubService.getQueuePlaylistInRoomHome().subscribe(songsQueued => {
      if (songsQueued) {
        this.loading = false;
        this.router.navigateByUrl('/');
      }
    });

    this.$songsPlaylistSubscription = this.hubService.getSongsPlaylist().subscribe(songsPlaylist => {
      if (songsPlaylist) {
        this.songsPlaylist = songsPlaylist;
      }
    });

    this.route.params.subscribe(params => {
      const id = params['id']; // + params converts id to numbers
      if (id != null) {
        this.hubService.requestPlaylist(id);
      }
    });

   }

  ngOnInit(): void {
    // this.hubService.requestSongsPlaylist(this.playlist.id);
    this.hubService.requestGenres();
  }

  ngOnDestroy(): void {
    this.$playlistSubscription.unsubscribe();
    this.$applicationUserSubscription.unsubscribe();
    this.$songPlaylistSubscription.unsubscribe();
    this.$genresReceivedSubscription.unsubscribe();
    this.$songsPlaylistSubscription.unsubscribe();
    this.$addSpotifyPlaylistToPlaylistSubscription.unsubscribe();
    this.$addYoutubePlaylistToPlaylistSubscription.unsubscribe();
    this.$clearPlaylistImageSubscription.unsubscribe();
    this.$queuePlaylistInHomeSubscription.unsubscribe();
  }

  savePlaylist(): void {
    const request: IPlaylistCreateRequest = {
      id: this.id,
      playlistName: this.playlistName,
      isSelected: this.isSelected,
      isPublic: this.isPublic,
      playlistGenres: this.playlistGenres
    };
    this.hubService.savePlaylist(request);

    this.router.navigateByUrl('/playlists');
  }

  removeSongFromPlaylist(songPlaylist: ISongPlaylist): void {
    this.hubService.deleteSongPlaylist(songPlaylist.id);
  }

  queuePlaylist(): void {
    let confirmationModalData: IConfirmationModalData = {
      title: 'Are your sure ?',
      message: 'Are your sure you want to add the entire playlist to your queue?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    let confirmationModal = this.modalDialog.open(ConfirmationmodalComponent, {
      width: '250px',
      data: confirmationModalData
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {

        confirmationModalData = {
          title: 'Randomize the playlist?',
          message: 'would you like to randomize the playlist in the queue?',
          isConfirmed: false,
          confirmMessage: 'Randomize',
          cancelMessage: 'Do not Randomize'
        };

        confirmationModal = this.modalDialog.open(ConfirmationmodalComponent, {
          width: '350px',
          data: confirmationModalData
        });

        confirmationModal.afterClosed().subscribe(randomizeResult => {
          this.loading = true;
          if (randomizeResult !== undefined) {
            this.hubService.requestQueuePlaylistInRoomHome(this.playlist.id, true);
          }else {
            this.hubService.requestQueuePlaylistInRoomHome(this.playlist.id, false);
          }
        });
      }
    });
  }

  importYoutubePlaylist(): void {
    const inputData: IInputModalData = {
      title: 'Youtube Playlist Url',
      message: 'Please enter the Youtube Playlist Url',
      placeholder: 'Enter Youtube Playlist Url ...',
      data: ''
    };

    const inputModal = this.modalDialog.open(InputmodalComponent, {
      width: '350px',
      data: inputData
    });

    inputModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
       this.loading = true;
       this.hubService.requestAddYoutubePlaylistToPlaylist(result, this.playlist.id);
      }
    });
  }

  importSpotifyPlaylist(): void {
    const inputData: IInputModalData = {
      title: 'Spotify Playlist Url',
      message: 'Please enter the Spotify Playlist Url',
      placeholder: 'Enter Spotify Playlist Url ...',
      data: ''
    };

    const inputModal = this.modalDialog.open(InputmodalComponent, {
      width: '350px',
      data: inputData
    });

    inputModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
       this.loading = true;
       this.hubService.requestAddSpotifyPlaylistToPlaylist(result, this.playlist.id);
      }
    });
  }

  addGenre(): void {
    if (this.genreSelectedId === undefined || this.genreSelectedId === null) {
      this.toastrService.error('You have not selected any genres, please choose one.', 'No Genre Selected');
    }else {
      let playlistGenre = this.playlistGenres.filter(x => x.genre.id === this.genreSelectedId)[0];
      if (playlistGenre) {
        this.toastrService.error('Genre already selected for this playlist, you have choose different one.', 'Genre already selected');
      }else {
        if (this.playlistGenres.length >= 3) {
          this.toastrService.warning('There are already 3 genres for this playlist', 'Max 3 Genres Per Playlist');
        }else {
          const genreSelected = this.genres.filter(x => x.id === this.genreSelectedId)[0];

          playlistGenre = {
            genre : genreSelected
          };

          this.playlistGenres.push(playlistGenre);
          this.toastrService.success('Genre added to the playlist successfully', genreSelected.name + ' Added');
        }
      }
    }
  }

  removeGenre(id: string): void {
    // Confirmation modal to make sure to remove the genre
    const selectedPlaylistGenre = this.playlistGenres.filter(x => x.genre.id === id)[0];

    if (selectedPlaylistGenre) {
      this.playlistGenres.splice(this.playlistGenres.indexOf(selectedPlaylistGenre), 1);
      this.toastrService.success(selectedPlaylistGenre.genre.name + ' has been deleted from the playlist successfully', 'Genre Removed');
    }else {
      this.toastrService.error('There was an error while deleting the genre, please try again.', 'Could not remove genre');
    }
  }

  uploadPlaylistImage(): void {
    this.file.nativeElement.click();
  }

  onFilesAdded(): void {
    const file = this.file.nativeElement.files[0];

    if (file !== undefined) {
      if (file.size > 500000) {
        this.toastrService.error('That picture is too large - the maximum size is 500 kb.', 'Image too large');
        return;
      }else {
          this.httpService.requestPlaylistImageUpload(file, this.id)
          .subscribe(playlist => {
            this.loading = false;
            if (playlist !== undefined && playlist !== null) {
              this.toastrService.success('You have successfully updated a new Playlist Image', 'Uploaded Success');

              this.loading = true;
              this.hubService.requestPlaylist(this.id);
            }else {
              this.toastrService.error('Your upload was not successful, please try again.', 'Upload Failed');
            }
          });
      }
    }
  }

  clearPlaylistImage(): void {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Are you Sure ?',
      message: 'Are you sure you would like to delete this playlist image?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    const confirmationModal = this.modalDialog.open(ConfirmationmodalComponent,
      {
        width: '300px',
        data: confirmationModalData
      });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.requestClearPlaylistImage(this.playlist.id);
      }
    });
  }

  back(): void {
    this.location.back();
  }
}
