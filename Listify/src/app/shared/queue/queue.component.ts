import { RoomHubService } from './../../services/room-hub.service';
import { Subscription } from 'rxjs';
// tslint:disable-next-line:max-line-length
import { IRoom, IWagerQuantitySongQueuedRequest, IApplicationUserRoomCurrencyRoom, IRoomInformation, ISongQueued, IConfirmationModalData } from './../../interfaces';
import { Component, OnInit, Input, OnDestroy, ViewChild } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { MatPaginator } from '@angular/material/paginator';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationmodalComponent } from '../modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-queue',
  templateUrl: './queue.component.html',
  styleUrls: ['./queue.component.css']
})
export class QueueComponent implements OnInit, OnDestroy {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns: string[] = ['songName', 'weightedValue', 'applicationUserRoomCurrency', 'quantityWagered', 'addQuantityToSongQueued', 'removeFromQueue'];
  dataSource = new MatTableDataSource<ISongQueued>();

  loading = false;

  @Input() room: IRoom;

  songsQueued: ISongQueued[] = [];
  applicationUserRoomCurrencies: IApplicationUserRoomCurrencyRoom[] = [];
  applicationUserRoomCurrency: IApplicationUserRoomCurrencyRoom;
  isRoomOwner: boolean;

  $songsQueuedSubscription: Subscription;
  $roomReceivedSubscription: Subscription;
  $applicationUserRoomCurrenciesReceived: Subscription;
  $applicationUserRoomCurrencyReceived: Subscription;

  constructor(
    private roomService: RoomHubService,
    private confirmationModal: MatDialog,
    private toastrService: ToastrService) {

    this.$songsQueuedSubscription = this.roomService.getSongsQueued().subscribe((songsQueued: ISongQueued[]) => {
      this.loading = false;

      songsQueued.forEach(element => {
        const minimumValue = Math.min(...element.song.songThumbnails.map(s => s.sizeX));
        element.song.songThumbnailSelected = element.song.songThumbnails.filter(x => x.sizeX === minimumValue)[0];
      });

      this.songsQueued = songsQueued;
      this.dataSource.data = this.songsQueued;
      this.loading = false;
    });

    this.$roomReceivedSubscription = this.roomService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
      this.isRoomOwner = roomInformation.applicationUserRoom.isOwner;
      this.room = roomInformation.room;
      this.applicationUserRoomCurrencies = roomInformation.applicationUserRoomCurrenciesRoom;
    });

    // tslint:disable-next-line:max-line-length
    this.$applicationUserRoomCurrenciesReceived = this.roomService.getApplicationUserRoomCurrenciesRoom().subscribe(applicationUserRoomCurrencies => {
      this.applicationUserRoomCurrencies = applicationUserRoomCurrencies;
    });

    // tslint:disable-next-line:max-line-length
    this.$applicationUserRoomCurrencyReceived = this.roomService.getApplicationUserRoomCurrencyRoom().subscribe(applicationUserRoomCurrency => {
      const applicationUserRoomCurrencySelected = this.applicationUserRoomCurrencies
        .filter(x => x.id === applicationUserRoomCurrency.id)[0];

      if (applicationUserRoomCurrencySelected) {
        this.applicationUserRoomCurrencies[this.applicationUserRoomCurrencies.indexOf(applicationUserRoomCurrency)]
          = applicationUserRoomCurrency;

        this.applicationUserRoomCurrency = applicationUserRoomCurrencySelected;
      }
    });
  }

  ngOnInit(): void {
    if (this.roomService.isConnected) {
      this.roomService.requestSongsQueued(this.room.id);
    }

    this.dataSource.paginator = this.paginator;
  }

  ngOnDestroy(): void {
    this.$songsQueuedSubscription.unsubscribe();
    this.$roomReceivedSubscription.unsubscribe();
    this.$applicationUserRoomCurrenciesReceived.unsubscribe();
    this.$applicationUserRoomCurrencyReceived.unsubscribe();
  }

  addQuantityToSongQueued(songQueued: ISongQueued): void {
    if (songQueued.quantityWagered <= 0 || songQueued.quantityWagered === undefined || songQueued.quantityWagered === null) {
      this.toastrService.warning('You must wager more than 0 on a song in the queue, please try again',
      'Not Enough Wagered');
    }else {
      const applicationUserRoomCurrency = this.roomService.applicationUserRoomCurrenciesRoom
      .filter(x => x.id === songQueued.applicationUserRoomCurrencyId)[0];

      if (applicationUserRoomCurrency !== undefined && applicationUserRoomCurrency !== null) {
        if (songQueued.quantityWagered > applicationUserRoomCurrency.quantity) {
          this.toastrService.warning('You do not have enough ' + applicationUserRoomCurrency.currencyRoom.currency.currencyName + ' for this action, you have ' + applicationUserRoomCurrency.quantity + ' available',
          'Not Enough Currency');
        }else {
          const request: IWagerQuantitySongQueuedRequest = {
            songQueued: songQueued,
            applicationUserRoom: this.roomService.applicationUserRoom,
            applicationUserRoomCurrencyRoom: applicationUserRoomCurrency,
            quantity: songQueued.quantityWagered
          };

          this.roomService.wagerQuantitySongQueued(request);
          this.toastrService.success('you have added more points to ' + request.songQueued.song.songName, request.quantity + ' added to the song')
        }
      }else {
        this.toastrService.warning('You must select a type of currency to wager on a song in the queue, please try again',
        'Invalid Selected Currency');
      }
    }
  }

  clearQueue(): void {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Clear the queue ?',
      message: 'Are your sure you want to remove this clear the queue and refund all currency spent on the songs?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
      width: '250px',
      data: confirmationModalData
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.loading = true;
        this.roomService.requestQueueClear();
        this.toastrService.success('You have added cleared the entry queue.', 'Clear Success');
      }
    });
  }

  removeFromQueue(songQueued: ISongQueued): void {
    if (this.roomService.applicationUserRoom.isOwner) {
      const confirmationModalData: IConfirmationModalData = {
        title: 'Are your sure ?',
        message: 'Are your sure you want to remove this song from the queue and refund all currency spent?',
        isConfirmed: false,
        confirmMessage: 'Confirm',
        cancelMessage: 'Cancel'
      };

      const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
        width: '250px',
        data: confirmationModalData
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.loading = true;
          this.roomService.removeSongFromQueue(songQueued);
          this.toastrService.success('You have removed ' + songQueued.song.songName + ' from the queue', 'Removed success');
        }
      });
    }
  }
}
