import { Subject, Observable } from 'rxjs';
import { IApplicationUserRoom, IRoomInformation, ISongQueued, IRoom, ISongQueuedCreateRequest, ISongStateRequest } from './../interfaces';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import * as singalR from '@aspnet/signalR';
import { IApplicationUserRoomCurrency } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class RoomHubService {

  applicationUserRoomCurrency: IApplicationUserRoomCurrency;
  applicationUserRoom: IApplicationUserRoom;
  room: IRoom;

  private _roomCode: string;
  private _hubConnection: singalR.HubConnection;

  $roomInformationReceived = new Subject<IRoomInformation>();
  $songNextReceived = new Subject<ISongQueued>();
  $songsQueuedReceived = new Subject<ISongQueued[]>();
  $songQueuedReceived = new Subject<ISongQueued>();
  $pingReceived = new Subject<string>();
  $songStateRequestReceived = new Subject<string>();

  constructor(private oauthService: OAuthService) { }

  connectToHub(hubUrl: string, roomCode: string): void {

    if (this._hubConnection) {
      this._hubConnection.stop();
      this._hubConnection = null;
    }

    this._roomCode = roomCode;
    const accessToken = this.oauthService.getAccessToken();

    // we want to convert to Base64 String this access Token
    this._hubConnection = new singalR.HubConnectionBuilder()
      .withUrl(hubUrl + '?token=' + accessToken + '&roomCode=' + this._roomCode)
      .build();

    // Subscribe my hub invoked functions here

    // this function is fired when the hub first connect
    this._hubConnection.on('ReceiveRoomInformation', (roomInformation: IRoomInformation) => {
      this.applicationUserRoomCurrency = roomInformation.applicationUserRoomCurrencies[0];
      this.applicationUserRoom = roomInformation.applicationUserRoom;
      this.room = roomInformation.room;
      this.$roomInformationReceived.next(roomInformation);
    });

    this._hubConnection.on('ReceiveSongNext', (songRequest: ISongQueued) => {
      // this fires this user is the owner and is validated by the backend
      if (songRequest !== null && songRequest !== undefined) {
        this.$songNextReceived.next(songRequest);
      }
    });

    this._hubConnection.on('ReceiveSongsQueued', (songsQueued: ISongQueued[]) => {
      this.$songsQueuedReceived.next(songsQueued);
    });

    this._hubConnection.on('ReceiveSongQueued', (songQueued: ISongQueued) => {
      this.$songQueuedReceived.next(songQueued);

      this.requestSongsQueued(this.applicationUserRoom.room.id);
    });

    this._hubConnection.on('ReceiveSongStateRequest', (connectionId: string) => {
      this.$songStateRequestReceived.next(connectionId);
    });

    this._hubConnection.on('ReceiveSongState', (songStateRequest: ISongStateRequest) => {
      // this.$songStateRequestReceived.next(connectionId);
    });

    this._hubConnection.on('PingRequest', (ping: string) => {
      this.$pingReceived.next(ping);
    });

    this._hubConnection.start();
  }

  sendSongState(songStateRequest: ISongStateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('ReceiveSongState', songStateRequest);
    }
  }

  requestRoom(roomCode: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRoom', roomCode);
    }
  }

  requestSongsQueued(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSongsQueued', roomId);
    }
  }

  requestPing(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('PingResponse');
    }
  }

  createSongQueued(request: ISongQueuedCreateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreateSongQueued', request);
    }
  }

  requestSongNext(room: IRoom): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSongNext', room.id);
    }
  }

  getRoomInformation(): Observable<IRoomInformation> {
    return this.$roomInformationReceived.asObservable();
  }

  getSongNext(): Observable<ISongQueued> {
    return this.$songNextReceived.asObservable();
  }

  getSongsQueued(): Observable<ISongQueued[]> {
    return this.$songsQueuedReceived.asObservable();
  }

  getSongQueued(): Observable<ISongQueued> {
    return this.$songQueuedReceived.asObservable();
  }

  getPing(): Observable<string> {
    return this.$pingReceived.asObservable();
  }

  getSongStateRequest(): Observable<string> {
    return this.$songStateRequestReceived.asObservable();
  }

  isConnected(): boolean {
    if (this._hubConnection.state !== 0) {
      return true;
    }
    return false;
  }
}
