import { YoutubeService, PlayerState } from './../../services/youtube.service';
import { ISongQueued, ISongStateRequest } from './../../interfaces';
import { RoomHubService } from './../../services/room-hub.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-youtube',
  templateUrl: './youtube.component.html',
  styleUrls: ['./youtube.component.css']
})
export class YoutubeComponent implements OnInit, OnDestroy {

  songName: string;
  requestedBy: string;
  playValue: string;

  songQueued: ISongQueued;

  $songNextSubscription: Subscription;
  $songStateSubscription: Subscription;

  constructor(
    private roomService: RoomHubService,
    private youtubeService: YoutubeService
  ) {
    this.$songNextSubscription = this.roomService.getSongNext().subscribe(songQueued => {
      this.songQueued = songQueued;

      this.songName = this.songQueued.song.songName;
      this.requestedBy = this.songQueued.applicationUser.username;
      this.playValue = this.songQueued.weightedValue.toString();

      this.youtubeService.loadVideo(songQueued.song.youtubeId);
      this.youtubeService.play();
    });

    this.$songStateSubscription = this.roomService.getSongStateRequest().subscribe(connectionId => {
      const request: ISongStateRequest = {
        songQueuedId: this.songQueued.id,
        currentTime: 30,
        playerState: this.youtubeService.getPlayerState(),
        connectionId: connectionId
      };

      this.roomService.sendSongState(request);
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.$songNextSubscription.unsubscribe();
  }

  onReady(player: any): void {
    this.youtubeService.setPlayer(player, true);
  }

  onChange(player: any): void {

  }

}
