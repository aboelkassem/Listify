import { MatDialog } from '@angular/material/dialog';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { IConfirmationModalData, IPlaylist, IPlaylistCommunity } from '../interfaces';
import { ConfirmationmodalComponent } from '../shared/modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-playlistscommunity',
  templateUrl: './playlistscommunity.component.html',
  styleUrls: ['./playlistscommunity.component.css']
})
export class PlaylistscommunityComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['playlistName', 'owner', 'numberOfSongs', 'genreName', 'queuePlaylist'];
  dataSource = new MatTableDataSource<IPlaylistCommunity>();
  playlists: IPlaylistCommunity[] = [];

  $playlistsCommunityReceivedSubscription: Subscription;

  constructor(private hubService: HubService, private confirmationModal: MatDialog) {
    this.$playlistsCommunityReceivedSubscription = this.hubService.getPlaylistsCommunity().subscribe(playlists => {
      this.playlists = playlists;
      this.dataSource.data = this.playlists;
    });
  }

  ngOnInit(): void {
    this.dataSource.sort = this.sort;
    this.dataSource.paginator = this.paginator;

    this.hubService.requestPlaylistsCommunity();
  }

  ngOnDestroy(): void {
    this.$playlistsCommunityReceivedSubscription.unsubscribe();
  }

  queuePlaylist(id: string): void {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Are your sure ?',
      message: 'Are your sure you want to add the entire playlist to your queue?',
      isConfirmed: false
    };

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
      width: '250px',
      data: confirmationModalData
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.requestQueuePlaylistInRoomHome(id);
      }
    });
  }
}
