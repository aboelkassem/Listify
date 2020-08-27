import { RoomHubService } from './../../services/room-hub.service';
import { HubService } from './../../services/hub.service';
import { IChatMessage } from './../../interfaces';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {

  messages: IChatMessage[] = this.roomService.messages;
  message = '';

  constructor(private roomService: RoomHubService) { }

  ngOnInit(): void {
    // this.hubService.connectToHub('https://localhost:44315/chathub');
  }

  sendMessage(): void {
    const message: IChatMessage = {
      applicationUserRoom: this.roomService.applicationUserRoom,
      message: this.message
    };
    this.roomService.sendMessage(message);
    this.message = '';
  }

}
