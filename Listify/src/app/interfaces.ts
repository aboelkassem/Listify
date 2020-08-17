export interface IChatMessage {
  applicationUserRoom: IApplicationUserRoom;
  message: string;
}

export interface ISongQueuedCreateRequest {
  searchSnippet: string;
}

export interface ISongSearchResult {
  name: string;
}

export interface IApplicationUser {
  id: string;
  aspNetUserId: string;
  username: string;
  room: IRoom;
  songPoolCountSongsMax: number;
  playlistCountMax: number;
}

export interface IRoom {
  id: string;
  roomCode: string;
  isRoomPublic: boolean;
}
export interface IApplicationUserRoom {
  id: string;
  applicationUser: IApplicationUser;
  room: IRoom;
  isOnline: boolean;
}

export interface IChatData {
  applicationUserRoom: IApplicationUserRoom;
}
