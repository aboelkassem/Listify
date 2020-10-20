import { PlayerService } from './../../services/player.service';
import { ApplicationusersroommodalComponent } from './../modals/applicationusersroommodal/applicationusersroommodal.component';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { YoutubeService } from './../../services/youtube.service';
import { RoomHubService } from './../../services/room-hub.service';
import { Subscription, timer } from 'rxjs';
import { ISongQueued, IServerStateResponse, IApplicationUserRoom, IPlayFromServerResponse } from './../../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { HubService } from 'src/app/services/hub.service';
import { ApplicationusersfollowingmodalComponent } from '../modals/applicationusersfollowingmodal/applicationusersfollowingmodal.component';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.css']
})
export class PlayerComponent implements OnInit, OnDestroy {

  loading = false;

  songName: string;
  requestedBy: string;
  requestedByColor: string;
  playValue: string;
  roomTitle: string;
  roomOwnerUsername: string;
  roomOwnerColor: string;
  numberUsersOnline: number;
  numberFollows: number;
  isPlaying: boolean;
  isFollowing: boolean;
  isOwner = true;

  applicationUsersRoom: IApplicationUserRoom[] = [];
  songQueued: ISongQueued;

  $playFromServerSubscription: Subscription;
  $serverStateRequestSubscription: Subscription;
  $serverStateResponseSubscription: Subscription;
  $roomInformationSubscription: Subscription;
  $applicationUsersRoomOnlineSubscription: Subscription;
  $applicationUserRoomOnlineSubscription: Subscription;
  $applicationUserRoomOfflineSubscription: Subscription;
  $timerSubscription: Subscription;
  $addCurrentSongToPlaylistSubscription: Subscription;
  $followReceived: Subscription;
  $followsReceived: Subscription;

  constructor(
    private roomService: RoomHubService,
    private hubService: HubService,
    private youtubeService: YoutubeService,
    private playerService: PlayerService,
    private matDialog: MatDialog,
    private toastrService: ToastrService) {
      this.$timerSubscription = timer(0, 30000).subscribe(() => {
        this.roomService.requestApplicationUsersRoomOnline();
      });

      this.$followReceived = this.roomService.getFollow().subscribe(follow => {
        this.loading = false;
        if (follow) {
          this.isFollowing = true;
          this.toastrService.success('You are now following this room.', 'Following');
        }else {
          this.isFollowing = false;
          this.toastrService.success('You are now no longer following this room.', 'UnFollowed');
        }
      });

      this.$followsReceived = this.roomService.getFollows().subscribe(follows => {
        this.roomService.room.follows = follows;
        this.numberFollows = follows.length;
      });

      this.$addCurrentSongToPlaylistSubscription = this.roomService.getAddSongCurrentToPlaylist()
        .subscribe((wasSongQueuedSuccessful: boolean) => {
          if (wasSongQueuedSuccessful) {
            this.loading = false;
            this.toastrService.success('The current song was added to the default playlist.', 'Added to Playlist');
          }else {
            this.loading = false;
            this.toastrService.error('The current song is already in your default playlist, so could not be added again.', 'Failed to Added to Playlist');
          }
      });

      this.$applicationUserRoomOfflineSubscription = this.roomService.getApplicationUserRoomOfflineReceived()
        .subscribe(applicationUserRoomOffline => {
          const selectedApplicationUserRoom = this.applicationUsersRoom.filter(x => x.id === applicationUserRoomOffline.id)[0];

          if (selectedApplicationUserRoom) {
            this.applicationUsersRoom.splice(this.applicationUsersRoom.indexOf(selectedApplicationUserRoom), 1);
          }

          this.numberUsersOnline = this.applicationUsersRoom.length;
        });

      this.$applicationUserRoomOnlineSubscription = this.roomService.getApplicationUserRoomOnlineReceived()
        .subscribe(applicationUserRoomOnline => {
          const applicationUserRoom = this.applicationUsersRoom.find(x => x.id === applicationUserRoomOnline.id);

          if (!applicationUserRoom) {
            this.applicationUsersRoom.push(applicationUserRoomOnline);
          }

          this.numberUsersOnline = this.applicationUsersRoom.length;
        });

      this.$applicationUsersRoomOnlineSubscription = this.roomService.getApplicationUsersRoomOnline()
        .subscribe(applicationUsersRoomOnline => {
        this.applicationUsersRoom = applicationUsersRoomOnline;
        this.numberUsersOnline = this.applicationUsersRoom.length;
      });

      this.$roomInformationSubscription = this.roomService.getRoomInformation().subscribe(roomInformation => {
        this.roomTitle = roomInformation.room.roomTitle;
        this.roomOwnerUsername = roomInformation.roomOwner.username;
        this.roomOwnerColor = roomInformation.roomOwner.chatColor;
        this.numberFollows = roomInformation.room.follows.length;
        this.numberUsersOnline = this.applicationUsersRoom.length;
        this.isPlaying = roomInformation.room.isRoomPlaying;

        this.isFollowing = roomInformation.room.follows.filter(x => x.applicationUser.id === this.hubService.applicationUser.id).length > 0;
        this.isOwner = roomInformation.applicationUserRoom.isOwner;

        // this.songQueued = undefined;
        // this.songName = '';
        // this.requestedBy = '';
        // this.requestedByColor = '';
        // this.playValue = '';

        this.roomService.requestApplicationUsersRoomOnline();
      });

      // this sets the properties for a server ordered play
      this.$playFromServerSubscription = this.roomService.getPlayFromServerResponse().subscribe(response => {
        if (response.songQueued) {
          this.songQueued = response.songQueued;
          this.songName = this.songQueued.song.songName;
          this.requestedBy = this.songQueued.applicationUser.username;
          this.requestedByColor = this.songQueued.applicationUser.chatColor;
          this.playValue = this.songQueued.weightedValue.toString();
          this.isPlaying = this.songQueued.room.isRoomPlaying;

          // this.roomTitle = this.roomService.applicationUserRoom.room.roomTitle;
          // this.roomOwnerUsername = this.roomService.roomOwner.username;
          // this.roomOwnerColor = this.roomService.roomOwner.chatColor;
        }else {
          this.songQueued = undefined;
          this.songName = '';
          this.requestedBy = '';
          this.requestedByColor = '';
          this.playValue = '';
        }

        this.playerService.setCurrentSong(this.songQueued);
        // this.youtubeService.loadVideoAndSeek(response.songQueued.song.youtubeId, response.currentTime);
        // this.youtubeService.play();
      });

      // this is to return the state of the server to each client
      this.$serverStateRequestSubscription = this.roomService.getServerStateRequest().subscribe(request => {
        let response: IServerStateResponse;
        if (this.songQueued) {
          response = {
            songQueued: this.songQueued,
            currentTime: this.youtubeService.getCurrentTime(),
            playerState: this.youtubeService.getPlayerState(),
            connectionId: request.connectionId,
            weight: this.songQueued.weightedValue
          };
        }else {
          response = {
            songQueued: undefined,
            currentTime: 0,
            playerState: this.youtubeService.getPlayerState(),
            connectionId: request.connectionId,
            weight: 0
          };
        }

        this.roomService.sendServerState(response);
      });

      // this assigns the video properties when the server response
      this.$serverStateResponseSubscription = this.roomService.getServerStateResponse().subscribe((response) => {
        if (response.songQueued) {
          this.songQueued = response.songQueued;
          this.songName = response.songQueued.song.songName;
          this.requestedBy = response.songQueued.applicationUser.username;
          this.requestedByColor = response.songQueued.applicationUser.chatColor;
          this.playValue = response.weight.toString();
          this.isPlaying = response.songQueued.room.isRoomPlaying;

        }else {
          this.songQueued = undefined;
          this.songName = '';
          this.requestedBy = '';
          this.requestedByColor = '';
          this.playValue = '';
        }

        this.playerService.setCurrentSong(this.songQueued);
        // this.youtubeService.loadVideoAndSeek(response.songQueued.song.youtubeId, response.currentTime);
        // this.youtubeService.play();
      });
    }

