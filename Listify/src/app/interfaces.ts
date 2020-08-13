export interface ISongQueuedCreateRequest {
  searchSnippet: string;
}

export interface ISongSearchResult {
  name: string;
}

export interface IUser {
  aspNetUserId: string;
  userName: string;
  displayName: string;
  room: IRoom;
}

export interface IRoom {
  roomCode: string;
  isPublic: boolean;
}

export interface IMessage {
  user: IUser;
  message: string;
}

export interface IChatData {
  user: IUser;
  room: IRoom;
}
