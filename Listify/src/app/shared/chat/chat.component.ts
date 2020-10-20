import { Subscription } from 'rxjs';
import { RoomHubService } from './../../services/room-hub.service';
import { HubService } from './../../services/hub.service';
import { IChatMessage } from './../../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['username', 'message'];
  dataSource = new MatTableDataSource<IChatMessage>();

  $chatMessageSubscription: Subscription;
  $updatedChatColorSubscription: Subscription;

  messages: IChatMessage[] = [];
  message = '';

  constructor(private roomService: RoomHubService) {
    this.$chatMessageSubscription = this.roomService.getMessageReceived().subscribe(message => {
      if (this.messages.length > 50) {
        this.messages.splice(0, 1);
      }

      this.messages.push(message);
      this.dataSource.data = this.messages;

      // this.receiveMessage(message);
    });

    this.$updatedChatColorSubscription = this.roomService.getUpdatedChatColor().subscribe(applicationUser => {
      if (this.roomService.applicationUserRoom.applicationUser.id === applicationUser.id) {
        this.roomService.applicationUserRoom.applicationUser.chatColor = applicationUser.chatColor;
      }

      this.messages.forEach(s => {
        if (s.applicationUserRoom.applicationUser.id === applicationUser.id) {
          s.applicationUserRoom.applicationUser.chatColor = applicationUser.chatColor;
        }
      });
    });
   }

  ngOnDestroy(): void {
    this.$chatMessageSubscription.unsubscribe();
    this.$updatedChatColorSubscription.unsubscribe();
  }

  ngOnInit(): void {
    // this.hubService.connectToHub(this.globalsService.developmentWebAPIUrl + 'chathub');
    // this.dataSource.data = this.messages;
  }

  sendMessage(): void {
    const message: IChatMessage = {
      applicationUserRoom: this.roomService.applicationUserRoom,
      message: this.message
    };
    this.roomService.sendMessage(message);
    this.message = '';
  }

  // receiveMessage(message: IChatMessage): void {
  // }
}
