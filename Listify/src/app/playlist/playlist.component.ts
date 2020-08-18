import { Subscription } from 'rxjs';
import { IPlaylistCreateRequest, IPlaylist } from './../interfaces';
import { HubService } from './../hub.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-playlist',
  templateUrl: './playlist.component.html',
  styleUrls: ['./playlist.component.css']
})
export class PlaylistComponent implements OnInit {

  // @Input() playlist: IPlaylist;
  id: string;
  playlistName: string;
  isSelected: boolean;

  getPlaylistSub: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private route: ActivatedRoute) {
    this.getPlaylistSub = this.hubService.getPlaylist().subscribe(playlist => {
      this.id = playlist.id;
      this.playlistName = playlist.playlistName;
      this.isSelected = playlist.isSelected;
    });

    this.route.queryParams.subscribe(params => {
      const id = params['id'];
      if (id != null) {
        this.getPlaylist(id);
      }
    });

   }

  ngOnInit(): void {
  }

  savePlaylist(): void {
    const request: IPlaylistCreateRequest = {
      id: this.id,
      playlistName: this.playlistName,
      isSelected: this.isSelected
    };
    this.hubService.savePlaylist(request);

    // returning back/playlists-page after creating playlist
    this.router.navigateByUrl('/playlists');
    // this.location.back();
  }

  getPlaylist(id: string): void {
    this.hubService.requestPlaylist(id);
  }

  // back(): void {
  //   this.location.back();
  // }
}
