import { HubService } from './../../services/hub.service';
import { IChatMessage } from './../../interfaces';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-chat',
  templateUrl: './chat.component.html',
  styleUrls: ['./chat.component.css']
})
export class ChatComponent implements OnInit {


  messages: IChatMessage[] = this.hubService.messages;
  message = '';

  constructor(private hubService: HubService) { }

  ngOnInit(): void {
    // this.hubService.connectToHub('https://localhost:44315/chathub');
  }

  sendMessage(): void {
    this.hubService.sendMessage(this.message);
    this.message = '';
  }

}
