import { Subscription } from 'rxjs';
import { HubService } from './../hub.service';
import { Component, OnInit } from '@angular/core';
import { IPlaylist } from '../interfaces';

@Component({
  selector: 'app-playlists',
  templateUrl: './playlists.component.html',
  styleUrls: ['./playlists.component.css']
})
export class PlaylistsComponent implements OnInit {

  playlists: IPlaylist[] = [];

  subscription: Subscription;

  constructor(private hubService: HubService) {
    this.subscription = this.hubService.getPlaylists().subscribe(playlists => {
      this.playlists = playlists;
    });
  }

  ngOnInit(): void {
    this.hubService.requestPlaylists();
  }

  deletePlaylist(id: string): void {
    this.hubService.deletePlaylist(id);
    this.hubService.requestPlaylists();
  }
}
