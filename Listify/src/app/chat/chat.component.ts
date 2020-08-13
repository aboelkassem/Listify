import { ChatService } from './chat.service';
import { IMessage } from './../interfaces';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {


  messages: IMessage[] = this.chatService.messages;
  message: string = '';

  constructor(private chatService: ChatService) { }

  ngOnInit(): void {
    this.chatService.connectToHub('https://localhost:44361/chathub');
  }

  sendMessage(): void {
    this.chatService.sendMessage(this.message);
  }

}
