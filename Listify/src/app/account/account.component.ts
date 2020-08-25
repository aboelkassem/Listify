import { IApplicationUserRequest } from './../interfaces';
import { HubService } from './../services/hub.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit, OnDestroy {

  id: string;
  username: string;
  roomCode: string;
  songPoolCountMax: number;
  playlistCount: number;

  $userInformationSubscription: Subscription;

  constructor(
    private hubService: HubService) {
      this.$userInformationSubscription = this.hubService.getApplicationUser().subscribe(user => {
        this.id = user.id;
        this.username = user.username;
        this.songPoolCountMax = user.songPoolCountSongsMax;
        this.playlistCount = user.playlistCountMax;
        this.roomCode = user.room.roomCode;
      });
    }

  ngOnInit(): void {
    this.hubService.requestApplicationUserInformation();
  }

  ngOnDestroy(): void {
    this.$userInformationSubscription.unsubscribe();
  }

  saveApplicationUserInfo(): void {
    const request: IApplicationUserRequest = {
      id: this.id,
      username: this.username,
      songPoolCountSongsMax: this.songPoolCountMax,
      playlistCountMax: this.playlistCount,
      roomCode: this.roomCode
    };

    this.hubService.updateApplicationUserInformation(request);
  }
}
