import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalR';
import { IRoom, IChatMessage, ISongQueuedCreateRequest, IChatData, IApplicationUser, IApplicationUserRoom } from '../interfaces';
import { ActivatedRoute } from '@angular/router';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ChatService {

  messages: IChatMessage[] = [];
  roomCurrent: IRoom;
  applicationUserRoomCurrent: IApplicationUserRoom;
  applicationUser: IApplicationUser;

  // event for asynchronous room for using in other places (observable)
  $roomReceived = new Subject<IRoom>();
  $userInfoReceived = new Subject<IApplicationUser>();

  private _hubConnection: singalR.HubConnection;
  private _applicationUser: IApplicationUser;
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
    this._hubConnection.on('ReceiveMessage', (message: IChatMessage) => {
      this.receiveMessage(message);
    });

    this._hubConnection.on('ReceiveData', (data: IChatData) => {
      this.applicationUserRoomCurrent = data.applicationUserRoom;
      this.roomCurrent = this.applicationUserRoomCurrent.room;
      // console.log(data);
      this.$roomReceived.next(this.roomCurrent);
    });

    this._hubConnection.on('ReceiveUserInformation', (applicationUser: IApplicationUser) => {
      this.applicationUser = applicationUser;
      this.$userInfoReceived.next(applicationUser);
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

    this._hubConnection.start();
  }

  getUserInfo(): Observable<IApplicationUser> {
    return this.$userInfoReceived.asObservable();
  }

  receiveMessage(message: IChatMessage): void {
    this.messages.push(message);
  }

  // receiveUser(applicationUser: IApplicationUser): void {
  //   this._applicationUser = applicationUser;

  //   // set the default room for this user
  //   if (this._roomCode === undefined || this._roomCode === '') {
  //     this._roomCode = applicationUser.room.roomCode;
  //   }

  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('JoinRoom', this._roomCode);
  //   }
  // }

  requestUserInformation(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestUserInformation');
    }
  }

  sendMessage(message: string): void {
    const data: IChatMessage = {
      applicationUserRoom: this.applicationUserRoomCurrent,
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
