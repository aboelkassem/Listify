import { MatDialog } from '@angular/material/dialog';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { IConfirmationModalData, IGenre, IPlaylist } from 'src/app/interfaces';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { Subscription } from 'rxjs';
import { ConfirmationmodalComponent } from '../modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-profileplaylists',
  templateUrl: './profileplaylists.component.html',
  styleUrls: ['./profileplaylists.component.css']
})
export class ProfileplaylistsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['playlistImageUrl', 'playlistName', 'numberOfSongs', 'genreName', 'queuePlaylist'];
  dataSource = new MatTableDataSource<IPlaylist>();

  loading = false;

  playlists: IPlaylist[] = [];
  genres: IGenre[] = [];
  genreSelectedId: string;

  $genresReceivedSubscription: Subscription;
  $profileReceivedSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private matDialog: MatDialog) {
      this.$genresReceivedSubscription = this.hubService.getGenres().subscribe(genres => {
        this.genres = genres;
      });

      this.$profileReceivedSubscription = this.hubService.getProfile().subscribe(profile => {
        this.playlists = profile.playlists;
        this.dataSource.data = this.playlists;
        this.loading = false;
      });
    }

  ngOnInit(): void {
    this.hubService.requestGenres();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.$genresReceivedSubscription.unsubscribe();
    this.$profileReceivedSubscription.unsubscribe();
  }

  queuePlaylist(id: string): void {
    let confirmationModalData: IConfirmationModalData = {
      title: 'Are your sure ?',
      message: 'Are your sure you want to add the entire playlist to your queue?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    let confirmationModal = this.matDialog.open(ConfirmationmodalComponent, {
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

        confirmationModal = this.matDialog.open(ConfirmationmodalComponent, {
          width: '350px',
          data: confirmationModalData
        });

        confirmationModal.afterClosed().subscribe(randomizeResult => {
          this.loading = true;
          if (randomizeResult !== undefined) {
            this.hubService.requestQueuePlaylistInRoomHome(id, true);
          }else {
            this.hubService.requestQueuePlaylistInRoomHome(id, false);
          }
        });
      }
    });
  }

  genreChanged(id: string): void {
    if (id === undefined || id === null || id === '') {
      this.dataSource.data = this.playlists;
    }else {
      const playlists: IPlaylist[] = [];

      this.playlists.forEach(playlist => {
        playlist.playlistGenres.forEach(playlistGenreFiltered => {
          if (playlistGenreFiltered.genre.id === id) {
            playlists.push(playlist);
            return;
          }
        });
      });

      this.dataSource.data = playlists;
    }
  }

  clearFilter(): void {
    this.dataSource.data = this.playlists;
    this.genreSelectedId = '';
  }
}
