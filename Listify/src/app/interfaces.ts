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

export interface IChatMessage {
  applicationUserRoom: IApplicationUserRoom;
  message: string;
}

export interface IPlaylist {
  id: string;
  playlistName: string;
  isSelected: boolean;
  applicationUser: IApplicationUser;
  songsPlaylists: ISongPlaylist[];
}

export interface ISongPlaylist {
  playCount: number;
  song: ISong;
}

export interface ISong {
  songName: string;
  youtubeId: string;
  songLengthSec: number;
}

export interface IPlaylistCreateRequest{
  id: string;
  playlistName: string;
  isSelected: boolean;
}
