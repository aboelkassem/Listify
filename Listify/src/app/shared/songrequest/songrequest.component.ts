import { Router } from '@angular/router';
import { HubService } from './../../services/hub.service';
import { Subscription } from 'rxjs';
import { IPlaylist, ISongSearchResult, ISong, ISongSearchResults, ISongPlaylist, ISongPlaylistCreateRequest } from './../../interfaces';
import { Component, OnInit, Input, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-songrequest',
  templateUrl: './songrequest.component.html',
  styleUrls: ['./songrequest.component.css']
})
export class SongrequestComponent implements OnInit, OnDestroy {

  @Input() playlist: IPlaylist;

  searchSnippet: string;
  songSearchResults: ISongSearchResult[] = [];
  song: ISong;

  private $searchYoutubeSubscription: Subscription;
  private $songPlaylistSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router) {
      this.$searchYoutubeSubscription = this.hubService.getSearchYoutube().subscribe((songSearchResults: ISongSearchResults) => {
        this.songSearchResults = songSearchResults.results;
      });

      this.$songPlaylistSubscription = this.hubService.getSongPlaylist().subscribe((songPlaylist: ISongPlaylist) => {
        this.songSearchResults = [];
      });

     }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.$searchYoutubeSubscription.unsubscribe();
    this.$songPlaylistSubscription.unsubscribe();
  }

  searchForSong(): void {
    if (this.searchSnippet != null) {
      this.hubService.requestSearchYoutube(this.searchSnippet);
    }
  }

  addSongToPlaylist(searchResult: ISongSearchResult): void {
    if (searchResult != null) {
      const request: ISongPlaylistCreateRequest = {
        playlistId: this.playlist.id,
        songSearchResult: searchResult
      };
      this.hubService.saveSongPlaylist(request);
    }
  }
}
