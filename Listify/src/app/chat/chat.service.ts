import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalR';
import { IUser, IRoom, IMessage, ISongQueuedCreateRequest, IChatData } from '../interfaces';
import { ActivatedRoute } from '@angular/router';
import { Subject } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  messages: IMessage[] = [];
  message: string;
  roomCurrent: IRoom;

  // event for asynchronous room for using in other places (observable)
  $roomReceived = new Subject<IRoom>();

  private _hubConnection: singalR.HubConnection;
  private _user: IUser;
  private _roomCode: string = "";
  private _rooms: IRoom[] = [];

  constructor(private oauthService: OAuthService, private route: ActivatedRoute) {
    this.route.queryParams.subscribe(params => {
      this._roomCode = params['roomCode'];
    });
  }

  connectToHub(hubUrl: string): void {
    const accessToken = this.oauthService.getAccessToken();

    // we want to convert to Base64 String this access Token
    this._hubConnection = new singalR.HubConnectionBuilder()
      .withUrl(hubUrl + '?token=' + accessToken + '&roomCode=' + this._roomCode)
      .build();

    // Subscribe my hub invoked functions here
    this._hubConnection.on('ReceiveMessage', (message: IMessage) => {
      this.receiveMessage(message);
    });

    this._hubConnection.on('ReceiveData', (data: IChatData) => {
      this._user = data.user;
      this.roomCurrent = data.room;

      this.$roomReceived.next(this.roomCurrent);
    });

    this._hubConnection.on('ReceiveRooms', (rooms: IRoom[]) => {
      this._rooms = rooms;
    });

    this._hubConnection.on('PingRequest', (ping: any) => {
      if (ping === 'Ping') {
        this._hubConnection.invoke('PingResponse');
      }
    });

    this._hubConnection.on('ServerOrderDisconnect', () => {
      alert('Server Disconnect you');
      this._hubConnection.stop();
    });

    this._hubConnection.start().catch(err => console.error(err.ToString()));
  }

  receiveMessage(message: IMessage): void {
    this.messages.push(message);
  }

  receiveUser(user: IUser): void {
    this._user = user;

    // set the default room for this user
    if (this._roomCode === undefined || this._roomCode === '') {
      this._roomCode = user.room.roomCode;
    }

    if (this._hubConnection) {
      this._hubConnection.invoke('JoinRoom', this._roomCode);
    }
  }

  sendMessage(message: string): void {
    const data: IMessage = {
      user: this._user,
      message: message
    };

    if (this._hubConnection) {
      this._hubConnection.invoke('SendMessage', data);
    }
  }

  requestSong(request: ISongQueuedCreateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSong', request);
    }
  }

  getRooms(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('GetRooms');
    }
  }
}
