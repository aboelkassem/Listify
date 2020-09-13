import { ConfirmationmodalService } from './../services/confirmationmodal.service';
import { ConfirmationmodalComponent } from './../shared/confirmationmodal/confirmationmodal.component';
import { IPurchasableItem, IPurchasableLineItem } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { IPlaylist } from '../interfaces';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { GlobalsService } from '../services/globals.service';

@Component({
  selector: 'app-playlists',
  templateUrl: './playlists.component.html',
  styleUrls: ['./playlists.component.css']
})
export class PlaylistsComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['playlistName', 'isSelected', 'deletePlaylist'];
  dataSource = new MatTableDataSource<IPlaylist>();

  playlists: IPlaylist[] = [];
  numberOfPlaylist = this.hubService.applicationUser.playlistCountMax;

  $playlistsSubscription: Subscription;
  $purchasableItemsSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private cartService: CartService,
    private globalsService: GlobalsService,
    private toastrService: ToastrService,
    private router: Router,
    private confirmationModalService: ConfirmationmodalService,
    private confirmationModal: MatDialog) {
    this.$playlistsSubscription = this.hubService.getPlaylists().subscribe(playlists => {
      this.playlists = playlists;
      this.dataSource.data = this.playlists;
    });

    this.$purchasableItemsSubscription = this.hubService.getPurchasableItems().subscribe(purchasableItems => {
      const playlistPurchasableItem = purchasableItems
        .filter(x => x.purchasableItemType === this.globalsService.getPurchasableItemType('Playlist') && x.quantity === 1)[0];

      const purchasableLineItem: IPurchasableLineItem = {
        purchasableItem: playlistPurchasableItem,
        orderQuantity: 1
      };

      this.toastrService.success('Added 1 playlist to your cart', 'Added To Cart');
      this.cartService.addPurchasableItemToCart(purchasableLineItem);
      this.router.navigateByUrl('/cart');
    });
  }

  ngOnInit(): void {
    this.hubService.requestPlaylists();
  }
  ngOnDestroy(): void {
    this.$playlistsSubscription.unsubscribe();
    this.$purchasableItemsSubscription.unsubscribe();
  }

  createPlaylist(): void {
    // we need logic to test if can create a new playlist - if not, navigate to the cart
    // ToDo: this data need to come from the back end
    if (this.hubService.applicationUser.playlistCountMax < this.playlists.length) {
      this.hubService.requestPurchasableItems();
    }else {
      this.router.navigateByUrl('/playlist');
    }
  }

  deletePlaylist(id: string): void {
    if (this.playlists.length <= 1) {
      this.toastrService.error('You Must have at least 1 playlist, so you can not delete this playlist', 'Cannot Deleted');
    }else {
      this.confirmationModalService.setConfirmationModalMessage('delete This Playlist');

      const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
        width: '250px',
        data: {isConfirmed: false}
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          this.hubService.deletePlaylist(id);
          this.hubService.requestPlaylists();

          this.toastrService.success('You have successfully deleted the playlist',
            'Deleting ' + this.playlists.filter(x => x.id === id)[0].playlistName);
        }
      });
    }
  }
}
