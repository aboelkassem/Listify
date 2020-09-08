import { Router } from '@angular/router';
import { IPurchasableItem } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from 'src/app/services/hub.service';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { ConfirmationmodalComponent } from '../shared/confirmationmodal/confirmationmodal.component';
import { ConfirmationmodalService } from '../services/confirmationmodal.service';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-purchasableitems',
  templateUrl: './purchasableitems.component.html',
  styleUrls: ['./purchasableitems.component.css']
})
export class PurchasableitemsComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['purchasableItemName', 'purchasableItemType', 'quantity', 'unitCost', 'discountApplied', 'image', 'removePurchasableItem'];
  dataSource = new MatTableDataSource<IPurchasableItem>();

  purchasableItems: IPurchasableItem[] = [];
  purchasableItemTypes: string[] = [];

  $purchasableItemsSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private router: Router,
    private confirmationModal: MatDialog,
    private confirmationModalService: ConfirmationmodalService,
    private toastrService: ToastrService) {
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


  removePurchasableItem(id: string): void {

    this.confirmationModalService.setConfirmationModalMessage('remove this purchasable item');

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
      width: '250px',
      data: {isConfirmed: false}
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.deletePurchasableItem(id);
        this.router.navigateByUrl('/purchasableItems');

        this.toastrService.success('You have successfully deleted the purchasable item', 'Update Success');
      }
    });

  }
}
