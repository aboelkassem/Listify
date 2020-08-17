import { ChatService } from './chat.service';
import { IChatMessage } from './../interfaces';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {


  messages: IChatMessage[] = this.chatService.messages;
  message = '';

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.chatService.connectToHub('https://localhost:44315/chathub');
  }

  sendMessage(): void {
    this.chatService.sendMessage(this.message);
    this.message = '';
  }

}
