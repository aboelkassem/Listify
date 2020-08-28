import { RoomHubService } from './../services/room-hub.service';
import { ISongSearchResult, ISongQueued } from 'src/app/interfaces';
import { IRoom, ISongQueuedCreateRequest, ICurrency, IRoomInformation, IApplicationUserRoomCurrency } from './../interfaces';
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
  songsQueued: ISongQueued[];
  applicationUserRoomCurrencies: IApplicationUserRoomCurrency[] = this.roomHubService.applicationUserRoomCurrencies;
  applicationUserRoomCurrencyIdSelected: string;

  room: IRoom = this.roomHubService.room;
  songSearchResults: ISongSearchResult[] = [];

  $youtubeSearchSubscription: Subscription;
  $songsQueuedSubscription: Subscription;
  $playFromServerSubscription: Subscription;
  $roomReceivedSubscription: Subscription;
  $pingSubscription: Subscription;
  // $applicationUserSubscription: Subscription;
  // $applicationUserRoomCurrencySubscription: Subscription;
  // $currencySubscription: Subscription;
  // $roomSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private route: ActivatedRoute,
    private roomHubService: RoomHubService,
    private youtubeService: YoutubeService) {
      this.route.params.subscribe(params => {
        this.roomCode = params['id'];
      });

      // this.$applicationUserSubscription = this.hubService.getApplicationUser().subscribe(applicationUser => {
      //   // this.hubService.requestRoomByRoomCode(this.roomCode);
      //   this.roomHubService.connectToHub('https://localhost:44315/roomHub', this.roomCode);
      // });

      // this.$roomSubscription = this.hubService.getRoom().subscribe(room => {
      //   this.room = room;

      //   // Now I need to check if it is the host, if it is not, then connect to the proper time in the video,
      //   // otherwise, we need to set the room to online and pull from the queue/ playlist
      // });

      this.$youtubeSearchSubscription = this.hubService.getSearchYoutube().subscribe(songSearchResponse => {
        this.songSearchResults = songSearchResponse.results;
      });

      this.$roomReceivedSubscription = this.roomHubService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
        this.room = roomInformation.room;
        this.roomCode = roomInformation.room.roomCode;

        // if (!this.roomHubService.applicationUserRoom.isOwner) {
        //   this.roomHubService.requestServerState(this.roomHubService.room.id);
        // }
        // this.hubService.requestSongsQueued(roomInformation.room.id);

        // If it is the room owner, then request the next queued song, and then load the queue
        // If it is not the room owner, request the next song and position, then load the queue
      });

      this.$songsQueuedSubscription = this.roomHubService.getSongsQueued().subscribe(songsQueued => {
        this.songsQueued = songsQueued;
      });

      this.$playFromServerSubscription = this.roomHubService.getPlayFromServerResponse().subscribe(response => {
        // this.youtubeService.loadVideo(songQueued.song.youtubeId);
        // this.youtubeService.play();

        this.roomHubService.requestSongsQueued(this.roomHubService.room.id);
      });

      this.$pingSubscription = this.roomHubService.getPing().subscribe(ping => {
        if (ping === 'Ping') {
          this.roomHubService.requestPing();
        }
      });

      // this.$currencySubscription = this.hubService.getCurrencyActive().subscribe(currencyActive => {
      //   this.currency = currencyActive;
      //   this.currencyName = currencyActive.currencyName;
      // });

      // tslint:disable-next-line:max-line-length
      // this.$applicationUserRoomCurrencySubscription = this.hubService.getApplicationUserRoomCurrency().subscribe(applicationUserRoomCurrency => {
      //   this.quantity = applicationUserRoomCurrency.quantity.toString();
      // });

      // this.$songsQueuedSubscription = this.hubService.getSongsQueued().subscribe(songsQueued => {
      //   this.songsQueued = songsQueued;

      //   if (this.songsQueued !== undefined && this.songsQueued.length > 0){
      //     this.youtubeService.loadVideo(this.songsQueued[0].song.youtubeId);
      //     this.youtubeService.play();
      //   }
      // });
    }

  ngOnInit(): void {
    // if (this.roomHubService.isConnected()) {
    //   // const id = this.route.snapshot.paramMap.get('id');
    //   this.roomHubService.requestRoom(this.roomCode);
    // }
    // this.hubService.requestRoom(this.roomCode);
    // this.roomHubService.requestRoom(this.roomCode);
    if (this.hubService.isConnected) {
      this.roomHubService.connectToHub('https://localhost:44315/roomHub', this.roomCode);
    }

  }

  ngOnDestroy(): void {
    this.$youtubeSearchSubscription.unsubscribe();
    this.$roomReceivedSubscription.unsubscribe();
    this.$songsQueuedSubscription.unsubscribe();
    this.$playFromServerSubscription.unsubscribe();
    this.$pingSubscription.unsubscribe();
    // this.$applicationUserSubscription.unsubscribe();
    // this.$roomSubscription.unsubscribe();
    // this.$currencySubscription.unsubscribe();
    // this.$applicationUserRoomCurrencySubscription.unsubscribe();
  }

  // onReady(player: any): void {
  //   this.youtubeService.setPlayer(player, true);
  //   this.hubService.requestSongsQueued(this.room.id);
  // }
  // onChange(player: any): void {

  // }

  requestSong(): void {
    this.hubService.requestSearchYoutube(this.searchSnippet);
    // this.youtubeService.loadVideo(this.searchSnippet);
    // this.youtubeService.play();
  }

  addSongToQueue(searchResult: ISongSearchResult, applicationUserRoomCurrency: IApplicationUserRoomCurrency): void {
    const request: ISongQueuedCreateRequest = {
      applicationUserRoomId: this.roomHubService.applicationUserRoom.id,
      applicationUserRoomCurrencyId: applicationUserRoomCurrency.id,
      quantityWagered: searchResult.quantityWagered,
      songSearchResult: searchResult
    };

    // this.hubService.createSongQueued(request);
    this.roomHubService.createSongQueued(request);
    this.songSearchResults = [];
  }


  playerState(): void {
    let state = this.youtubeService.getPlayerState();
  }

  addToPlaylist(): void {

  }
}
