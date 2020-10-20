import { Subject, Observable } from 'rxjs';
// tslint:disable-next-line:max-line-length
import { IApplicationUserRoom, IRoomInformation, ISongQueued, IRoom, ISongQueuedCreateRequest, IServerStateRequest, IServerStateResponse, IChatMessage, IWagerQuantitySongQueuedRequest, IApplicationUserRoomCurrencyRoom, IApplicationUser, IFollow, IApplicationUserRequest, IPlayFromServerResponse, IUpvoteSongQueuedRequest } from './../interfaces';
import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import * as singalR from '@aspnet/signalR';

@Injectable({
  providedIn: 'root'
})
export class RoomHubService {

  applicationUserRoomCurrenciesRoom: IApplicationUserRoomCurrencyRoom[] = [];
  applicationUserRoom: IApplicationUserRoom;
  room: IRoom;
  roomOwner: IApplicationUser;

  private _roomCode: string;
  private _hubConnection: singalR.HubConnection;

  $followReceived = new Subject<IFollow>();
  $followsReceived = new Subject<IFollow[]>();
  $roomInformationReceived = new Subject<IRoomInformation>();
  $songNextReceived = new Subject<ISongQueued>();
  $songsQueuedReceived = new Subject<ISongQueued[]>();
  $songQueuedReceived = new Subject<ISongQueued>();
  $pingReceived = new Subject<string>();
  $serverStateRequestReceived = new Subject<IServerStateRequest>();
  $serverStateResponseReceived = new Subject<IServerStateResponse>();
  $playFromServerResponseReceived = new Subject<IPlayFromServerResponse>();
  $applicationUserRoomCurrencyRoomReceived = new Subject<IApplicationUserRoomCurrencyRoom>();
  $applicationUserRoomCurrenciesRoomReceived = new Subject<IApplicationUserRoomCurrencyRoom[]>();
  $messageReceived = new Subject<IChatMessage>();
  $messagesReceived = new Subject<IChatMessage[]>();
  $pauseRequestReceived = new Subject<string>();
  $forceDisconnectReceived = new Subject<string>();
  $applicationUserReceived = new Subject<IApplicationUser>();
  $applicationUserReceivedAfterUpdate = new Subject<IApplicationUser>();
  $applicationUsersRoomOnline = new Subject<IApplicationUserRoom[]>();
  $applicationUserRoomOfflineReceived = new Subject<IApplicationUserRoom>();
  $applicationUserRoomOnlineReceived = new Subject<IApplicationUserRoom>();
  $requestRoomChange = new Subject<IRoom>();
  $addSongCurrentToPlaylistReceived = new Subject<boolean>();
  $RoomOwnerLogoutReceived = new Subject<boolean>();
  $UpdatedChatColorReceived = new Subject<IApplicationUser>();

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
      this.applicationUserRoomCurrenciesRoom = roomInformation.applicationUserRoomCurrenciesRoom;
      this.applicationUserRoom = roomInformation.applicationUserRoom;
      this.room = roomInformation.room;
      this.roomOwner = roomInformation.roomOwner;
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
      this.$messageReceived.next(message);
    });

    this._hubConnection.on('ReceiveLastMessages', (messages: IChatMessage[]) => {
      this.$messagesReceived.next(messages);
    });

    this._hubConnection.on('ReceiveApplicationUserRoomCurrencyRoom', (applicationUserRoomCurrency: IApplicationUserRoomCurrencyRoom) => {
      const originalCurrency = this.applicationUserRoomCurrenciesRoom.filter(x => x.id === applicationUserRoomCurrency.id)[0];
      if (originalCurrency) {
        // tslint:disable-next-line:max-line-length
        this.applicationUserRoomCurrenciesRoom[this.applicationUserRoomCurrenciesRoom.indexOf(originalCurrency)] = applicationUserRoomCurrency;
      }

      this.$applicationUserRoomCurrencyRoomReceived.next(applicationUserRoomCurrency);
    });

    // tslint:disable-next-line:max-line-length
    this._hubConnection.on('ReceiveApplicationUserRoomCurrenciesRoom', (applicationUserRoomCurrencies: IApplicationUserRoomCurrencyRoom[]) => {
      this.$applicationUserRoomCurrenciesRoomReceived.next(applicationUserRoomCurrencies);
    });

    this._hubConnection.on('ReceiveApplicationUser', (response: IApplicationUser) => {
      // ToDO: Need to complete in frontend
      this.$applicationUserReceived.next(response);
    });

    this._hubConnection.on('ReceiveApplicationUserUpdated', (response: IApplicationUser) => {
      // ToDO: Need to complete in frontend
      this.$applicationUserReceivedAfterUpdate.next(response);
    });

    this._hubConnection.on('ReceiveApplicationUserRoomOnline', (response: IApplicationUserRoom) => {
      // ToDO: Need to complete in frontend
      this.$applicationUserRoomOnlineReceived.next(response);
    });

    this._hubConnection.on('ReceiveApplicationUserRoomOffline', (response: IApplicationUserRoom) => {
      // ToDO: Need to complete in frontend
      this.$applicationUserRoomOfflineReceived.next(response);
    });

    this._hubConnection.on('ReceiveApplicationUsersRoomOnline', (applicationUserRooms: IApplicationUserRoom[]) => {
      // ToDO: Need to complete in frontend
      this.$applicationUsersRoomOnline.next(applicationUserRooms);
    });

    this._hubConnection.on('ReceiveAddSongCurrentToPlaylist', (wasSuccessful: boolean) => {
      // ToDO: Need to complete in frontend
      this.$addSongCurrentToPlaylistReceived.next(wasSuccessful);
    });

    this._hubConnection.on('ReceiveFollow', (follow: IFollow) => {
      this.$followReceived.next(follow);
    });

    this._hubConnection.on('ReceiveFollows', (follows: IFollow[]) => {
      this.$followsReceived.next(follows);
    });

    this._hubConnection.on('ReceiveRoomOwnerLogout', (isLogged: boolean) => {
      this.$RoomOwnerLogoutReceived.next(isLogged);
    });

    this._hubConnection.on('ReceiveUpdatedUserChatColor', (applicationUser: IApplicationUser) => {
      this.$UpdatedChatColorReceived.next(applicationUser);
    });

    this._hubConnection.on('ReceivePause', () => {
      this.$pauseRequestReceived.next('ReceivePause');
    });

    this._hubConnection.on('PingRequest', (ping: string) => {
      this.$pingReceived.next(ping);
    });

    this._hubConnection.on('ForceServerDisconnect', () => {
      this._hubConnection.stop();
      this.$forceDisconnectReceived.next('Disconnect');
    });

    this._hubConnection.start();
  }

  requestApplicationUser(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestApplicationUser');
    }
  }

  updateApplicationUser(applicationUserRequest: IApplicationUserRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('UpdateApplicationUser', applicationUserRequest);
    }
  }

  changeRoom(roomCode: string): void {
    this._roomCode = roomCode;

    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRoom', roomCode);
    }
  }

  requestQueueUpdated(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestQueueUpdated', roomId);
    }
  }

  requestApplicationUsersRoomOnline(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestApplicationUsersRoomOnline');
    }
  }

  requestLastMessagesForRoom(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestLastMessagesForRoom', roomId);
    }
  }

  requestUpdateUsersRoomInformationInRoom(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('UpdateUsersRoomInformationInRoom');
    }
  }

  requestAddSongCurrentToPlaylist(songQueuedId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestAddSongCurrentToPlaylist', songQueuedId);
    }
  }

  requestQueueClear(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestQueueClear');
    }
  }

  requestRoomChange(room: IRoom): void {
    this.$requestRoomChange.next(room);
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

  requestUpdatedChatColor(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestUpdatedChatColor');
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

  skipSong(songQueued: ISongQueued): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('SkipSong', songQueued);
    }
  }

  removeSongFromQueue(songQueued: ISongQueued): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RemoveFromQueue', songQueued);
    }
  }

  sendMessage(message: IChatMessage): void {
    const data: IChatMessage = {
      applicationUserRoom: this.applicationUserRoom,
      message: message.message
    };

    if (this._hubConnection) {
      this._hubConnection.invoke('SendMessage', data);
    }
  }

  wagerQuantitySongQueued(request: IWagerQuantitySongQueuedRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('WagerQuantitySongQueued', request);
    }
  }

  upvoteSongQueued(request: IUpvoteSongQueuedRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('UpvoteSongQueued', request);
    }
  }

  // requestUpvoteSongQueuedNoWager(songQueuedId: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('UpvoteSongQueuedNoWager', songQueuedId);
  //   }
  // }

  // requestDownvoteSongQueuedNoWager(songQueuedId: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('DownvoteSongQueuedNoWager', songQueuedId);
  //   }
  // }

  requestApplicationUserRoomCurrencies(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestApplicationUserRoomCurrencies');
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

  requestFollow(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestFollow', roomId);
    }
  }

  requestUnfollow(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestUnFollow', roomId);
    }
  }

  getRoomInformation(): Observable<IRoomInformation> {
    return this.$roomInformationReceived.asObservable();
  }

  getRoomChangeRequest(): Observable<IRoom> {
    return this.$requestRoomChange.asObservable();
  }

  getApplicationUser(): Observable<IApplicationUser> {
    return this.$applicationUserReceived.asObservable();
  }

  getApplicationUserUpdated(): Observable<IApplicationUser> {
    return this.$applicationUserReceivedAfterUpdate.asObservable();
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

  getApplicationUserRoomCurrencyRoom(): Observable<IApplicationUserRoomCurrencyRoom> {
    return this.$applicationUserRoomCurrencyRoomReceived.asObservable();
  }

  getMessageReceived(): Observable<IChatMessage> {
    return this.$messageReceived.asObservable();
  }

  getApplicationUserRoomCurrenciesRoom(): Observable<IApplicationUserRoomCurrencyRoom[]> {
    return this.$applicationUserRoomCurrenciesRoomReceived.asObservable();
  }

  getApplicationUsersRoomOnline(): Observable<IApplicationUserRoom[]> {
    return this.$applicationUsersRoomOnline.asObservable();
  }

  getApplicationUserRoomOnlineReceived(): Observable<IApplicationUserRoom> {
    return this.$applicationUserRoomOnlineReceived.asObservable();
  }

  getApplicationUserRoomOfflineReceived(): Observable<IApplicationUserRoom> {
    return this.$applicationUserRoomOfflineReceived.asObservable();
  }

  getAddSongCurrentToPlaylist(): Observable<boolean> {
    return this.$addSongCurrentToPlaylistReceived.asObservable();
  }

  getFollow(): Observable<IFollow> {
    return this.$followReceived.asObservable();
  }

  getFollows(): Observable<IFollow[]> {
    return this.$followsReceived.asObservable();
  }

  getLastMessages(): Observable<IChatMessage[]> {
    return this.$messagesReceived.asObservable();
  }

  getRoomOwnerLogout(): Observable<boolean> {
    return this.$RoomOwnerLogoutReceived.asObservable();
  }

  getUpdatedChatColor(): Observable<IApplicationUser> {
    return this.$UpdatedChatColorReceived.asObservable();
  }

  getForceDisconnect(): Observable<string> {
    return this.$forceDisconnectReceived.asObservable();
  }

  disconnectFromHub(): void {
    if (this._hubConnection) {
      this._hubConnection.stop();
    }
  }

  isConnected(): boolean {
    if (this._hubConnection.state !== 0) {
      return true;
    }
    return false;
  }
}
