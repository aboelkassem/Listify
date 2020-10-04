import { ProfileService } from './../../services/profile.service';
import { Component, OnInit, ViewChild, OnDestroy, Input } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { IGenre, IInputModalData, IRoom } from '../../interfaces';
import { HubService } from '../../services/hub.service';
import { RoomHubService } from '../../services/room-hub.service';
import { InputmodalComponent } from '../../shared/modals/inputmodal/inputmodal.component';

@Component({
  selector: 'app-roomsfollowed',
  templateUrl: './roomsfollowed.component.html',
  styleUrls: ['./roomsfollowed.component.css']
})
export class RoomsfollowedComponent implements OnInit, OnDestroy {

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

  genres: IGenre[] = [];
  genreSelectedId: string;

  $roomsFollowsSubscription: Subscription;
  $profileChanged: Subscription;
  $genresReceivedSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private roomService: RoomHubService,
    private profileService: ProfileService,
    private matDialog: MatDialog,
    private router: Router) {
    this.$genresReceivedSubscription = this.hubService.getGenres().subscribe(genres => {
      this.genres = genres;
    });

    this.$profileChanged = this.profileService.getApplicationUserId().subscribe(applicationUserId => {
      if (applicationUserId !== undefined && applicationUserId !== null) {
        this.hubService.requestRoomsFollowedProfile(applicationUserId);
      }else {
        this.hubService.requestRoomsFollowed();
      }
    });

    this.$roomsFollowsSubscription = this.hubService.getRoomsFollowed().subscribe(rooms => {
      this.rooms = rooms;
      this.dataSource.data = this.rooms;
    });
  }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;

    this.hubService.requestGenres();

    if (this.profileService.applicationUserId !== undefined && this.profileService.applicationUserId !== null) {
      this.hubService.requestRoomsFollowedProfile(this.profileService.applicationUserId);
    }else {
      this.hubService.requestRoomsFollowed();
    }
  }

  ngOnDestroy(): void {
    this.$roomsFollowsSubscription.unsubscribe();
    this.$profileChanged.unsubscribe();
    this.$genresReceivedSubscription.unsubscribe();
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
          this.hubService.requestAuthToLockedRoom(result.data, room.id);
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
