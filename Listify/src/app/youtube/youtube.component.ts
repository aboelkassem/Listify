import { ISongQueuedCreateRequest } from './../interfaces';
import { YoutubeService } from './youtube.service';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-youtube',
  templateUrl: './youtube.component.html',
  styleUrls: ['./youtube.component.css']
})
export class YoutubeComponent implements OnInit {

  searchSnippet: string;
  quantityWagered: number;

  constructor(private youtubeService: YoutubeService) { }

  ngOnInit(): void {

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
