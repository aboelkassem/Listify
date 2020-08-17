import { HubService } from './../hub.service';
import { IRoom } from './../interfaces';
import { Component, OnInit } from '@angular/core';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit {

  rooms: IRoom[] = [];
  roomCurrent: IRoom;
  subscription: Subscription;

  constructor(private hubService: HubService) {
    this.subscription = this.hubService.getRooms().subscribe(rooms => {
      this.rooms = rooms;
    });

    // this.hubService.requestRooms();
  }

  ngOnInit(): void {
    this.hubService.requestRooms();
  }

}
