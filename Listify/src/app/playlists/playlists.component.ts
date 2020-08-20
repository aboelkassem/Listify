import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { IPlaylist } from '../interfaces';

@Component({
  selector: 'app-playlists',
  templateUrl: './playlists.component.html',
  styleUrls: ['./playlists.component.css']
})
export class PlaylistsComponent implements OnInit, OnDestroy {

  playlists: IPlaylist[] = [];

  $playlistsSubscription: Subscription;

  constructor(private hubService: HubService) {
    this.$playlistsSubscription = this.hubService.getPlaylists().subscribe(playlists => {
      this.playlists = playlists;
    });
  }

  ngOnInit(): void {
    this.hubService.requestPlaylists();
  }
  ngOnDestroy(): void {
    this.$playlistsSubscription.unsubscribe();
  }

  deletePlaylist(id: string): void {
    this.hubService.deletePlaylist(id);
    this.hubService.requestPlaylists();
  }
}
