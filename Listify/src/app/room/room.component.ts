import { YoutubeService } from './../youtube.service';
import { HubService } from './../hub.service';
import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ISongQueuedCreateRequest } from '../interfaces';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit {

  roomCode: string;
  searchSnippet: string;
  quantityWagered: number;

  constructor(
    private hubService: HubService,
    private route: ActivatedRoute,
    private youtubeService: YoutubeService) {
      this.route.queryParams.subscribe(params => {
        this.roomCode = params['id'];
      });
    }

  ngOnInit(): void {
    // const id = this.route.snapshot.paramMap.get('id');
  }

  onReady(player: any): void {
    this.youtubeService.setPlayer(player, true);
  }
  onChange(player: any): void {

  }

  requestSong(): void {
    let requst: ISongQueuedCreateRequest = {
      searchSnippet: this.searchSnippet
    };

    this.youtubeService.loadVideo(this.searchSnippet);
    this.youtubeService.play();
  }

  playerState(): void {
    let state = this.youtubeService.getPlayerState();
  }

  skip(): void {

  }

  addToPlaylist(): void {

  }

}
