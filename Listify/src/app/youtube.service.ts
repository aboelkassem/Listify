import { Injectable } from '@angular/core';
import reframe from 'reframe.js';

@Injectable({
  providedIn: 'root'
})
export class YoutubeService {

  constructor() { }

  player: any;
  reframed: boolean = false;

  setPlayer(player: any, isReframed: boolean): void {
    this.player = player;

    if (!this.reframed && isReframed) {
      this.reframed = true;
      reframe(player.a);
    }
  }

  loadVideo(videoId: string): void {
    this.player.loadVideoById(videoId);
  }

  queueVideo(videoId: string): void {
    this.player.cueVideoById(videoId);
  }

  play(): void {
    this.player.playVideo();
  }

  stop(): void {
    this.player.stopVideo();
  }

  seek(seekToTime: number): void {
    this.player.seekTo(seekToTime);
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
