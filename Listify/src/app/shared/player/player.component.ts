import { YoutubeService } from './../../services/youtube.service';
import { RoomHubService } from './../../services/room-hub.service';
import { Subscription } from 'rxjs';
import { ISongQueued, IServerStateResponse, IPlayFromServerResponse } from './../../interfaces';
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

  $playFromServerSubscription: Subscription;
  $serverStateRequestSubscription: Subscription;
  $serverStateResponseSubscription: Subscription;

  constructor(
    private roomService: RoomHubService,
    private youtubeService: YoutubeService) {
      // this sets the properties for a server ordered play
      this.$playFromServerSubscription = this.roomService.getPlayFromServerResponse().subscribe(response => {
        this.songQueued = response.songQueued;
        this.songName = this.songQueued.song.songName;
        this.requestedBy = this.songQueued.applicationUser.username;
        this.playValue = this.songQueued.weightedValue.toString();

        // this.youtubeService.loadVideo(response.songQueued.song.youtubeId);
        // this.youtubeService.play();
      });

      // this is to return the state of the server to each client
      this.$serverStateRequestSubscription = this.roomService.getServerStateRequest().subscribe(request => {
        const response: IServerStateResponse = {
          songQueued: this.songQueued,
          currentTime: this.youtubeService.getCurrentTime(),
          playerState: this.youtubeService.getPlayerState(),
          connectionId: request.connectionId,
          weight: this.songQueued.weightedValue
        };

        this.roomService.sendServerState(response);
      });

      // this assigns the video properties when the server response
      this.$serverStateResponseSubscription = this.roomService.getServerStateResponse().subscribe((response) => {
        this.songName = response.songQueued.song.songName;
        this.requestedBy = response.songQueued.applicationUser.username;
        this.playValue = response.weight.toString();

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
  }

  onReady(player: any): void {
    this.youtubeService.setPlayer(player, true);

    if (this.roomService.applicationUserRoom.isOwner) {
      this.youtubeService.loadVideo(this.songQueued.song.youtubeId);
      this.youtubeService.play();
    }
  }

  onStateChange(event: any): void {
    switch (event.data) {
      case YT.PlayerState.ENDED:
        // Load the next song here
        if (this.roomService.applicationUserRoom.isOwner) {
          this.roomService.dequeueNextSong();
        }
        break;

      case YT.PlayerState.PLAYING:
        if (this.roomService.applicationUserRoom.isOwner) {
          const request: IPlayFromServerResponse = {
            songQueued: this.songQueued,
            currentTime: this.youtubeService.getCurrentTime(),
            playerState: this.youtubeService.getPlayerState(),
            weight: this.songQueued.weightedValue
          };

          this.roomService.requestPlayFromServer(request);
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
