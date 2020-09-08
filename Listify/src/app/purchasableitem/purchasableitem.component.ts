import { IInjectableComponent, IContentComponent, ICurrency, IPurchasableItemCurrency } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, OnDestroy, ComponentFactoryResolver, ViewContainerRef } from '@angular/core';
import { IPurchasableItem } from '../interfaces';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-purchasableitem',
  templateUrl: './purchasableitem.component.html',
  styleUrls: ['./purchasableitem.component.css']
})
export class PurchasableitemComponent implements OnInit, OnDestroy {
  id: string;
  purchasableItemName: string;
  purchasableItemType: string;
  quantity: number;
  unitCost: number;
  imageUri: string;
  discountApplied: number;

  purchasableItemTypes: string[] = [];
  purchasableItem: IPurchasableItem;
  currencies: ICurrency[] = [];
  currency: ICurrency;

  $purchasableItemSubscription: Subscription;
  $currenciesSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private route: ActivatedRoute) {
    this.route.params.subscribe(params => {
      const id = params['id'];

      if (id !== undefined || id !== null) {
        this.hubService.requestPurchasableItem(id);
      }
    });

    this.$purchasableItemSubscription = this.hubService.getPurchasableItem().subscribe(purchasableItem => {
      this.purchasableItem = purchasableItem;
      this.id = this.purchasableItem.id;
      this.purchasableItemName = this.purchasableItem.purchasableItemName;
      this.purchasableItemType = this.getPurchasableItemsTypes()[this.purchasableItem.purchasableItemType];
      this.quantity = this.purchasableItem.quantity;
      this.unitCost = this.purchasableItem.unitCost;
      this.imageUri = this.purchasableItem.imageUri;
      this.discountApplied = this.purchasableItem.discountApplied;
    });

    this.$currenciesSubscription = this.hubService.getCurrencies().subscribe(currencies => {
      this.currencies = currencies;
    });
  }

  ngOnInit(): void {
    this.purchasableItemTypes = this.getPurchasableItemsTypes();
  }

  ngOnDestroy(): void {
    this.$purchasableItemSubscription.unsubscribe();
    this.$currenciesSubscription.unsubscribe();
  }

  savePurchasableItem(): void {
    if (this.currency === undefined || this.currency === null) {
      const request: IPurchasableItem = {
        id: this.id,
        purchasableItemName: this.purchasableItemName,
        purchasableItemType: this.getPurchasableItemType(this.purchasableItemType),
        quantity: this.quantity,
        unitCost: this.unitCost,
        imageUri: this.imageUri,
        discountApplied: this.discountApplied,
      };

      this.hubService.savePurchasableItem(request);
    }else {
      const request: IPurchasableItemCurrency = {
        id: this.id,
        purchasableItemName: this.purchasableItemName,
        purchasableItemType: this.getPurchasableItemType(this.purchasableItemType),
        quantity: this.quantity,
        unitCost: this.unitCost,
        imageUri: this.imageUri,
        discountApplied: this.discountApplied,
        currency: this.currency
      };

      this.hubService.savePurchasableItemCurrency(request);
    }
    this.router.navigateByUrl('/purchasableItems');
  }

  purchasableItemTypeChange(event: any): void {
    if (event === 'PurchaseCurrency') {
      this.hubService.requestCurrencies();
    }else {
      this.currencies = [];
      this.currency = undefined;
    }
  }

  getPurchasableItemType(type: string): number {
    switch (type) {
      case 'Playlist':
        return 0;
        break;
      case 'PlaylistSongs':
        return 1;
        break;
      case 'PurchaseCurrency':
        return 2;
        break;
    }
  }

  getPurchasableItemsTypes(): string[] {
    return ['Playlist', 'PlaylistSongs', 'PurchaseCurrency'];
  }
}
