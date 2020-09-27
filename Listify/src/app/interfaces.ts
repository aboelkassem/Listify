export interface IApplicationUser {
  id: string;
  aspNetUserId: string;
  username: string;
  room: IRoom;
  playlistSongCount: number;
  playlistCountMax: number;
  chatColor: string;
  queueCount: number;
  timestamp: string;
  dateJoined: string;
}

export interface IApplicationUserRequest {
  id: string;
  username: string;
  roomCode: string;
  roomTitle: string;
  roomKey: string;
  allowRequests: boolean;
  isRoomPublic: boolean;
  isRoomLocked: boolean;
  matureContent: boolean;
  matureContentChat: boolean;
  chatColor: string;
}

export interface IRoom {
  id: string;
  roomCode: string;
  roomTitle: string;
  roomKey: string;
  allowRequests: boolean;
  isRoomLocked: boolean;
  isRoomPublic: boolean;
  matureContent: boolean;
  matureContentChat: boolean;
  songsQueued: ISongQueued[];
  numberUsersOnline: number;
}

export interface IApplicationUserRoom {
  id: string;
  isOnline: boolean;
  applicationUser: IApplicationUser;
  room: IRoom;
  isOwner: boolean;
}

export interface IApplicationUserRoomCurrencyRoom {
  id: string;
  quantity: number;
  applicationUserRoom: IApplicationUserRoom;
  currencyRoom: ICurrencyRoom;
}

export interface ICurrencyRoom {
  id: string;
  currencyName: string;
  currency: ICurrency;
  room: IRoom;
}

export interface IChatMessage {
  applicationUserRoom: IApplicationUserRoom;
  message: string;
}

export interface IGenre {
  id: string;
  name: string;
}

export interface IPlaylist {
  id: string;
  playlistName: string;
  isSelected: boolean;
  isPublic: boolean;
  applicationUser: IApplicationUser;
  songsPlaylist: ISongPlaylist[];
  playlistGenres: IPlaylistGenre[];
}

export interface IPlaylistCommunity extends IPlaylist {
  numberOfSongs: number;
}

export interface IPlaylistGenre {
  genre: IGenre;
}

export interface ISongRequest {
  id: string;
  song: ISong;
}

export interface ISongPlaylist extends ISongRequest{
  playCount: number;
  playlist: IPlaylist;
}

export interface ISongQueued extends ISongRequest {
  weightedValue: number;
  applicationUser: IApplicationUser;
  room: IRoom;
  applicationUserRoomCurrencyId: string;
  quantityWagered: number;
}

export interface ISongQueuedCreateRequest {
  applicationUserRoomId: string;
  applicationUserRoomCurrencyId: string;
  quantityWagered: number;
  songSearchResult: ISongSearchResult;
}

export interface ISong {
  id: string;
  songName: string;
  youtubeId: string;
  songLengthSec: number;
}

export interface IPlaylistCreateRequest{
  id: string;
  playlistName: string;
  isSelected: boolean;
  isPublic: boolean;
  playlistGenres: IPlaylistGenre[];
}

export interface ICurrency {
  id: string;
  currencyName: string;
  weight: number;
  quantityIncreasePerTick: number;
  timeSecBetweenTick: number;
}

export interface ISongPlaylistCreateRequest {
  songSearchResult: ISongSearchResult;
  playlistId: string;
}

export interface ISongSearchResults {
  results: ISongSearchResult[];
}

export interface ISongSearchResult {
  songName: string;
  lengthSec: number;
  videoId: string;
  quantityWagered: number;
  applicationUserRoomCurrencyId: string;
}

export interface IRoomInformation {
  room: IRoom;
  applicationUserRoom: IApplicationUserRoom;
  applicationUserRoomCurrenciesRoom: IApplicationUserRoomCurrencyRoom[];
  roomOwner: IApplicationUser;
}

export interface IPlayFromServerResponse {
  currentTime: number;
  playerState: number;
  weight: number;
  songQueued: ISongQueued;
}

export interface IServerStateRequest {
  connectionId: string;
}

export interface IServerStateResponse extends IPlayFromServerResponse {
  connectionId: string;
}

export interface IWagerQuantitySongQueuedRequest {
  songQueued: ISongQueued;
  applicationUserRoom: IApplicationUserRoom;
  applicationUserRoomCurrencyRoom: IApplicationUserRoomCurrencyRoom;
  quantity: number;
}

export interface IPurchase {
  id: string;
  purchaseMethod: number;
  subtotal: number;
  amountCharged: number;
  purchaseLineItems: IPurchasableLineItem[];
  hasBeenCharged: boolean;
  wasChargeAccepted: boolean;
  timestampCharged: string;
}

export interface IPurchaseOrderRequest {
  purchaseMethod: number;
  subtotal: number;
  amountCharged: number;
  purchasableItemsJSON: string[];
}

export interface IPurchaseConfirmed {
  purchase: IPurchase;
  wasChargeAccepted: boolean;
}

export interface IPurchasableItem {
  id: string;
  purchasableItemName: string;
  purchasableItemType: number;
  quantity: number;
  unitCost: number;
  imageUri: string;
  discountApplied: number;
}

export interface IPurchasableLineItem{
  purchasableItem: IPurchasableItem;
  orderQuantity: number;
}

export interface IPurchasableCurrencyLineItem extends IPurchasableLineItem{
  applicationUserRoomCurrencyId: string;
}

export interface IConfirmationModalData {
  title: string;
  message: string;
  isConfirmed: boolean;
}

export interface IInputModalData {
  title: string;
  message: string;
  placeholder: string;
  data: string;
}

export interface IInformationModalData {
  title: string;
  message: string;
}

export interface IAuthToLockedRoomResponse {
  authToLockedRoomResponseType: number;
  room: IRoom;
}

export interface IValidatedTextRequest {
  content: string;
  validatedTextType: number;
}

export interface IValidatedTextResponse {
  isAvailable: boolean;
  validatedTextType: number;
}

export interface IProfile {
  username: string;
  roomName: string;
  roomUrl: string;
  profileTitle: string;
  profileDescription: string;
  avatarUrl: string;
  dateJoined: Date;
  playlistsCommunity: IPlaylistCommunity[];
}
