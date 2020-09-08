import { Router } from '@angular/router';
import { IPurchasableItem } from './../../interfaces';
import { CartService } from './../../services/cart.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { IApplicationUserRoomCurrency, IRoomInformation } from 'src/app/interfaces';
import { MatTableDataSource } from '@angular/material/table';
import { Subscription } from 'rxjs';
import { RoomHubService } from 'src/app/services/room-hub.service';

@Component({
  selector: 'app-applicationuserroomcurrencies',
  templateUrl: './applicationuserroomcurrencies.component.html',
  styleUrls: ['./applicationuserroomcurrencies.component.css']
})
export class ApplicationuserroomcurrenciesComponent implements OnInit, OnDestroy {

  displayedColumns: string[] = ['currencyName', 'quantity', 'purchaseCurrency'];
  dataSource = new MatTableDataSource<IApplicationUserRoomCurrency>();

  applicationUserRoomCurrencies: IApplicationUserRoomCurrency[] = [];

  $roomReceivedSubscription: Subscription;
  $applicationUserRoomCurrencySubscription: Subscription;

  constructor(
    private roomHubService: RoomHubService,
    private router: Router,
    private cartService: CartService) {

    this.$roomReceivedSubscription = this.roomHubService.getRoomInformation().subscribe((roomInformation: IRoomInformation) => {
      this.applicationUserRoomCurrencies = roomInformation.applicationUserRoomCurrencies;
      this.dataSource.data = this.applicationUserRoomCurrencies;
    });

    // tslint:disable-next-line:max-line-length
    this.$applicationUserRoomCurrencySubscription = this.roomHubService.getApplicationUserRoomCurrency().subscribe(applicationUserRoomCurrency => {
      const originalCurrency = this.applicationUserRoomCurrencies.filter(x => x.id === applicationUserRoomCurrency.id)[0];

      if (originalCurrency !== undefined && originalCurrency !== null) {
        const index = this.applicationUserRoomCurrencies.indexOf(originalCurrency);
        this.applicationUserRoomCurrencies[index] = applicationUserRoomCurrency;
      }
    });
  }
  ngOnDestroy(): void {
    this.$applicationUserRoomCurrencySubscription.unsubscribe();
    this.$roomReceivedSubscription.unsubscribe();
  }

  ngOnInit(): void {
  }

  addApplicationUserRoomCurrency(applicationUserRoomCurrency: IApplicationUserRoomCurrency): void {
    const item: IPurchasableItem = {
      id: '',
      purchasableItemName: applicationUserRoomCurrency.currency.currencyName,
      purchasableItemType: 2,
      quantity: 5,
      unitCost: 5,
      discountApplied: 1,
      imageUri: ''
    };

    this.cartService.addPurchasableItemToCart(item);
    this.router.navigate(['/', 'cart']);

  }
}
