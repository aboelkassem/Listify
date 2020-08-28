import { RoomHubService } from './../../services/room-hub.service';
import { Subscription } from 'rxjs';
import { IRoom, IWagerQuantitySongQueuedRequest, IApplicationUserRoomCurrency } from './../../interfaces';
import { HubService } from './../../services/hub.service';
import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { ISongQueued } from 'src/app/interfaces';

@Component({
  selector: 'app-queue',
  templateUrl: './queue.component.html',
  styleUrls: ['./queue.component.css']
})
export class QueueComponent implements OnInit, OnDestroy {

  @Input() room: IRoom;

  songsQueued: ISongQueued[] = [];
  applicationUserRoomCurrencies: IApplicationUserRoomCurrency[] = this.roomService.applicationUserRoomCurrencies;
  quantityWagered: number;

  $songsQueuedSubscription: Subscription;


  constructor(
    private hubService: HubService,
    private roomService: RoomHubService) {

    this.$songsQueuedSubscription = this.roomService.getSongsQueued().subscribe((songsQueued: ISongQueued[]) => {
      this.songsQueued = songsQueued;
    });
  }

  ngOnInit(): void {
    if (this.hubService.isConnected) {
      this.hubService.requestSongsQueued(this.room.id);
    }
  }

  ngOnDestroy(): void {
    this.$songsQueuedSubscription.unsubscribe();
  }

  addQuantityToSongQueued(songQueued: ISongQueued, applicationUserRoomCurrencyId: string): void {
    // tslint:disable-next-line:max-line-length
    const applicationUserRoomCurrency = this.roomService.applicationUserRoomCurrencies.filter(x => x.id === applicationUserRoomCurrencyId)[0];
    if (applicationUserRoomCurrency !== undefined && applicationUserRoomCurrency !== null) {
      const request: IWagerQuantitySongQueuedRequest = {
        songQueued: songQueued,
        applicationUserRoom: this.roomService.applicationUserRoom,
        applicationUserRoomCurrency: applicationUserRoomCurrency,
        quantity: this.quantityWagered
      };

      this.roomService.wagerQuantitySongQueued(request);
    }
  }

  removeSongFromQueue(songQueued: ISongQueued): void {

  }

}
