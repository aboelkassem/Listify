import { PlayerService } from './../../services/player.service';
import { MatDialog } from '@angular/material/dialog';
import { ToastrService } from 'ngx-toastr';
import { GlobalsService } from './../../services/globals.service';
import { HubService } from 'src/app/services/hub.service';
import { Router } from '@angular/router';
import { IPurchasableItem, IPurchasableCurrencyLineItem, IConfirmationModalData } from './../../interfaces';
import { CartService } from './../../services/cart.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { IApplicationUserRoomCurrencyRoom, IRoomInformation } from 'src/app/interfaces';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { RoomHubService } from 'src/app/services/room-hub.service';
import { ConfirmationmodalComponent } from '../modals/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-applicationuserroomcurrencies',
  templateUrl: './applicationuserroomcurrencies.component.html',
  styleUrls: ['./applicationuserroomcurrencies.component.css']
})
export class ApplicationuserroomcurrenciesComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['currencyName', 'quantity', 'purchaseCurrency', 'skipButton'];
  dataSource = new MatTableDataSource<IApplicationUserRoomCurrencyRoom>();

  applicationUserRoomCurrencies: IApplicationUserRoomCurrencyRoom[] = [];
  purchasableItems: IPurchasableItem[] = [];
  isRoomOwner = false;

  $roomReceivedSubscription: Subscription;
  $applicationUserRoomCurrencySubscription: Subscription;
  $purchasableItemsSubscription: Subscription;

  constructor(
    private roomHubService: RoomHubService,
    private playerService: PlayerService,
    private router: Router,
    private hubService: HubService,
    private confirmationModal: MatDialog,
    private globalsService: GlobalsService,
    private toastrService: ToastrService,
    private cartService: CartService) {

    this.$roomReceivedSubscription = this.roomHubService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
      this.applicationUserRoomCurrencies = roomInformation.applicationUserRoomCurrenciesRoom;
      this.dataSource.data = this.applicationUserRoomCurrencies;
      this.isRoomOwner = roomInformation.applicationUserRoom.isOwner;
      this.hubService.requestPurchasableItems();
    });

    // tslint:disable-next-line:max-line-length
    this.$applicationUserRoomCurrencySubscription = this.roomHubService.getApplicationUserRoomCurrencyRoom().subscribe(applicationUserRoomCurrency => {
      const originalCurrency = this.applicationUserRoomCurrencies.filter(x => x.id === applicationUserRoomCurrency.id)[0];

      if (originalCurrency !== undefined && originalCurrency !== null) {
        const index = this.applicationUserRoomCurrencies.indexOf(originalCurrency);
        this.applicationUserRoomCurrencies[index] = applicationUserRoomCurrency;
        this.dataSource.data = this.applicationUserRoomCurrencies;
      }
    });

    this.$purchasableItemsSubscription = this.hubService.getPurchasableItems().subscribe(purchasableItems => {
      this.purchasableItems = purchasableItems;
    });
  }
  ngOnDestroy(): void {
    this.$applicationUserRoomCurrencySubscription.unsubscribe();
    this.$roomReceivedSubscription.unsubscribe();
    this.$purchasableItemsSubscription.unsubscribe();
  }

  ngOnInit(): void {
    // this.hubService.requestPurchasableItems();
  }

  skipSong(): void {
    if (this.isRoomOwner) {
      const confirmationModalData: IConfirmationModalData = {
        title: 'Skip This Song ?',
        message: 'Are your sure you want to skip the current song?',
        isConfirmed: false,
        confirmMessage: 'Confirm',
        cancelMessage: 'Cancel'
      };

      const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
        width: '300px',
        data: confirmationModalData
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined && this.playerService.currentSong) {
          this.roomHubService.skipSong(this.playerService.currentSong);
        }
      });
    }
  }

  addApplicationUserRoomCurrency(applicationUserRoomCurrency: IApplicationUserRoomCurrencyRoom): void {
    const selectedItem = this.purchasableItems
    .filter(x => x.purchasableItemType === this.globalsService.getPurchasableItemType('PurchaseCurrency') && x.quantity === 40)[0];

    if (selectedItem) {
      const purchasableCurrencyLineItem: IPurchasableCurrencyLineItem = {
        purchasableItem: {
          id: selectedItem.id,
          purchasableItemName: selectedItem.purchasableItemName + ' ' + applicationUserRoomCurrency.currencyRoom.room.roomCode,
          purchasableItemType: selectedItem.purchasableItemType,
          discountApplied: selectedItem.discountApplied,
          unitCost: selectedItem.unitCost,
          quantity: selectedItem.quantity,
          imageUri: selectedItem.imageUri,
        },
        orderQuantity: 1,
        applicationUserRoomCurrencyId: applicationUserRoomCurrency.id
      };
      this.cartService.addPurchasableItemToCart(purchasableCurrencyLineItem);

      this.router.navigate(['/', 'cart']);

      // tslint:disable-next-line:max-line-length
      this.toastrService.success('You have added a ' + purchasableCurrencyLineItem.purchasableItem.purchasableItemName + ' to your cat', 'Add Success');
    }
  }
}
