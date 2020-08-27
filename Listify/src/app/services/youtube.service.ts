import { Subscription } from 'rxjs';
import { RoomHubService } from './room-hub.service';
import { Injectable, OnDestroy } from '@angular/core';
import reframe from 'reframe.js';

@Injectable({
  providedIn: 'root'
})
export class YoutubeService implements OnDestroy {

  player: any;

  $pauseSubscription: Subscription;
  $playSubscription: Subscription;

  // reframed: boolean = false;

  constructor(
    private roomService: RoomHubService
  ) {
    this.$playSubscription = this.roomService.getPlayFromServerResponse().subscribe(playFromServerResponse => {
      this.stop();
      this.loadVideoAndSeek(playFromServerResponse.songQueued.song.youtubeId, playFromServerResponse.currentTime);
      this.play();
    });

    this.$pauseSubscription = this.roomService.getPauseResponse().subscribe(pauseString => {
      this.pause();
    });
  }

  ngOnDestroy(): void {
    this.$pauseSubscription.unsubscribe();
    this.$playSubscription.unsubscribe();
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
    this.player.seekTo(seekToTime);
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
    this.player.unmute();
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
