import { Subscription } from 'rxjs';
import { ChatService } from './../chat/chat.service';
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
    private chatService: ChatService) {
      this.subscription = this.chatService.getUserInfo().subscribe(user => {
        this.username = user.username;
        this.songPoolCountMax = user.songPoolCountMax;
        this.playlistCount = user.playlistCount;
      });
    }

  ngOnInit(): void {
    this.chatService.requestUserInformation();
  }

  ngOnDestroy(): void {
    this.subscription.unsubscribe();
  }

  submitUserData(): void {

  }
}
