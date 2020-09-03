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
   }

  ngOnDestroy(): void {
    this.$chatMessageSubscription.unsubscribe();
  }

  ngOnInit(): void {
    // this.hubService.connectToHub('https://localhost:44315/chathub');
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
