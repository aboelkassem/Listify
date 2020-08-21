import { Subscription } from 'rxjs';
import { IPlaylistCreateRequest, IPlaylist, ISongPlaylist } from './../interfaces';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit, OnDestroy {

  // @Input() playlist: IPlaylist;
  id: string;
  playlistName: string;
  isSelected: boolean;
  playlist: IPlaylist;
  songsPlaylist: ISongPlaylist[] = [];

  $playlistSubscription: Subscription;
  $songPlaylistSubscription: Subscription;
  $songsPlaylistSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private route: ActivatedRoute) {
    this.$playlistSubscription = this.hubService.getPlaylist().subscribe(playlist => {
      this.id = playlist.id;
      this.playlistName = playlist.playlistName;
      this.isSelected = playlist.isSelected;
      this.playlist = playlist;
      this.songsPlaylist = playlist.songsPlaylists;
    });

    this.$songPlaylistSubscription = this.hubService.getSongPlaylist().subscribe(songPlaylist => {
      // this.getPlaylist(this.id);
    });

    this.$songsPlaylistSubscription = this.hubService.getSongsPlaylist().subscribe(songsPlaylist => {
      this.songsPlaylist = songsPlaylist;
    });

    this.route.params.subscribe(params => {
      const id = params['id']; // + params converts id to numbers
      if (id != null) {
        this.getPlaylist(id);
      }
    });

   }

  ngOnInit(): void {
    this.hubService.requestSongsPlaylist(this.playlist.id);
  }

  ngOnDestroy(): void {
    this.$playlistSubscription.unsubscribe();
    this.$songPlaylistSubscription.unsubscribe();
    this.$songsPlaylistSubscription.unsubscribe();
  }

  savePlaylist(): void {
    const request: IPlaylistCreateRequest = {
      id: this.id,
      playlistName: this.playlistName,
      isSelected: this.isSelected
    };
    this.hubService.savePlaylist(request);

    this.router.navigateByUrl('/playlists');
    // this.location.back();
  }

  getPlaylist(id: string): void {
    this.hubService.requestPlaylist(id);
  }

  removeSongFromPlaylist(songPlaylist: ISongPlaylist): void {
    this.hubService.deleteSongPlaylist(songPlaylist.id);
  }

  // back(): void {
  //   this.location.back();
  // }
}
