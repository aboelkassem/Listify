import { ConfirmationmodalService } from './../services/confirmationmodal.service';
import { Router } from '@angular/router';
import { IApplicationUserRequest, IPurchasableItem } from './../interfaces';
import { HubService } from './../services/hub.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CartService } from '../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationmodalComponent } from '../shared/confirmationmodal/confirmationmodal.component';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit, OnDestroy {

  id: string;
  username: string;
  roomCode: string;
  playlistSongCount: number;
  playlistCount: number;

  disableAttr = false;
  private _nextRequestType: NextRequestType;

  $userInformationSubscription: Subscription;
  $purchasableItemsSubscription: Subscription;
  $purchasableItemSubscription: Subscription;

  constructor(
    private router: Router,
    private hubService: HubService,
    private cartService: CartService,
    private toastrService: ToastrService,
    private confirmationModal: MatDialog,
    private confirmationModalService: ConfirmationmodalService) {
      this.$userInformationSubscription = this.hubService.getApplicationUser().subscribe(user => {
        this.id = user.id;
        this.username = user.username;
        this.playlistSongCount = user.playlistSongCount;
        this.playlistCount = user.playlistCountMax;
        this.roomCode = user.room.roomCode;
      });

      this.$purchasableItemsSubscription = this.hubService.getPurchasableItems().subscribe(purchasableItems => {
        let selectedItem: IPurchasableItem;

        switch (this._nextRequestType) {
          case NextRequestType.Playlist:
          // tslint:disable-next-line:max-line-length
          selectedItem = purchasableItems.filter(x => x.purchasableItemType === this.getPurchasableItemType('Playlist') && x.quantity === 1)[0];
          break;

          case NextRequestType.PlaylistSongs:
          // tslint:disable-next-line:max-line-length
          selectedItem = purchasableItems.filter(x => x.purchasableItemType === this.getPurchasableItemType('PlaylistSongs') && x.quantity === 15)[0];
          break;
        }

        if (selectedItem) {
          this.hubService.requestPurchasableItem(selectedItem.id);
        }
      });

      this.$purchasableItemSubscription = this.hubService.getPurchasableItem().subscribe(purchasableItem => {
        this.addPurchasableItemToCard(purchasableItem);
      });
    }

  ngOnInit(): void {
    this.hubService.requestApplicationUserInformation();
  }

  ngOnDestroy(): void {
    this.$userInformationSubscription.unsubscribe();
    this.$purchasableItemsSubscription.unsubscribe();
    this.$purchasableItemSubscription.unsubscribe();
  }

  changeUsername(): void {
    // just make the input field editable and save the request
    this.disableAttr = true;
  }

  saveApplicationUserInfo(): void {

    if (this.roomCode === undefined || this.roomCode === null || this.roomCode === '') {
      this.toastrService.error('You have selected and invalid room code, please change the room code and try again', 'Invalid Room Code');
    }else {

      this.confirmationModalService.setConfirmationModalMessage('update your account information');

      const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
        width: '250px',
        data: {isConfirmed: false}
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          const request: IApplicationUserRequest = {
            id: this.id,
            username: this.username,
            playlistSongCount: this.playlistSongCount,
            playlistCountMax: this.playlistCount,
            roomCode: this.roomCode
          };

          this.hubService.updateApplicationUserInformation(request);

          this.router.navigate(['/', 'account']);

          this.toastrService.success('You have updated your account information successfully', 'Updated Successfully');
        }
      });
    }
  }

  addPlaylistSongCount(): void {
    this._nextRequestType = NextRequestType.PlaylistSongs;
    this.hubService.requestPurchasableItems();
  }

  addPlaylistCount(): void {
    this._nextRequestType = NextRequestType.Playlist;
    this.hubService.requestPurchasableItems();
  }

  addPurchasableItemToCard(purchasableItem: IPurchasableItem): void {
    this.cartService.addPurchasableItemToCart(purchasableItem);

    this.router.navigate(['/', 'cart']);

    this.toastrService.success('You have added a ' + purchasableItem.purchasableItemName + ' to your cat', 'Add Success');
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
}

enum NextRequestType {
  Playlist,
  PlaylistSongs
}
