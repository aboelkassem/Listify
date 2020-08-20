import { ISongSearchResult } from 'src/app/interfaces';
import { IRoom } from './../interfaces';
import { Subscription } from 'rxjs';
import { YoutubeService } from './../services/youtube.service';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit, OnDestroy {

  roomCode: string;
  searchSnippet: string;
  quantityWagered: number;

  room: IRoom;
  songSearchResults: ISongSearchResult[] = [];

  $roomSubscription: Subscription;
  $youtubeSearchSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private route: ActivatedRoute,
    private youtubeService: YoutubeService) {
      this.route.params.subscribe(params => {
        this.roomCode = params['id'];
      });

      this.$roomSubscription = this.hubService.getRoom().subscribe(room => {
        this.room = room;

        // Now I need to check if it is the host, if it is not, then connect to the proper time in the video,
        // otherwise, we need to set the room to online and pull from the queue/ playlist
      });

      this.$youtubeSearchSubscription = this.hubService.getSearchYoutube().subscribe(songSearchResponses => {
        this.songSearchResults = songSearchResponses.results;
      });
    }

  ngOnInit(): void {
    // const id = this.route.snapshot.paramMap.get('id');
    this.hubService.requestRoom(this.roomCode);
  }

  ngOnDestroy(): void {
    this.$roomSubscription.unsubscribe();
    this.$youtubeSearchSubscription.unsubscribe();
  }

  onReady(player: any): void {
    this.youtubeService.setPlayer(player, true);
  }
  onChange(player: any): void {

  }

  requestSong(): void {
    this.hubService.requestSearchYoutube(this.searchSnippet);
    // this.youtubeService.loadVideo(this.searchSnippet);
    // this.youtubeService.play();
  }

  addSongToQueue(songSearch: ISongSearchResult): void {

  }

  playerState(): void {
    let state = this.youtubeService.getPlayerState();
  }

  skip(): void {

  }

  addToPlaylist(): void {

  }

}
