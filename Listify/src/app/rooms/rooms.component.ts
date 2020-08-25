import { HubService } from './../services/hub.service';
import { IRoom } from './../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit, OnDestroy {

  rooms: IRoom[] = [];
  roomCurrent: IRoom;

  $RoomsSubscription: Subscription;

  constructor(private hubService: HubService) {
    this.$RoomsSubscription = this.hubService.getRooms().subscribe(rooms => {
      this.rooms = rooms;
    });

    // this.hubService.requestRooms();
  }

  ngOnInit(): void {
    this.hubService.requestRooms();
  }

  ngOnDestroy(): void {
    this.$RoomsSubscription.unsubscribe();
  }
}