  ngOnInit(): void {
    const tag = document.createElement('script');
    tag.src = 'https://www.youtube.com/iframe_api';
    document.body.appendChild(tag);
  }

  ngOnDestroy(): void {
    this.$playFromServerSubscription.unsubscribe();
    this.$serverStateRequestSubscription.unsubscribe();
    this.$serverStateResponseSubscription.unsubscribe();
    this.$roomInformationSubscription.unsubscribe();
    this.$applicationUsersRoomOnlineSubscription.unsubscribe();
    this.$applicationUserRoomOnlineSubscription.unsubscribe();
    this.$applicationUserRoomOfflineSubscription.unsubscribe();
    this.$timerSubscription.unsubscribe();
    this.$addCurrentSongToPlaylistSubscription.unsubscribe();
    this.$followReceived.unsubscribe();
    this.$followsReceived.unsubscribe();
  }

  onReady(player: any): void {
    this.youtubeService.setPlayer(player, true);

    if (this.roomService.applicationUserRoom.isOwner) {
      this.youtubeService.loadVideo(this.songQueued.song.youtubeId);
      this.youtubeService.play();
    }else {
      this.roomService.requestServerState(this.roomService.room.id);
    }

  }

  onStateChange(event: any): void {
    switch (event.data) {
      case YT.PlayerState.ENDED:
        this.isPlaying = false;
        // Load the next song here
        if (this.roomService.applicationUserRoom.isOwner) {
          this.roomService.dequeueNextSong();
        }
        break;

      case YT.PlayerState.PLAYING:
        this.isPlaying = true;
        if (this.roomService.applicationUserRoom.isOwner && this.songQueued) {
          const request: IPlayFromServerResponse = {
            songQueued: this.songQueued,
            currentTime: this.youtubeService.getCurrentTime(),
            playerState: this.youtubeService.getPlayerState(),
            weight: this.songQueued.weightedValue
          };

          this.roomService.requestPlayFromServer(request);
        }else if (this.youtubeService.isPausedFromServer){
          this.youtubeService.stop();
        }
        break;

      case YT.PlayerState.PAUSED:
        this.isPlaying = false;
        if (this.roomService.applicationUserRoom.isOwner) {
          this.roomService.requestPause();
        }else {
          this.youtubeService.pause();
        }
        break;

      case YT.PlayerState.BUFFERING:
        break;

      case YT.PlayerState.CUED:
        break;

      default:
        break;
    }
  }

  showApplicationUsersRoomModal(): void {
    this.matDialog.open(ApplicationusersroommodalComponent, {
      width: '400px',
      height: '500px',
      data: this.applicationUsersRoom
    });
  }

  addToPlaylistDefault(): void {
    this.loading = true;
    this.roomService.requestAddSongCurrentToPlaylist(this.youtubeService.songCurrent.id);
  }

  followButtonClick(): void {
    this.loading = true;
    this.roomService.requestFollow(this.roomService.applicationUserRoom.room.id);
  }

  unfollowButtonClick(): void {
    this.loading = true;
    this.roomService.requestUnfollow(this.roomService.applicationUserRoom.room.id);
  }

  viewProfileFollows(): void {
    this.matDialog.open(ApplicationusersfollowingmodalComponent, {
      width: '400px',
      height: '500px',
      data: this.roomService.room.follows
    });
  }
}
