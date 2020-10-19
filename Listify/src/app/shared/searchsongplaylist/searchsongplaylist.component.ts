import { ToastrService } from 'ngx-toastr';
import { ISongSearchResult, ISong, ISongSearchResults, ISongPlaylist, ISongPlaylistCreateRequest } from './../../interfaces';
import { Component, OnInit, OnDestroy, Input, ViewChild, AfterViewInit } from '@angular/core';
import { IPlaylist } from 'src/app/interfaces';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Router } from '@angular/router';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-searchsongplaylist',
  templateUrl: './searchsongplaylist.component.html',
  styleUrls: ['./searchsongplaylist.component.css']
})
export class SearchsongplaylistComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['songThumbnail', 'songName', 'addToPlaylist'];
  dataSource = new MatTableDataSource<ISongSearchResult>();


  @Input() playlist: IPlaylist;

  searchSnippet: string;
  songSearchResults: ISongSearchResult[] = [];
  song: ISong;

  private $searchYoutubeSubscription: Subscription;
  private $songPlaylistSubscription: Subscription;

  constructor(
    private hubService: HubService, private toastrService: ToastrService) {
      this.$searchYoutubeSubscription = this.hubService.getSearchYoutube().subscribe((songSearchResults: ISongSearchResults) => {
        this.songSearchResults = songSearchResults.results;
        this.dataSource.data = this.songSearchResults;
        this.dataSource.paginator = this.paginator;
        this.dataSource.sort = this.sort;
      });

      this.$songPlaylistSubscription = this.hubService.getSongPlaylist().subscribe((songPlaylist: ISongPlaylist) => {
        this.searchSnippet = '';
        this.songSearchResults = [];
      });

     }

  ngOnInit(): void {
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
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
      this.toastrService.success('you have successfully added ' + request.songSearchResult.songName + 'to your playlist'
        , 'Song Added to Playlist');
      this.clearSearch();
    }
  }

  clearSearch(): void {
    this.songSearchResults = [];
    this.searchSnippet = '';
  }
}
