import { YoutubeService } from './../../services/youtube.service';
import { RoomHubService } from './../../services/room-hub.service';
import { Subscription } from 'rxjs';
import { ISongQueued, ISongStateRequest, ISongPlayRequest } from './../../interfaces';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-player',
  templateUrl: './player.component.html',
  styleUrls: ['./player.component.css']
})
export class PlayerComponent implements OnInit, OnDestroy {

  songName: string;
  requestedBy: string;
  playValue: string;

  songQueued: ISongQueued;

  $songNextSubscription: Subscription;
  $songStateRequestSubscription: Subscription;
  $songStateResponseSubscription: Subscription;

  constructor(
    private roomService: RoomHubService,
    private youtubeService: YoutubeService) {
      this.$songNextSubscription = this.roomService.getSongNext().subscribe(songQueued => {
        this. songQueued = songQueued;

        this.songName = this.songQueued.song.songName;
        this.requestedBy = this.songQueued.applicationUser.username;
        this.playValue = this.songQueued.weightedValue.toString();

        this.youtubeService.loadVideo(songQueued.song.youtubeId);
        this.youtubeService.play();
      });

      this.$songStateRequestSubscription = this.roomService.getSongStateRequest().subscribe(connectionId => {
        const request: ISongStateRequest = {
          songQueuedId: this.songQueued.id,
          songId: this.songQueued.song.id,
          currentTime: this.youtubeService.getCurrentTime(),
          playerState: this.youtubeService.getPlayerState(),
          connectionId: connectionId
        };

        this.roomService.sendSongState(request);
      });

      this.$songStateResponseSubscription = this.roomService.getSongStateResponse().subscribe((songStateResponse) => {
        this.youtubeService.stop();
        this.youtubeService.loadVideoAndSeek(songStateResponse.song.youtubeId, songStateResponse.currentTime);
        this.youtubeService.play();
      });
    }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.$songNextSubscription.unsubscribe();
    this.$songStateRequestSubscription.unsubscribe();
    this.$songStateResponseSubscription.unsubscribe();
  }

  onReady(player: any): void {
    this.youtubeService.setPlayer(player, true);
  }

  onChange(event: any): void {
    switch (event.data) {
      case YT.PlayerState.ENDED:
        // Load the next song here
        if (this.roomService.applicationUserRoom.isOwner) {
          this.roomService.dequeueNextSong();
        }
        break;

      case YT.PlayerState.PLAYING:
        if (this.roomService.applicationUserRoom.isOwner) {
          const request: ISongPlayRequest = {
            songId: this.songQueued.song.id,
            songQueuedId: this.songQueued.id,
            currentTime: this.youtubeService.getCurrentTime(),
            playerState: this.youtubeService.getPlayerState(),
          };

          this.roomService.requestPlay(request);
        }
        else {
          this.roomService.requestSongState(this.roomService.room.id);
        }
        break;

      case YT.PlayerState.PAUSED:
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

}
