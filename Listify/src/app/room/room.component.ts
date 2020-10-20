import { InformationmodalComponent } from './../shared/modals/informationmodal/informationmodal.component';
import { MatDialog } from '@angular/material/dialog';
import { GlobalsService } from './../services/globals.service';
import { YoutubeService } from './../services/youtube.service';
import { RoomHubService } from './../services/room-hub.service';
import { ISongQueued } from 'src/app/interfaces';
import { IRoom, IRoomInformation, IInformationModalData } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-room',
  templateUrl: './room.component.html',
  styleUrls: ['./room.component.css']
})
export class RoomComponent implements OnInit, OnDestroy {

  loading = false;

  roomCode: string;
  songsQueued: ISongQueued[];

  allowRequests: boolean;
  isRoomOwner: boolean;
  room: IRoom = this.roomService.room;

  $songsQueuedSubscription: Subscription;
  $songQueuedSubscription: Subscription;
  $queuePlaylistInHomeSubscription: Subscription;
  $playFromServerSubscription: Subscription;
  $roomReceivedSubscription: Subscription;
  $pingSubscription: Subscription;
  $applicationUserReceivedSubscription: Subscription;
  $forceDisconnectSubscription: Subscription;
  $RoomOwnerLogoutReceivedSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private route: ActivatedRoute,
    private router: Router,
    private globalsService: GlobalsService,
    private youtubeService: YoutubeService,
    private matDialog: MatDialog,
    private roomService: RoomHubService) {
      this.route.params.subscribe(params => {
        this.roomCode = params['id'];
        // this.roomHubService.requestRoom(this.roomCode);
      });

      this.$forceDisconnectSubscription = this.roomService.getForceDisconnect().subscribe(data => {
        const informationModalData: IInformationModalData = {
          title: 'Disconnected',
          message: 'You have been disconnected from the server.'
        };

        this.matDialog.open(InformationmodalComponent, {
          width: '250px',
          data: informationModalData
        });

        this.youtubeService.stop();
        this.roomService.disconnectFromHub();
      });

      this.$roomReceivedSubscription = this.roomService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
        this.room = roomInformation.room;
        this.roomCode = roomInformation.room.roomCode;
        this.allowRequests = roomInformation.room.allowRequests;
        this.isRoomOwner = this.roomService.applicationUserRoom.isOwner;
        this.roomService.requestLastMessagesForRoom(this.roomService.applicationUserRoom?.room?.id);

        // deleted
        if (!this.roomService.applicationUserRoom.isOwner) {
          this.roomService.requestServerState(this.roomService.room.id);
        }
      });

      this.$applicationUserReceivedSubscription = this.hubService.getApplicationUser().subscribe(applicationUser => {
        this.roomService.connectToHub(this.globalsService.developmentWebAPIUrl + 'roomHub', this.roomCode);
      });

      this.$songsQueuedSubscription = this.roomService.getSongsQueued().subscribe(songsQueued => {
        this.songsQueued = songsQueued;
      });

      this.$songQueuedSubscription = this.roomService.getSongQueued().subscribe(songQueued => {
        this.roomService.requestApplicationUserRoomCurrencies();
      });

      this.$queuePlaylistInHomeSubscription = this.hubService.getQueuePlaylistInRoomHome().subscribe(songsQueued => {
        this.loading = false;
        this.roomService.requestSongsQueued(this.room.id);
      });

      this.$playFromServerSubscription = this.roomService.getPlayFromServerResponse().subscribe(response => {
        this.roomService.requestSongsQueued(this.roomService.room.id);

        // deleted
        this.youtubeService.loadVideoAndSeek(response.songQueued.song.youtubeId, response.currentTime);
        this.youtubeService.play();
      });

      this.$RoomOwnerLogoutReceivedSubscription = this.roomService.getRoomOwnerLogout().subscribe(isLogged => {
        if (isLogged) {
          const informationModalData: IInformationModalData = {
            title: 'Room Owner Disconnected',
            message: 'Sorry! The Owner for this room is logout, so this room is not online anymore. you will redirected to your room.'
          };

          this.matDialog.open(InformationmodalComponent, {
            width: '350px',
            data: informationModalData
          }).afterClosed().subscribe(result => {
              this.router.navigateByUrl('/');
          });
        }
      });

      this.$pingSubscription = this.roomService.getPing().subscribe(ping => {
        if (ping === 'Ping') {
          this.roomService.requestPing();
        }
      });

    }

  ngOnInit(): void {
    if (this.hubService.isConnected && this.hubService.applicationUser) {
      this.roomService.connectToHub(this.globalsService.developmentWebAPIUrl + 'roomHub', this.roomCode);
    }
  }

  ngOnDestroy(): void {
    this.$roomReceivedSubscription.unsubscribe();
    this.$songsQueuedSubscription.unsubscribe();
    this.$songQueuedSubscription.unsubscribe();
    this.$playFromServerSubscription.unsubscribe();
    this.$pingSubscription.unsubscribe();
    this.$applicationUserReceivedSubscription.unsubscribe();
    this.$queuePlaylistInHomeSubscription.unsubscribe();
    this.$RoomOwnerLogoutReceivedSubscription.unsubscribe();
  }
}
