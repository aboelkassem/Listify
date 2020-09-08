import { ConfirmationmodalService } from './../services/confirmationmodal.service';
import { ConfirmationmodalComponent } from './../shared/confirmationmodal/confirmationmodal.component';
import { IPurchasableItem } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { IPlaylist } from '../interfaces';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-playlists',
  templateUrl: './playlists.component.html',
  styleUrls: ['./playlists.component.css']
})
export class PlaylistsComponent implements OnInit, OnDestroy {
  displayedColumns: string[] = ['playlistName', 'deletePlaylist', 'isSelected'];
  dataSource = new MatTableDataSource<IPlaylist>();

  playlists: IPlaylist[] = [];
  numberOfPlaylist = this.hubService.applicationUser.playlistCountMax;

  $playlistsSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private cartService: CartService,
    private toastrService: ToastrService,
    private router: Router,
    private confirmationModalService: ConfirmationmodalService,
    private confirmationModal: MatDialog) {
    this.$playlistsSubscription = this.hubService.getPlaylists().subscribe(playlists => {
      this.playlists = playlists;
      this.dataSource.data = this.playlists;
    });
  }

  ngOnInit(): void {
    this.hubService.requestPlaylists();
  }
  ngOnDestroy(): void {
    this.$playlistsSubscription.unsubscribe();
  }

  createPlaylist(): void {
    // we need logic to test if can create a new playlist - if not, navigate to the cart
    // ToDo: this data need to come from the back end
    if (this.hubService.applicationUser.playlistCountMax > this.playlists.length) {
      const purchasablePlaylist: IPurchasableItem = {
        purchasableItemName: 'playlist',
        purchasableItemType: 0,
        quantity: 1,
        unitCost: 1,
        id: '',
        discountApplied: 1,
        imageUri: ''
      };
      this.toastrService.success('Added 1 playlist to your cart.', 'Added to Cart');

      this.cartService.addPurchasableItemToCart(purchasablePlaylist);

      this.router.navigateByUrl('/cart');
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
