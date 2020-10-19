import { IGenre, IInputModalData } from 'src/app/interfaces';
import { RoomHubService } from 'src/app/services/room-hub.service';
import { Router } from '@angular/router';
import { MatDialog } from '@angular/material/dialog';
import { InputmodalComponent } from './../shared/modals/inputmodal/inputmodal.component';
import { HubService } from './../services/hub.service';
import { IRoom } from './../interfaces';
import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { Subscription } from 'rxjs';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { MatSort } from '@angular/material/sort';
import { MatPaginator } from '@angular/material/paginator';

@Component({
  selector: 'app-rooms',
  templateUrl: './rooms.component.html',
  styleUrls: ['./rooms.component.css']
})
export class RoomsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = [
    'roomImageUrl',
    'roomName',
    'roomTitle',
    'username',
    'profileImageUrl',
    'usersOnline',
    'numberFollows',
    'genres',
    'isLocked',
    'matureContent'];
  dataSource = new MatTableDataSource<IRoom>();

  rooms: IRoom[] = [];
  roomCode: string;
  loading = false;

  genres: IGenre[] = [];
  genreSelectedId: string;

  $roomsSubscription: Subscription;
  $roomSubscription: Subscription;
  $authToLockedRoomResponse: Subscription;
  $genresReceivedSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private roomService: RoomHubService,
    private matDialog: MatDialog,
    private router: Router,
    private toastrService: ToastrService) {
    this.$genresReceivedSubscription = this.hubService.getGenres().subscribe(genres => {
      this.genres = genres;
    });

    this.$roomSubscription = this.hubService.getRoom().subscribe(room => {
      if (room) {
        if (room.isRoomOnline) {
          this.navigateToRoom(room);
        }else {
          this.toastrService.error(room.roomCode + ' is not currently online. please try again later', 'Room Offline');
        }
      }else {
        this.toastrService.error('That room was not found. please try again', 'Room Not Found');
      }
    });

    this.$roomsSubscription = this.hubService.getRooms().subscribe(rooms => {
      this.loading = false;
      this.rooms = rooms;
      this.dataSource.data = this.rooms;
    });

    this.$authToLockedRoomResponse = this.hubService.getAuthToLockedRoom().subscribe(authToLockedRoomResponse => {
      // success = 0 , fail = 1
      if (authToLockedRoomResponse.authToLockedRoomResponseType === 0) {
        this.roomService.requestRoomChange(authToLockedRoomResponse.room);
        this.router.navigate(['/', authToLockedRoomResponse.room.roomCode]);
      }else {
        this.toastrService.error('You were not authorize into room ' + authToLockedRoomResponse.room.roomCode + '.', 'Incorrect Room Key');
      }
    });

    // this.hubService.requestRooms();
  }

  ngOnInit(): void {
    this.loading = true;
    this.hubService.requestRooms();
    this.hubService.requestGenres();
    // this.profileService.setApplicationUserId('');
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.$roomsSubscription.unsubscribe();
    this.$roomSubscription.unsubscribe();
    this.$authToLockedRoomResponse.unsubscribe();
    this.$genresReceivedSubscription.unsubscribe();
  }

  requestRoom(): void {
    this.hubService.requestRoom(this.roomCode);
  }

  navigateToRoom(room: IRoom): void {
    if (room.isRoomLocked) {
      const inputModalData: IInputModalData = {
        title: 'Room Key',
        message: 'Room Locked! Please Enter Room Key to enter this room',
        placeholder: 'Enter room key ....',
        data: ''
      };

      const confirmationModal = this.matDialog.open(InputmodalComponent, {
        width: '250px',
        data: inputModalData
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.hubService.requestAuthToLockedRoom(result, room.id);
        }
      });

    }else {
      this.roomService.requestRoomChange(room);
      this.router.navigate(['/', room.roomCode]);
    }
  }

  genreChanged(id: string): void {
    if (id === undefined || id === null || id === '') {
      this.dataSource.data = this.rooms;
    }else {
      const rooms: IRoom[] = [];

      this.rooms.forEach(room => {
        room.roomGenres.forEach(roomGenreFiltered => {
          if (roomGenreFiltered.genre.id === id) {
            rooms.push(room);
            return;
          }
        });
      });

      this.dataSource.data = rooms;
    }
  }

  clearFilter(): void {
    this.dataSource.data = this.rooms;
    this.genreSelectedId = '';
  }
}
