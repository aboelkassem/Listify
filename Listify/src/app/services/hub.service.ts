import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalR';
// tslint:disable-next-line:max-line-length
import { IRoom, IChatMessage, ISongQueuedCreateRequest, IChatData, IApplicationUser, IApplicationUserRoom, IPlaylist, IPlaylistCreateRequest, ICurrency, ISongPlaylist, ISongSearchResults, ISongPlaylistCreateRequest, IApplicationUserRequest, ISongQueued } from './../interfaces';
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

  // events for asynchronous room for using in other places (observable)
  $roomReceived = new Subject<IRoom>();
  $roomsReceived = new Subject<IRoom[]>();
  $playlistReceived = new Subject<IPlaylist>();
  $playlistsReceived = new Subject<IPlaylist[]>();
  $currencyReceived = new Subject<ICurrency>();
  $currenciesReceived = new Subject<ICurrency[]>();
  $songPlaylistReceived = new Subject<ISongPlaylist>();
  $songsPlaylistReceived = new Subject<ISongPlaylist[]>();
  $queueReceived = new Subject<ISongQueued[]>();
  $searchYoutubeReceived = new Subject<ISongSearchResults>();
  $userInfoReceived = new Subject<IApplicationUser>();
  $forceDisconnectReceived = new Subject<string>();

  private _applicationUserRoomCurrent: IApplicationUserRoom;
  private _hubConnection: singalR.HubConnection;
  private _roomCode: string = "";
  // private _applicationUser: IApplicationUser;
  // private _rooms: IRoom[] = [];


  constructor(private oauthService: OAuthService) {
  }

  connectToHub(hubUrl: string): void {
    if (!this._hubConnection) {
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
        this.$userInfoReceived.next(this._applicationUserRoomCurrent.applicationUser);
        // this.requestPlaylists();
        // this.requestRooms();
        this.requestRoom(this._applicationUserRoomCurrent.room.id);
      });

      this._hubConnection.on('ReceiveApplicationUserInformation', (applicationUser: IApplicationUser) => {
        this.$userInfoReceived.next(applicationUser);
      });

      this._hubConnection.on('ReceiveRooms', (rooms: IRoom[]) => {
        this.$roomsReceived.next(rooms);
      });

      this._hubConnection.on('ReceiveRoom', (room: IRoom) => {
        this.$roomReceived.next(room);
      });

      this._hubConnection.on('ReceivePlaylists', (playlists: IPlaylist[]) => {
        this.$playlistsReceived.next(playlists);
      });

      this._hubConnection.on('ReceivePlaylist', (playlist: IPlaylist) => {
        this.$playlistReceived.next(playlist);
      });

      this._hubConnection.on('ReceiveCurrency', (currency: ICurrency) => {
        this.$currencyReceived.next(currency);
      });

      this._hubConnection.on('ReceiveCurrencies', (currencies: ICurrency[]) => {
        this.$currenciesReceived.next(currencies);
      });

      this._hubConnection.on('ReceiveSongPlaylist', (songPlaylist: ISongPlaylist) => {
        this.$songPlaylistReceived.next(songPlaylist);
      });

      this._hubConnection.on('ReceiveSongsPlaylist', (songsPlaylist: ISongPlaylist[]) => {
        this.$songsPlaylistReceived.next(songsPlaylist);
      });

      this._hubConnection.on('ReceiveSearchYoutube', (responses: ISongSearchResults) => {
        this.$searchYoutubeReceived.next(responses);
      });

      this._hubConnection.on('ReceiveQueue', (songsQueued: ISongQueued[]) => {
        this.$queueReceived.next(songsQueued);
      });

      this._hubConnection.on('PingRequest', (ping: any) => {
        if (ping === 'Ping') {
          this._hubConnection.invoke('PingResponse');
        }
      });

      this._hubConnection.on('ForceServerDisconnect', () => {
        this._hubConnection.stop();
        alert('you have been disconnected');
        this.$forceDisconnectReceived.next('Disconnect');
      });

      this._hubConnection.start();
    }
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

  requestApplicationUserInformation(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestApplicationUserInformation');
    }
  }

  updateApplicationUserInformation(applicationUser: IApplicationUserRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('UpdateApplicationUserInformation', applicationUser);
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

  requestRoom(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRoom', id);
      // this._hubConnection.invoke('RequestRooms', id);
    }
  }

  requestPlaylists(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPlaylists');
    }
  }

  requestPlaylist(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPlaylist', id);
    }
  }

  deletePlaylist(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DeletePlaylist', id);
    }
  }

  savePlaylist(playlist: IPlaylistCreateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreatePlaylist', playlist);
    }
  }
  // getRooms(): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('GetRooms');
  //   }
  // }
  requestCurrencies(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestCurrencies');
    }
  }

  requestCurrency(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestCurrency', id);
    }
  }

  saveCurrency(currency: ICurrency): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreateCurrency', currency);
    }
  }

  deleteCurrency(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DeleteCurrency', id);
    }
  }

  requestSearchYoutube(searchSnippet: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSearchYoutube', searchSnippet);
    }
  }

  saveSongPlaylist(request: ISongPlaylistCreateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreateSongPlaylist', request);
    }
  }

  deleteSongPlaylist(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DeleteSongPlaylist', id);
    }
  }

  requestQueue(room: IRoom): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestQueue', room);
    }
  }

  requestSongsPlaylist(playlistId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSongsPlaylist', playlistId);
    }
  }

  getUserInfo(): Observable<IApplicationUser> {
    return this.$userInfoReceived.asObservable();
  }

  getRooms(): Observable<IRoom[]> {
    return this.$roomsReceived.asObservable();
  }

  getRoom(): Observable<IRoom> {
    return this.$roomReceived.asObservable();
  }

  getPlaylists(): Observable<IPlaylist[]> {
    return this.$playlistsReceived.asObservable();
  }

  getPlaylist(): Observable<IPlaylist> {
    return this.$playlistReceived.asObservable();
  }

  getCurrencies(): Observable<ICurrency[]> {
    return this.$currenciesReceived.asObservable();
  }

  getCurrency(): Observable<ICurrency> {
    return this.$currencyReceived.asObservable();
  }

  getSongsPlaylist(): Observable<ISongPlaylist[]> {
    return this.$songsPlaylistReceived.asObservable();
  }

  getSongPlaylist(): Observable<ISongPlaylist> {
    return this.$songPlaylistReceived.asObservable();
  }

  getSearchYoutube(): Observable<ISongSearchResults> {
    return this.$searchYoutubeReceived.asObservable();
  }

  getQueue(): Observable<ISongQueued[]> {
    return this.$queueReceived.asObservable();
  }

  getForceDisconnect(): Observable<string> {
    return this.$forceDisconnectReceived.asObservable();
  }
}
