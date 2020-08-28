import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalR';
// tslint:disable-next-line:max-line-length
import { IRoom, ISongQueuedCreateRequest, IApplicationUser, IApplicationUserRoom, IPlaylist, IPlaylistCreateRequest, ICurrency, ISongPlaylist, ISongSearchResults, ISongPlaylistCreateRequest, IApplicationUserRequest, IServerStateResponse } from './../interfaces';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  applicationUser: IApplicationUser;
  // rooms: IRoom[] = [];
  // roomCurrent: IRoom;
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
  $searchYoutubeReceived = new Subject<ISongSearchResults>();
  $applicationUserReceived = new Subject<IApplicationUser>();
  $pingEvent = new Subject<string>();
  $forceDisconnectReceived = new Subject<string>();

  // private _applicationUserRoomCurrent: IApplicationUserRoom;
  private _hubConnection: singalR.HubConnection;
  // private _applicationUser: IApplicationUser;
  // private _rooms: IRoom[] = [];


  constructor(private oauthService: OAuthService) {
  }

  connectToHub(hubUrl: string): void {
    if (!this._hubConnection) {
      const accessToken = this.oauthService.getAccessToken();

      // we want to convert to Base64 String this access Token
      this._hubConnection = new singalR.HubConnectionBuilder()
        .withUrl(hubUrl + '?token=' + accessToken)
        .build();

      // Subscribe my hub invoked functions here

      this._hubConnection.on('ReceiveApplicationUser', (applicationUser: IApplicationUser) => {
        this.applicationUser = applicationUser;
        this.$applicationUserReceived.next(this.applicationUser);
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

      this._hubConnection.on('PingRequest', (ping: string) => {
        // if (ping === 'Ping') {
        //   this._hubConnection.invoke('PingResponse');
        // }
        this.$pingEvent.next(ping);
      });

      this._hubConnection.on('ForceServerDisconnect', () => {
        this._hubConnection.stop();
        this.$forceDisconnectReceived.next('Disconnect');
      });

      this._hubConnection.start();
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
    }
  }

  // requestRoomByRoomCode(roomCode: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('RequestRoomByRoomCode', roomCode);
  //   }
  // }

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

  savePlaylist(playlist: IPlaylistCreateRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreatePlaylist', playlist);
    }
  }

  deletePlaylist(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DeletePlaylist', id);
    }
  }

  requestSongsQueued(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSongsQueued', roomId);
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

  deleteSongQueued(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DeleteSongQueued', id);
    }
  }

  requestSongQueued(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSongQueued', roomId);
    }
  }

  requestSongsPlaylist(playlistId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSongsPlaylist', playlistId);
    }
  }

  requestPing(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('PingResponse');
    }
  }

  dequeueSongNext(room: IRoom): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('DequeueSongNext', room.roomCode);
    }
  }

  sendServerState(response: IServerStateResponse): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('ReceiveServerState', response);
    }
  }


  getApplicationUser(): Observable<IApplicationUser> {
    return this.$applicationUserReceived.asObservable();
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

  getPing(): Observable<string> {
    return this.$pingEvent.asObservable();
  }

  disconnectFromHub(): void {
    if (this._hubConnection) {
      this._hubConnection.stop();
    }
  }

  getForceDisconnect(): Observable<string> {
    return this.$forceDisconnectReceived.asObservable();
  }

  isConnected(): boolean {
    if (this._hubConnection.state !== 0) {
      return true;
    }
    return false;
  }
}
