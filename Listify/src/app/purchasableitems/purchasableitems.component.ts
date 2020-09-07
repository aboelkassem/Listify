import { IPurchasableItem } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';

@Component({
  selector: 'app-purchasableitems',
  templateUrl: './purchasableitems.component.html',
  styleUrls: ['./purchasableitems.component.css']
})
export class PurchasableitemsComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['purchasableItemName', 'purchasableItemType', 'quantity', 'unitCost', 'discountApplied', 'image'];
  dataSource = new MatTableDataSource<IPurchasableItem>();

  purchasableItems: IPurchasableItem[] = [];
  purchasableItemTypes: string[] = [];

  $purchasableItemsSubscription: Subscription;

  constructor(private hubService: HubService) {
    this.$purchasableItemsSubscription = this.hubService.getPurchasableItems().subscribe(purchasableItems => {
      this.purchasableItems = purchasableItems;
      this.dataSource.data = this.purchasableItems;
    });
   }

  ngOnInit(): void {
    this.hubService.requestPurchasableItems();
    this.purchasableItemTypes = this.getPurchasableItemsTypes();
  }

  ngOnDestroy(): void {
    this.$purchasableItemsSubscription.unsubscribe();
  }

  getPurchasableItemsTypes(): string[] {
    return ['Playlist', 'PlaylistSongs', 'PurchaseCurrency'];
  }
}
