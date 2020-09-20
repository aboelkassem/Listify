import { GlobalsService } from './../services/globals.service';
import { YoutubeService } from './../services/youtube.service';
import { RoomHubService } from './../services/room-hub.service';
import { ISongQueued } from 'src/app/interfaces';
import { IRoom, IRoomInformation } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit, OnDestroy {

  roomCode: string;
  songsQueued: ISongQueued[];

  allowRequests: boolean;
  isRoomOwner: boolean;
  room: IRoom = this.roomHubService.room;

  $songsQueuedSubscription: Subscription;
  $songQueuedSubscription: Subscription;
  $playFromServerSubscription: Subscription;
  $roomReceivedSubscription: Subscription;
  $pingSubscription: Subscription;
  $applicationUserReceivedSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private route: ActivatedRoute,
    private globalsService: GlobalsService,
    private youtubeService: YoutubeService,
    private roomHubService: RoomHubService) {
      this.route.params.subscribe(params => {
        this.roomCode = params['id'];
        // this.roomHubService.requestRoom(this.roomCode);
      });

      this.$roomReceivedSubscription = this.roomHubService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
        this.room = roomInformation.room;
        this.roomCode = roomInformation.room.roomCode;
        this.allowRequests = roomInformation.room.allowRequests;
        this.isRoomOwner = this.roomHubService.applicationUserRoom.isOwner;

        // deleted
        if (!this.roomHubService.applicationUserRoom.isOwner) {
          this.roomHubService.requestServerState(this.roomHubService.room.id);
        }
      });

      this.$applicationUserReceivedSubscription = this.hubService.getApplicationUser().subscribe(applicationUser => {
        this.roomHubService.connectToHub(this.globalsService.developmentWebAPIUrl + 'roomHub', this.roomCode);
      });

      this.$songsQueuedSubscription = this.roomHubService.getSongsQueued().subscribe(songsQueued => {
        this.songsQueued = songsQueued;
      });

      this.$songQueuedSubscription = this.roomHubService.getSongQueued().subscribe(songQueued => {
        this.roomHubService.requestApplicationUserRoomCurrencies();
      });

      this.$playFromServerSubscription = this.roomHubService.getPlayFromServerResponse().subscribe(response => {
        this.roomHubService.requestSongsQueued(this.roomHubService.room.id);

        // deleted
        this.youtubeService.loadVideoAndSeek(response.songQueued.song.youtubeId, response.currentTime);
        this.youtubeService.play();
      });

      this.$pingSubscription = this.roomHubService.getPing().subscribe(ping => {
        if (ping === 'Ping') {
          this.roomHubService.requestPing();
        }
      });

    }

  ngOnInit(): void {
    if (this.hubService.isConnected && this.hubService.applicationUser) {
      this.roomHubService.connectToHub(this.globalsService.developmentWebAPIUrl + 'roomHub', this.roomCode);
    }
  }

  ngOnDestroy(): void {
    this.$roomReceivedSubscription.unsubscribe();
    this.$songsQueuedSubscription.unsubscribe();
    this.$songQueuedSubscription.unsubscribe();
    this.$playFromServerSubscription.unsubscribe();
    this.$pingSubscription.unsubscribe();
    this.$applicationUserReceivedSubscription.unsubscribe();
  }
}
