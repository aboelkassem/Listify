import { RoomHubService } from './../../services/room-hub.service';
import { Subscription } from 'rxjs';
import { IRoom } from './../../interfaces';
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

  removeSongFromQueue(songQueued: ISongQueued): void {

  }

}
