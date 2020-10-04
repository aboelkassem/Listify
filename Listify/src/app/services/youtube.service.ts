import { Subscription, timer } from 'rxjs';
import { RoomHubService } from './room-hub.service';
import { Injectable, OnDestroy } from '@angular/core';
import reframe from 'reframe.js';
import { ISongQueued } from '../interfaces';

@Injectable({
  providedIn: 'root'
})
export class YoutubeService implements OnDestroy {

  player: any;
  isPausedFromServer: boolean;
  songCurrent: ISongQueued;

  $pauseSubscription: Subscription;
  $playSubscription: Subscription;
  $serverStateResponseSubscription: Subscription;
  $roomReceivedSubscription: Subscription;
  $timerSubscription: Subscription;

  // reframed: boolean = false;

  constructor(private roomService: RoomHubService) {
    this.$timerSubscription = timer(0, 15000).subscribe(() => {
      if (
        this.getPlayerState() !== PlayerState.PAUSED &&
        this.getPlayerState() !== PlayerState.PLAYING &&
        this.roomService !== undefined &&
        this.roomService.applicationUserRoom !== undefined &&
        !this.roomService.applicationUserRoom.isOwner) {
          this.roomService.requestServerState(this.roomService.applicationUserRoom.room.roomCode);
      }
    });

    this.$roomReceivedSubscription = this.roomService.getRoomInformation().subscribe(roomInformation => {
      this.stop();
      this.loadVideo('');
      this.songCurrent = undefined;
    });

    this.$playSubscription = this.roomService.getPlayFromServerResponse().subscribe(playFromServerResponse => {
      // this.stop();
      this.isPausedFromServer = false;
      if (playFromServerResponse.songQueued) {
        this.loadVideoAndSeek(playFromServerResponse.songQueued.song.youtubeId, playFromServerResponse.currentTime);
        this.play();
        this.songCurrent = playFromServerResponse.songQueued;
      }
    });

    // this assigns the video properties when the server responses
    this.$serverStateResponseSubscription = this.roomService.getServerStateResponse().subscribe(response => {
      this.stop();
      if (response.songQueued) {
        this.loadVideoAndSeek(response.songQueued.song.youtubeId, response.currentTime);
        this.play();
        this.songCurrent = response.songQueued;
      }
    });

    this.$pauseSubscription = this.roomService.getPauseResponse().subscribe(pauseString => {
      this.isPausedFromServer = true;
      this.pause();
    });
  }

  ngOnDestroy(): void {
    this.$pauseSubscription.unsubscribe();
    this.$playSubscription.unsubscribe();
    this.$serverStateResponseSubscription.unsubscribe();
    this.$roomReceivedSubscription.unsubscribe();
    this.$timerSubscription.unsubscribe();
  }

  setPlayer(player: any, isReframed: boolean): void {
    this.player = player;
    // reframe(player.a);

    // if (!this.reframed && isReframed) {
    //   this.reframed = true;
    // }
  }

  loadVideo(videoId: string): void {
    this.player.loadVideoById(videoId);
  }

  loadVideoAndSeek(videoId: string, timeToStart: number): void {
    this.player.loadVideoById(videoId, timeToStart);
  }

  queueVideo(videoId: string): void {
    this.player.cueVideoById(videoId);
  }

  play(): void {
    this.player.playVideo();
  }

  pause(): void {
    this.player.pauseVideo();
  }

  stop(): void {
    this.player.stopVideo();
  }

  seek(seekToTime: number): void {
    this.player.seekTo(seekToTime, true);
  }

  getCurrentTime(): number{
    return this.player.getCurrentTime();
  }

  setVolume(volume: number): void {
    this.player.setVolume(volume);
  }

  mute(): void {
    this.player.mute();
  }

  unmute(): void {
    this.player.unMute();
  }

  getPlayerState(): PlayerState {
    return this.player.getPlayerState();
  }
}

export enum PlayerState {
  UNSTARTED = -1,
  ENDED = 0,
  PLAYING = 1,
  PAUSED = 2,
  BUFFERING = 3,
  QUEUED = 5
}
