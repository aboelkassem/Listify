import { HubService } from './../hub.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit, OnDestroy {

  username: string;
  songPoolCountMax: number;
  playlistCount: number;
  subscription: Subscription;

  constructor(
    private hubService: HubService) {
      this.subscription = this.hubService.getUserInfo().subscribe(user => {
        this.username = user.username;
        this.songPoolCountMax = user.songPoolCountSongsMax;
        this.playlistCount = user.playlistCountMax;
      });
    }

  ngOnInit(): void {
    this.hubService.requestUserInformation();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  submitUserData(): void {

  }
}
