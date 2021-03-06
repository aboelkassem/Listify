import { IGenre, ISongQueued } from 'src/app/interfaces';
import { OAuthService } from 'angular-oauth2-oidc';
import { Injectable } from '@angular/core';
import * as singalR from '@aspnet/signalR';
// tslint:disable-next-line:max-line-length
import { IRoom, ISongQueuedCreateRequest, IApplicationUser, IPlaylist, IPlaylistCreateRequest, ICurrency, ISongPlaylist, ISongSearchResults, ISongPlaylistCreateRequest, IApplicationUserRequest, IServerStateResponse, IPurchasableItem, ICurrencyRoom, IPurchase, IAuthToLockedRoomResponse, IValidatedTextRequest, IValidatedTextResponse, IPurchaseOrderRequest, IProfile } from './../interfaces';
import { Subject, Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HubService {

  applicationUser: IApplicationUser;

  // events for asynchronous room for using in other places (observable)
  $receiveClearUserProfileImage = new Subject<boolean>();
  $receiveClearRoomImage = new Subject<boolean>();
  $receiveClearPlaylistImage = new Subject<boolean>();
  $roomReceived = new Subject<IRoom>();
  $roomsReceived = new Subject<IRoom[]>();
  $roomsFollowedReceived = new Subject<IRoom[]>();
  $playlistReceived = new Subject<IPlaylist>();
  $playlistsReceived = new Subject<IPlaylist[]>();
  $playlistsCommunityReceived = new Subject<IPlaylist[]>();
  $currencyRoomReceived = new Subject<ICurrencyRoom>();
  $currenciesRoomReceived = new Subject<ICurrencyRoom[]>();
  $songPlaylistReceived = new Subject<ISongPlaylist>();
  $songsPlaylistReceived = new Subject<ISongPlaylist[]>();
  $searchYoutubeReceived = new Subject<ISongSearchResults>();
  $searchYoutubePlaylistReceived = new Subject<ISongSearchResults>();
  $applicationUserReceived = new Subject<IApplicationUser>();
  $purchasableItemsReceived = new Subject<IPurchasableItem[]>();
  $purchasableItemReceived = new Subject<IPurchasableItem>();
  $receivePurchase = new Subject<IPurchase>();
  $receivePurchaseConfirmed = new Subject<IPurchase>();
  $authToLockedRoomResponse = new Subject<IAuthToLockedRoomResponse>();
  $validatedTextReceived = new Subject<IValidatedTextResponse>();
  $pingEvent = new Subject<string>();
  $forceDisconnectReceived = new Subject<string>();
  $clearAllEvent = new Subject<string>();
  $addYoutubePlaylistToPlaylistReceived = new Subject<ISongPlaylist[]>();
  $addSpotifyPlaylistToPlaylistReceived = new Subject<ISongPlaylist[]>();
  $genresReceived = new Subject<IGenre[]>();
  $genresRoomReceived = new Subject<IGenre[]>();
  $profileReceived = new Subject<IProfile>();
  $purchasesReceived = new Subject<IPurchase[]>();
  $receiveQueuePlaylistInRoomHome = new Subject<ISongQueued[]>();

  private _hubConnection: singalR.HubConnection;

  constructor(private oauthService: OAuthService) { }

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

      this._hubConnection.on('ReceivePlaylistsCommunity', (playlists: IPlaylist[]) => {
        this.$playlistsCommunityReceived.next(playlists);
      });

      this._hubConnection.on('ReceivePlaylist', (playlist: IPlaylist) => {
        this.$playlistReceived.next(playlist);
      });

      this._hubConnection.on('ReceiveCurrencyRoom', (currency: ICurrencyRoom) => {
        this.$currencyRoomReceived.next(currency);
      });

      this._hubConnection.on('ReceiveCurrenciesRoom', (currencies: ICurrencyRoom[]) => {
        this.$currenciesRoomReceived.next(currencies);
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

      this._hubConnection.on('ReceiveSearchYoutubePlaylist', (responses: ISongSearchResults) => {
        this.$searchYoutubePlaylistReceived.next(responses);
      });

      this._hubConnection.on('ReceivePurchasableItems', (purchasableItems: IPurchasableItem[]) => {
        this.$purchasableItemsReceived.next(purchasableItems);
      });

      this._hubConnection.on('ReceivePurchasableItem', (purchasableItem: IPurchasableItem) => {
        this.$purchasableItemReceived.next(purchasableItem);

        this.requestPurchasableItems();
      });

      this._hubConnection.on('ReceivePurchase', (purchase: IPurchase) => {
        this.$receivePurchase.next(purchase);
        // this.requestPurchasableItems();
      });

      this._hubConnection.on('ReceivePurchaseConfirmed', (purchase: IPurchase) => {
        this.$receivePurchaseConfirmed.next(purchase);
      });

      this._hubConnection.on('ResponseAuthToLockedRoom', (authToLockedRoomResponse: IAuthToLockedRoomResponse) => {
        this.$authToLockedRoomResponse.next(authToLockedRoomResponse);
      });

      this._hubConnection.on('ReceiveValidatedText', (response: IValidatedTextResponse) => {
        this.$validatedTextReceived.next(response);
      });

      this._hubConnection.on('ReceiveQueuePlaylistInRoomHome', (songsQueued: ISongQueued[]) => {
        this.$receiveQueuePlaylistInRoomHome.next(songsQueued);
      });

      this._hubConnection.on('ReceivePurchases', (Purchases: IPurchase[]) => {
        this.$purchasesReceived.next(Purchases);
      });

      this._hubConnection.on('ReceiveAddYoutubePlaylistToPlaylist', (songsPlaylist: ISongPlaylist[]) => {
        this.$addYoutubePlaylistToPlaylistReceived.next(songsPlaylist);
      });

      this._hubConnection.on('ReceiveAddSpotifyPlaylistToPlaylist', (songsPlaylist: ISongPlaylist[]) => {
        this.$addSpotifyPlaylistToPlaylistReceived.next(songsPlaylist);
      });

      this._hubConnection.on('ReceiveGenres', (genres: IGenre[]) => {
        this.$genresReceived.next(genres);
      });

      this._hubConnection.on('ReceiveGenresRoom', (genres: IGenre[]) => {
        this.$genresRoomReceived.next(genres);
      });

      this._hubConnection.on('ReceiveProfile', (profile: IProfile) => {
        this.$profileReceived.next(profile);
      });

      this._hubConnection.on('ReceiveClearUserProfileImage', (response: boolean) => {
        this.$receiveClearUserProfileImage.next(response);
      });

      this._hubConnection.on('ReceiveClearRoomImage', (response: boolean) => {
        this.$receiveClearRoomImage.next(response);
      });

      this._hubConnection.on('ReceiveClearPlaylistImage', (response: boolean) => {
        this.$receiveClearPlaylistImage.next(response);
      });

      this._hubConnection.on('ReceiveRoomsFollowed', (rooms: IRoom[]) => {
        this.$roomsFollowedReceived.next(rooms);
      });

      this._hubConnection.on('ReceiveUpdatedPlaylistsCount', (playlistCountMax: number) => {
        this.applicationUser.playlistCountMax = playlistCountMax;
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

  requestApplicationUserUpdatedPlaylistsCount(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestApplicationUserUpdatedPlaylistsCount');
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

  requestRoomsFollowedProfile(applicationUserId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRoomsFollowedProfile', applicationUserId);
    }
  }

  requestRoomsFollowed(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRoomsFollowed');
    }
  }

  requestRoom(roomCode: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestRoom', roomCode);
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

  requestPlaylistsCommunity(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPlaylistsCommunity');
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
  requestCurrenciesRoom(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestCurrenciesRoom', roomId);
    }
  }

  requestCurrencyRoom(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestCurrencyRoom', id);
    }
  }

  saveCurrency(currency: ICurrency): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreateCurrency', currency);
    }
  }

  // deleteCurrency(id: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('DeleteCurrency', id);
  //   }
  // }

  requestSearchYoutube(searchSnippet: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSearchYoutube', searchSnippet);
    }
  }

  requestSearchYoutubePlaylist(searchSnippet: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestSearchYoutubePlaylist', searchSnippet);
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

  requestPurchasableItems(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPurchasableItems');
    }
  }

  requestPurchasableItem(id: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPurchasableItem', id);
    }
  }

  requestQueuePlaylistInRoomHome(id: string, isRandomized: boolean): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('QueuePlaylistInRoomHome', id, isRandomized);
    }
  }

  requestAddYoutubePlaylistToPlaylist(youtubePlaylistUrl: string, playlistId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestAddYoutubePlaylistToPlaylist', youtubePlaylistUrl, playlistId);
    }
  }

  requestAddSpotifyPlaylistToPlaylist(spotifyPlaylistUrl: string, playlistId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestAddSpotifyPlaylistToPlaylist', spotifyPlaylistUrl, playlistId);
    }
  }

  requestPurchases(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestPurchases');
    }
  }

  requestProfile(username: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestProfile', username);
    }
  }

  requestProfileUpdate(profile: IProfile): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestProfileUpdate', profile);
    }
  }

  requestClearProfileImage(profileId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestClearProfileImage', profileId);
    }
  }

  requestClearRoomImage(roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestClearRoomImage', roomId);
    }
  }

  requestClearPlaylistImage(playlistId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestClearPlaylistImage', playlistId);
    }
  }

  requestGenres(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestGenres');
    }
  }

  requestGenresRoom(): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestGenresRoom');
    }
  }

  requestClearAll(): void {
    this.$clearAllEvent.next('ClearAll');
  }

  // savePurchasableItem(purchasableItem: IPurchasableItem): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('CreatePurchasableItem', purchasableItem);
  //   }
  // }

  // deletePurchasableItem(id: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('DeletePurchasableItem', id);
  //   }
  // }

  // requestPurchasableItemCurrency(id: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('RequestPurchasableItemCurrency', id);
  //   }
  // }

  // savePurchasableItemCurrency(purchasableItemCurrency: IPurchasableItemCurrency): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('CreatePurchasableItemCurrency', purchasableItemCurrency);
  //   }
  // }

  // deletePurchasableItemCurrency(id: string): void {
  //   if (this._hubConnection) {
  //     this._hubConnection.invoke('DeletePurchasableItemCurrency', id);
  //   }
  // }

  createPurchase(purchase: IPurchaseOrderRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('CreatePurchase', purchase);
    }
  }

  requestAuthToLockedRoom(roomKey: string, roomId: string): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestAuthToLockedRoom', roomKey, roomId);
    }
  }

  requestValidatedText(request: IValidatedTextRequest): void {
    if (this._hubConnection) {
      this._hubConnection.invoke('RequestValidatedText', request);
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

  getPlaylistsCommunity(): Observable<IPlaylist[]> {
    return this.$playlistsCommunityReceived.asObservable();
  }

  getPlaylist(): Observable<IPlaylist> {
    return this.$playlistReceived.asObservable();
  }

  getCurrenciesRoom(): Observable<ICurrencyRoom[]> {
    return this.$currenciesRoomReceived.asObservable();
  }

  getCurrencyRoom(): Observable<ICurrencyRoom> {
    return this.$currencyRoomReceived.asObservable();
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

  getSearchYoutubePlaylist(): Observable<ISongSearchResults> {
    return this.$searchYoutubePlaylistReceived.asObservable();
  }

  getPurchasableItems(): Observable<IPurchasableItem[]> {
    return this.$purchasableItemsReceived.asObservable();
  }

  getPurchasableItem(): Observable<IPurchasableItem> {
    return this.$purchasableItemReceived.asObservable();
  }

  getPurchase(): Observable<IPurchase> {
    return this.$receivePurchase.asObservable();
  }

  getPurchaseConfirmed(): Observable<IPurchase> {
    return this.$receivePurchaseConfirmed.asObservable();
  }

  getAuthToLockedRoom(): Observable<IAuthToLockedRoomResponse> {
    return this.$authToLockedRoomResponse.asObservable();
  }

  getValidatedTextReceived(): Observable<IValidatedTextResponse> {
    return this.$validatedTextReceived.asObservable();
  }

  getQueuePlaylistInRoomHome(): Observable<ISongQueued[]> {
    return this.$receiveQueuePlaylistInRoomHome.asObservable();
  }

  getPurchases(): Observable<IPurchase[]> {
    return this.$purchasesReceived.asObservable();
  }

  getGenres(): Observable<IGenre[]> {
    return this.$genresReceived.asObservable();
  }

  getGenresRoom(): Observable<IGenre[]> {
    return this.$genresRoomReceived.asObservable();
  }

  getClearAll(): Observable<string> {
    return this.$clearAllEvent.asObservable();
  }

  getAddYoutubePlaylistToPlaylist(): Observable<ISongPlaylist[]> {
    return this.$addYoutubePlaylistToPlaylistReceived.asObservable();
  }

  getAddSpotifyPlaylistToPlaylist(): Observable<ISongPlaylist[]> {
    return this.$addSpotifyPlaylistToPlaylistReceived.asObservable();
  }

  getProfile(): Observable<IProfile> {
    return this.$profileReceived.asObservable();
  }

  getClearUserProfileImage(): Observable<boolean> {
    return this.$receiveClearUserProfileImage.asObservable();
  }

  getClearRoomImage(): Observable<boolean> {
    return this.$receiveClearRoomImage.asObservable();
  }

  getClearPlaylistImage(): Observable<boolean> {
    return this.$receiveClearPlaylistImage.asObservable();
  }

  getRoomsFollowed(): Observable<IRoom[]> {
    return this.$roomsFollowedReceived.asObservable();
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
