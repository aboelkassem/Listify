import { ISongSearchResult, ISongQueued } from 'src/app/interfaces';
import { IRoom, ISongQueuedCreateRequest, ICurrency } from './../interfaces';
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
  currency: ICurrency;
  quantity: string;
  currencyName: string;
  songsQueued: ISongQueued[];

  room: IRoom;
  songSearchResults: ISongSearchResult[] = [];

  $applicationUserRoomCurrencySubscription: Subscription;
  $roomSubscription: Subscription;
  $connectedToHubSubscription: Subscription;
  $youtubeSearchSubscription: Subscription;
  $currencySubscription: Subscription;
  $songsQueuedSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private route: ActivatedRoute,
    private youtubeService: YoutubeService) {
      this.route.params.subscribe(params => {
        this.roomCode = params['id'];
      });

      this.$connectedToHubSubscription = this.hubService.getUserInfo().subscribe(applicationUser => {
        this.hubService.requestRoom(this.roomCode);
        this.hubService.requestCurrencyActive();
      });

      this.$roomSubscription = this.hubService.getRoom().subscribe(room => {
        this.room = room;

        // Now I need to check if it is the host, if it is not, then connect to the proper time in the video,
        // otherwise, we need to set the room to online and pull from the queue/ playlist
      });

      this.$youtubeSearchSubscription = this.hubService.getSearchYoutube().subscribe(songSearchResponses => {
        this.songSearchResults = songSearchResponses.results;
      });

      this.$currencySubscription = this.hubService.getCurrencyActive().subscribe(currencyActive => {
        this.currency = currencyActive;
        this.currencyName = currencyActive.currencyName;
      });

      // tslint:disable-next-line:max-line-length
      this.$applicationUserRoomCurrencySubscription = this.hubService.getApplicationUserRoomCurrency().subscribe(applicationUserRoomCurrency => {
        this.quantity = applicationUserRoomCurrency.quantity.toString();
      });

      this.$songsQueuedSubscription = this.hubService.getSongsQueued().subscribe(songsQueued => {
        this.songsQueued = songsQueued;

        if (this.songsQueued !== undefined && this.songsQueued.length > 0){
          this.youtubeService.loadVideo(this.songsQueued[0].song.youtubeId);
          this.youtubeService.play();
        }
      });
    }

  ngOnInit(): void {
    if (this.hubService.isConnected()) {
      // const id = this.route.snapshot.paramMap.get('id');
      this.hubService.requestRoom(this.roomCode);
      this.hubService.requestCurrencyActive();
    }
  }

  ngOnDestroy(): void {
    this.$roomSubscription.unsubscribe();
    this.$youtubeSearchSubscription.unsubscribe();
    this.$currencySubscription.unsubscribe();
    this.$applicationUserRoomCurrencySubscription.unsubscribe();
    this.$connectedToHubSubscription.unsubscribe();
    this.$songsQueuedSubscription.unsubscribe();
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

  addSongToPlaylist(searchResult: ISongSearchResult): void {
    const request: ISongQueuedCreateRequest = {
      applicationUserRoomId: this.hubService.applicationUserRoomCurrent.id,
      currencyId: this.currency.id,
      quantityWagered: searchResult.quantityWagered,
      songSearchResult: searchResult
    };

    this.hubService.createSongQueued(request);
  }


  playerState(): void {
    let state = this.youtubeService.getPlayerState();
  }

  addToPlaylist(): void {

  }

}
