import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { RoomkeymodalComponent } from './../shared/roomkeymodal/roomkeymodal.component';
import { HubService } from './../services/hub.service';
import { IRoom } from './../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['roomName', 'roomTitle', 'usersOnline', 'isLocked', 'matureContent'];
  dataSource = new MatTableDataSource<IRoom>();

  rooms: IRoom[] = [];
  // roomCurrent: IRoom;

  $roomsSubscription: Subscription;
  $authToLockedRoomResponse: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private toastrService: ToastrService,
    private roomKeyConfirmationModal: MatDialog) {
    this.$roomsSubscription = this.hubService.getRooms().subscribe(rooms => {
      this.rooms = rooms;
      this.dataSource.data = this.rooms;
    });

    this.$authToLockedRoomResponse = this.hubService.getAuthToLockedRoom().subscribe(authToLockedRoomResponse => {
      // success = 0 , fail = 1
      if (authToLockedRoomResponse.authToLockedRoomResponseType === 0) {
        this.router.navigate(['/', authToLockedRoomResponse.room.roomCode]);
      }else {
        this.toastrService.error('You were not authorize into room ' + authToLockedRoomResponse.room.roomCode + '.', 'Incorrect Room Key');
      }
    });

    // this.hubService.requestRooms();
  }

  ngOnInit(): void {
    this.hubService.requestRooms();
  }

  ngOnDestroy(): void {
    this.$roomsSubscription.unsubscribe();
    this.$authToLockedRoomResponse.unsubscribe();
  }

  navigateToRoom(room: IRoom): void {
    if (room.isRoomLocked) {

      const confirmationModal = this.roomKeyConfirmationModal.open(RoomkeymodalComponent, {
        width: '250px',
        data: {roomKey : ''}
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.hubService.requestAuthToLockedRoom(result, room.id);
        }
      });

    }else {
      this.router.navigate(['/', room.roomCode]);
    }
  }
}
