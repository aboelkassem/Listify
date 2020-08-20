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
  ) {
    this.$songsQueuedSubscription = this.hubService.getQueue().subscribe((songsQueued: ISongQueued[]) => {
      this.songsQueued = songsQueued;
    });
  }

  ngOnInit(): void {
    this.hubService.requestQueue(this.room);
  }

  ngOnDestroy(): void {
    this.$songsQueuedSubscription.unsubscribe();
  }

  removeSongFromQueue(songQueued: ISongQueued): void {

  }

}
