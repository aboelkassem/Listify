import { CartService } from './../services/cart.service';
import { Router } from '@angular/router';
import { IPurchasableItem, IPurchasableLineItem } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { GlobalsService } from '../services/globals.service';
import { MatPaginator } from '@angular/material/paginator';
import { MatSort } from '@angular/material/sort';

@Component({
  selector: 'app-purchasableitems',
  templateUrl: './purchasableitems.component.html',
  styleUrls: ['./purchasableitems.component.css']
})
export class PurchasableitemsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = ['image', 'purchasableItemName', 'purchasableItemType', 'quantity', 'unitCost', 'discountApplied'];
  dataSource = new MatTableDataSource<IPurchasableItem>();

  purchasableItems: IPurchasableItem[] = [];
  purchasableItemTypes: string[] = [];

  loading = false;

  $purchasableItemsSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private cartService: CartService,
    private globalsService: GlobalsService,
    private toastrService: ToastrService) {
    this.$purchasableItemsSubscription = this.hubService.getPurchasableItems().subscribe(purchasableItems => {
      // get all packages of purchasable items in only playlists and songs playlist
      // because purchase currencies will puy only it from the room page including another details
      this.purchasableItems = purchasableItems.filter(x => x.purchasableItemType !== this.globalsService.getPurchasableItemType('PurchaseCurrency'));
      this.dataSource.data = this.purchasableItems;
      this.dataSource.paginator = this.paginator;
      this.dataSource.sort = this.sort;
      this.loading = false;
    });
   }

  ngOnInit(): void {
    this.loading = true;
    this.hubService.requestPurchasableItems();
    this.purchasableItemTypes = this.getPurchasableItemsTypes();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.$purchasableItemsSubscription.unsubscribe();
  }

  getPurchasableItemsTypes(): string[] {
    return ['Playlist', 'PlaylistSongs', 'PurchaseCurrency'];
  }


  addPurchasableItemToCart(id: string): void {
    const selectedItem = this.purchasableItems.filter(x => x.id === id)[0];

    if (selectedItem) {
      const purchasableLineItem: IPurchasableLineItem = {
        purchasableItem: selectedItem,
        orderQuantity: 1
      };

      this.cartService.addPurchasableItemToCart(purchasableLineItem);
      this.router.navigate(['/', 'cart']);

      this.toastrService.success('You have added a ' + purchasableLineItem.purchasableItem.purchasableItemName + ' to your cat', 'Add Success');
    }
  }
}
