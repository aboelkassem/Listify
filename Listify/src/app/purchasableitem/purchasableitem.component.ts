import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
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

  $purchasableItemSubscription: Subscription;

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
      this.purchasableItemType = this.purchasableItem.purchasableItemType;
      this.quantity = this.purchasableItem.quantity;
      this.unitCost = this.purchasableItem.unitCost;
      this.imageUri = this.purchasableItem.imageUri;
      this.discountApplied = this.purchasableItem.discountApplied;
    });
  }

  ngOnInit(): void {
    this.purchasableItemTypes = this.getPurchasableItemsTypes();
  }

  ngOnDestroy(): void {
    this.$purchasableItemSubscription.unsubscribe();
  }

  savePurchasableItem(): void {
    const request: IPurchasableItem = {
      id: this.id,
      purchasableItemName: this.purchasableItemName,
      purchasableItemType: this.purchasableItemType,
      quantity: this.quantity,
      unitCost: this.unitCost,
      imageUri: this.imageUri,
      discountApplied: this.discountApplied,
    };

    this.hubService.savePurchasableItem(request);

    this.router.navigateByUrl('/purchasableItems');
  }

  getPurchasableItemsTypes(): string[] {
    return ['Playlist', 'PlaylistSongs', 'PurchaseCurrency'];
  }
}
