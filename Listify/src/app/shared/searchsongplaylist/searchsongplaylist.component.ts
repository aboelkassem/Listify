import { ISongSearchResult, ISong, ISongSearchResults, ISongPlaylist, ISongPlaylistCreateRequest } from './../../interfaces';
import { Component, OnInit, OnDestroy, Input, ViewChild } from '@angular/core';
import { IPlaylist } from 'src/app/interfaces';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-searchsongplaylist',
  templateUrl: './searchsongplaylist.component.html',
  styleUrls: ['./searchsongplaylist.component.css']
})
export class SearchsongplaylistComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['songName', 'addToPlaylist'];
  dataSource = new MatTableDataSource<ISongSearchResult>();


  @Input() playlist: IPlaylist;

  searchSnippet: string;
  songSearchResults: ISongSearchResult[] = [];
  song: ISong;

  private $searchYoutubeSubscription: Subscription;
  private $songPlaylistSubscription: Subscription;

  constructor(
    private hubService: HubService) {
      this.$searchYoutubeSubscription = this.hubService.getSearchYoutube().subscribe((songSearchResults: ISongSearchResults) => {
        this.songSearchResults = songSearchResults.results;
        this.dataSource.data = this.songSearchResults;
      });

      this.$songPlaylistSubscription = this.hubService.getSongPlaylist().subscribe((songPlaylist: ISongPlaylist) => {
        this.searchSnippet = '';
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
    if (this.searchSnippet !== null && this.searchSnippet !== undefined) {
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

      this.clearSearch();
    }
  }

  clearSearch(): void {
    this.songSearchResults = [];
    this.searchSnippet = '';
  }
}
