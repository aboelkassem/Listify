import { Subject, Observable } from 'rxjs';
// tslint:disable-next-line:max-line-length
import { IApplicationUserRoom, IRoomInformation, ISongQueued, IRoom, ISongQueuedCreateRequest, IServerStateRequest, IServerStateResponse, IChatMessage, IPlayFromServerResponse } from './../interfaces';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import * as singalR from '@aspnet/signalR';
import { IApplicationUserRoomCurrency } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class RoomHubService {

  messages: IChatMessage[];
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
  $serverStateRequestReceived = new Subject<IServerStateRequest>();
  $serverStateResponseReceived = new Subject<IServerStateResponse>();
  $playFromServerResponseReceived = new Subject<IPlayFromServerResponse>();
  $pauseRequestReceived = new Subject<string>();

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

    this._hubConnection.on('RequestServerState', (request: IServerStateRequest) => {
      this.$serverStateRequestReceived.next(request);
    });

    this._hubConnection.on('RequestPlayFromServer', (response: IPlayFromServerResponse) => {
      this.$playFromServerResponseReceived.next(response);
    });

    this._hubConnection.on('ReceiveMessage', (message: IChatMessage) => {
      this.messages.push(message);
    });

    this._hubConnection.on('ReceivePause', () => {
      this.$pauseRequestReceived.next('ReceivePause');
    });

    this._hubConnection.on('PingRequest', (ping: string) => {
      this.$pingReceived.next(ping);
    });

    this._hubConnection.start();
  }

  requestServerState(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestServerState', roomId);
    }
  }

  sendServerState(response: IServerStateResponse): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('ReceiveServerState', response);
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

  requestPlayFromServer(response: IPlayFromServerResponse): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPlayFromServer', response);
    }
  }

  requestPause(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPause');
    }
  }

  // requestNextSong(songPlayRequest: ISongPlayRequest): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('RequestPlay', songPlayRequest);
  //   }
  // }

  dequeueNextSong(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DequeueNextSong', this.room.roomCode);
    }
  }

  sendMessage(message: IChatMessage): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('SendMessage', message);
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

  getServerStateRequest(): Observable<IServerStateRequest> {
    return this.$serverStateRequestReceived.asObservable();
  }

  getServerStateResponse(): Observable<IServerStateResponse> {
    return this.$serverStateResponseReceived.asObservable();
  }

  getPauseResponse(): Observable<string> {
    return this.$pauseRequestReceived.asObservable();
  }

  getPlayFromServerResponse(): Observable<IPlayFromServerResponse> {
    return this.$playFromServerResponseReceived.asObservable();
  }

  isConnected(): boolean {
    if (this._hubConnection.state !== 0) {
      return true;
    }
    return false;
  }
}
