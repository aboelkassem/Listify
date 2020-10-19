import { IPlaylist } from './../interfaces';
import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, ViewChild, OnDestroy, AfterViewInit } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { IConfirmationModalData, IGenre } from '../interfaces';
import { ConfirmationmodalComponent } from '../shared/modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-playlistscommunity',
  templateUrl: './playlistscommunity.component.html',
  styleUrls: ['./playlistscommunity.component.css']
})
export class PlaylistscommunityComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['playlistImageUrl', 'playlistName', 'owner', 'profileImageUrl', 'numberOfSongs', 'genreName', 'queuePlaylist'];
  dataSource = new MatTableDataSource<IPlaylist>();
  playlists: IPlaylist[] = [];
  genres: IGenre[] = [];
  genreSelectedId: string;

  loading = false;

  $genresReceivedSubscription: Subscription;
  $playlistsCommunityReceivedSubscription: Subscription;

  constructor(private hubService: HubService, private confirmationModal: MatDialog) {
    this.$genresReceivedSubscription = this.hubService.getGenres().subscribe(genres => {
      this.genres = genres;
    });

    this.$playlistsCommunityReceivedSubscription = this.hubService.getPlaylistsCommunity().subscribe(playlists => {
      this.playlists = playlists;
      this.dataSource.data = this.playlists;
      this.loading = false;
    });
  }

  ngOnInit(): void {
    this.loading = true;

    this.hubService.requestPlaylistsCommunity();
    this.hubService.requestGenres();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.$playlistsCommunityReceivedSubscription.unsubscribe();
    this.$genresReceivedSubscription.unsubscribe();
  }

  queuePlaylist(id: string): void {
    let confirmationModalData: IConfirmationModalData = {
      title: 'Are your sure ?',
      message: 'Are your sure you want to add the entire playlist to your queue?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    let confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
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

        confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
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
