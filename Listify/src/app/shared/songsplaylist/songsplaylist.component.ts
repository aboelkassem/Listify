import { ConfirmationmodalService } from './../../services/confirmationmodal.service';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ISongPlaylist } from 'src/app/interfaces';
import { HubService } from 'src/app/services/hub.service';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { ConfirmationmodalComponent } from '../confirmationmodal/confirmationmodal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-songsplaylist',
  templateUrl: './songsplaylist.component.html',
  styleUrls: ['./songsplaylist.component.css']
})
export class SongsplaylistComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns: string[] = ['songName', 'songLengthSec', 'removeSongPlaylist'];
  dataSource = new MatTableDataSource<ISongPlaylist>();


  songsPlaylist: ISongPlaylist[] = [];

  $playlistSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private confirmationModalService: ConfirmationmodalService,
    private confirmationModal: MatDialog) {
    this.$playlistSubscription = this.hubService.getPlaylist().subscribe(playlist => {
      if (playlist.songsPlaylist) {
        this.songsPlaylist = playlist.songsPlaylist;
        this.dataSource.data = this.songsPlaylist;
      }
    });
  }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  ngOnDestroy(): void {
    this.$playlistSubscription.unsubscribe();
  }

  removeSongFromPlaylist(songPlaylist: ISongPlaylist): void {
    this.confirmationModalService.setConfirmationModalMessage('remove ' + songPlaylist.song.songName + ' from playlist');

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
      width: '250px',
      data: {isConfirmed: false}
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.deleteSongPlaylist(songPlaylist.id);
        this.hubService.requestPlaylist(songPlaylist.playlist.id);
      }
    });
  }
}
