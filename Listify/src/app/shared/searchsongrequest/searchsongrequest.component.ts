import { GlobalsService } from './../../services/globals.service';
import { RoomHubService } from './../../services/room-hub.service';
import { HubService } from './../../services/hub.service';
import { Component, OnInit, ViewChild, OnDestroy } from '@angular/core';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ISongSearchResult, ISongQueuedCreateRequest, IApplicationUserRoomCurrencyRoom, IRoomInformation } from 'src/app/interfaces';
import { Subscription } from 'rxjs';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-searchsongrequest',
  templateUrl: './searchsongrequest.component.html',
  styleUrls: ['./searchsongrequest.component.css']
})
export class SearchsongrequestComponent implements OnInit, OnDestroy {
  loading = false;

  @ViewChild(MatPaginator) paginator: MatPaginator;
  displayedColumns: string[] = ['songThumbnail', 'SongName', 'QuantityWager', 'CurrencyType', 'AddToQueue'];
  dataSource = new MatTableDataSource<ISongSearchResult>();

  roomCode: string;
  searchSnippet: string;
  songSearchResults: ISongSearchResult[] = [];
  applicationUserRoomCurrenciesRoom: IApplicationUserRoomCurrencyRoom[] = this.roomService.applicationUserRoomCurrenciesRoom;
  applicationUserRoomCurrency: IApplicationUserRoomCurrencyRoom;

  $youtubeSearchSubscription: Subscription;
  $roomReceivedSubscription: Subscription;
  $applicationUserRoomCurrencySubscription: Subscription;
  $applicationUserRoomCurrenciesSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private globalsService: GlobalsService,
    private toastrService: ToastrService,
    private roomService: RoomHubService) {

    this.$youtubeSearchSubscription = this.hubService.getSearchYoutube().subscribe(songSearchResponse => {
      this.songSearchResults = songSearchResponse.results;
      this.dataSource.data = this.songSearchResults;

      this.searchSnippet = '';
      this.loading = false;
    });

    this.$roomReceivedSubscription = this.roomService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
      this.roomCode = roomInformation.applicationUserRoom.room.roomCode;
      this.applicationUserRoomCurrenciesRoom = roomInformation.applicationUserRoomCurrenciesRoom;
      this.applicationUserRoomCurrency = roomInformation.applicationUserRoomCurrenciesRoom[0];

      // if (!this.roomService.applicationUserRoom.isOwner) {
      //   this.roomService.requestServerState(this.roomService.room.id);
      // }
    });

    this.$applicationUserRoomCurrencySubscription = this.roomService.getApplicationUserRoomCurrencyRoom()
      .subscribe(applicationUserRoomCurrency => {
        this.applicationUserRoomCurrency = applicationUserRoomCurrency;
    });

    this.$applicationUserRoomCurrenciesSubscription = this.roomService.getApplicationUserRoomCurrenciesRoom()
    .subscribe(applicationUserRoomCurrenciesRoom => {
      this.applicationUserRoomCurrenciesRoom = applicationUserRoomCurrenciesRoom;
      this.applicationUserRoomCurrency = applicationUserRoomCurrenciesRoom[0];
  });
  }

  ngOnInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  ngOnDestroy(): void {
    this.$youtubeSearchSubscription.unsubscribe();
    this.$roomReceivedSubscription.unsubscribe();
    this.$applicationUserRoomCurrencySubscription.unsubscribe();
    this.$applicationUserRoomCurrenciesSubscription.unsubscribe();
  }

  requestSong(): void {
    if (this.searchSnippet === undefined || this.searchSnippet === null || this.searchSnippet === '') {
      this.toastrService.warning('No Search terms were input, please try again', 'Invalid Search');
    }else {
      this.hubService.requestSearchYoutube(this.searchSnippet);
    }
  }

  clearSearch(): void {
    this.songSearchResults = [];
    this.dataSource.data = this.songSearchResults;

    this.searchSnippet = '';
  }

  addSongToQueue(searchResult: ISongSearchResult): void {
    if (searchResult.applicationUserRoomCurrencyId === '00000000-0000-0000-0000-000000000000' ||
      searchResult.applicationUserRoomCurrencyId === undefined ||
      searchResult.applicationUserRoomCurrencyId === null ) {
        this.toastrService.error('You must select a currency type to place a song request', 'Invalid Selected Currency');
    }else if (searchResult.quantityWagered <= 0 || searchResult.quantityWagered === undefined || searchResult.quantityWagered === null){
      this.toastrService.error('You must wager more than 0 to place a song request', 'Not Enough Wagered');
    }
    else {
      const correctCurrency = this.roomService.applicationUserRoomCurrenciesRoom
      .filter(x => x.id === searchResult.applicationUserRoomCurrencyId)[0];

      if (correctCurrency !== undefined && correctCurrency !== null) {

        if (searchResult.quantityWagered === undefined || searchResult.quantityWagered === null || searchResult.quantityWagered <= 0) {
          this.toastrService.warning('You must spend more than 0 to place a song request.', 'Spend Currency');
          return;
        }else {
          if (searchResult.quantityWagered > correctCurrency.quantity) {
            this.toastrService.error('You do not have enough ' + correctCurrency.currencyRoom.currency.currencyName
              + ' for this action. You have ' + correctCurrency.quantity + ' available.',
              'Not Enough Currency');
          }else {
            const request: ISongQueuedCreateRequest = {
              applicationUserRoomId: this.roomService.applicationUserRoom.id,
              applicationUserRoomCurrencyId: searchResult.applicationUserRoomCurrencyId,
              quantityWagered: searchResult.quantityWagered,
              songSearchResult: searchResult
            };

            this.roomService.createSongQueued(request);
            this.songSearchResults = [];
            this.toastrService.success('you have successfully added the song ' + request.songSearchResult.songName + 'to queue'
              , 'Song Added to Queue');
          }
        }
      }
    }
  }
}
