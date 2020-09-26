import { MatSort } from '@angular/material/sort';
import { Component, OnInit, OnDestroy, ViewChild, Input } from '@angular/core';
import { IConfirmationModalData, ISongPlaylist } from 'src/app/interfaces';
import { HubService } from 'src/app/services/hub.service';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { ConfirmationmodalComponent } from '../modals/confirmationmodal/confirmationmodal.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-songsplaylist',
  templateUrl: './songsplaylist.component.html',
  styleUrls: ['./songsplaylist.component.css']
})
export class SongsplaylistComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;
  displayedColumns: string[] = ['songName', 'songLengthSec', 'removeSongPlaylist'];
  dataSource = new MatTableDataSource<ISongPlaylist>();

  @Input() isOwner: boolean;

  maxNumberOfSongs: string = this.hubService.applicationUser.playlistSongCount.toString();
  songsPlaylist: ISongPlaylist[] = [];

  $playlistSubscription: Subscription;
  $applicationUserSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private confirmationModal: MatDialog) {
      this.$applicationUserSubscription = this.hubService.getApplicationUser().subscribe(applicationUser => {
        this.maxNumberOfSongs = applicationUser.playlistSongCount.toString();
      });

      this.$playlistSubscription = this.hubService.getPlaylist().subscribe(playlist => {
        if (playlist.songsPlaylist) {
          this.songsPlaylist = playlist.songsPlaylist;
          this.dataSource.data = this.songsPlaylist;
          this.dataSource.sort = this.sort;
        }
      });
  }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.$playlistSubscription.unsubscribe();
    this.$applicationUserSubscription.unsubscribe();
  }

  removeSongFromPlaylist(songPlaylist: ISongPlaylist): void {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Are your sure ?',
      message: 'Are your sure you want to remove ' + songPlaylist.song.songName + ' from your playlist?',
      isConfirmed: false
    };

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
      width: '250px',
      data: confirmationModalData
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.deleteSongPlaylist(songPlaylist.id);
        this.hubService.requestPlaylist(songPlaylist.playlist.id);
      }
    });
  }
}
