import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalR';
import { IRoom, IChatMessage, ISongQueuedCreateRequest, IChatData, IApplicationUser, IApplicationUserRoom } from './interfaces';
import { ActivatedRoute } from '@angular/router';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  messages: IChatMessage[] = [];
  // rooms: IRoom[] = [];
  // roomCurrent: IRoom;
  // applicationUserRoomCurrent: IApplicationUserRoom;
  // applicationUser: IApplicationUser;

  // event for asynchronous room for using in other places (observable)
  $roomReceived = new Subject<IRoom>();
  $roomsReceived = new Subject<IRoom[]>();
  $userInfoReceived = new Subject<IApplicationUser>();

  private _hubConnection: singalR.HubConnection;
  // private _applicationUser: IApplicationUser;
  private _roomCode: string = "";
  // private _rooms: IRoom[] = [];
  private _applicationUserRoomCurrent: IApplicationUserRoom;


  constructor(
    private oauthService: OAuthService,
    private route: ActivatedRoute) {
  }

  connectToHub(hubUrl: string): void {
    const accessToken = this.oauthService.getAccessToken();

    // we want to convert to Base64 String this access Token
    this._hubConnection = new singalR.HubConnectionBuilder()
      .withUrl(hubUrl + '?token=' + accessToken + '&roomCode=' + this._roomCode)
      .build();

    // Subscribe my hub invoked functions here
    this._hubConnection.on('ReceiveMessage', (message: IChatMessage) => {
      this.messages.push(message);
    });

    this._hubConnection.on('ReceiveData', (data: IChatData) => {
      this._applicationUserRoomCurrent = data.applicationUserRoom;
      // console.log(data);
      this.$roomReceived.next(this._applicationUserRoomCurrent.room);
      this.requestRooms();
    });

    this._hubConnection.on('ReceiveUserInformation', (applicationUser: IApplicationUser) => {
      this.$userInfoReceived.next(applicationUser);
    });

    this._hubConnection.on('ReceiveRoom', (room: IRoom) => {
      this.$roomReceived.next(room);
    });

    this._hubConnection.on('ReceiveRooms', (rooms: IRoom[]) => {
      this.$roomsReceived.next(rooms);
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

  sendMessage(message: string): void {
    const data: IChatMessage = {
      applicationUserRoom: this._applicationUserRoomCurrent,
      message: message
    };

    if (this._hubConnection) {
      this._hubConnection.invoke('SendMessage', data);
    }
  }

  // // this is invoked from Server
  // receiveMessage(message: IChatMessage): void {
  // }

  getUserInfo(): Observable<IApplicationUser> {
    return this.$userInfoReceived.asObservable();
  }

  getRoom(): Observable<IRoom> {
    return this.$roomReceived.asObservable();
  }

  getRooms(): Observable<IRoom[]> {
    return this.$roomsReceived.asObservable();
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

  requestSong(request: ISongQueuedCreateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSong', request);
    }
  }

  requestRooms(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRooms');
    }
  }

  // getRooms(): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('GetRooms');
  //   }
  // }
}
