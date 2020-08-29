import { Router } from '@angular/router';
import { IApplicationUserRequest, IPurchasableItem } from './../interfaces';
import { HubService } from './../services/hub.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { CartService } from '../services/cart.service';

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

  $userInformationSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private cartService: CartService,
    private router: Router) {
      this.$userInformationSubscription = this.hubService.getApplicationUser().subscribe(user => {
        this.id = user.id;
        this.username = user.username;
        this.playlistSongCount = user.playlistSongCount;
        this.playlistCount = user.playlistCountMax;
        this.roomCode = user.room.roomCode;
      });
    }

  ngOnInit(): void {
    this.hubService.requestApplicationUserInformation();
  }

  ngOnDestroy(): void {
    this.$userInformationSubscription.unsubscribe();
  }

  saveApplicationUserInfo(): void {
    const request: IApplicationUserRequest = {
      id: this.id,
      username: this.username,
      playlistSongCount: this.playlistSongCount,
      playlistCountMax: this.playlistCount,
      roomCode: this.roomCode
    };

    this.hubService.updateApplicationUserInformation(request);

    this.router.navigate(['/home']);
  }

  addPlaylistCount(): void {
    // this needs to come from backend
    // pass here the purchasable object
    const purchasableObject: IPurchasableItem = {
      purchasableItemType: 0,
      purchasableItemName: 'Add an additional Playlist',
      id: '',
      quantity: 1,
      unitCost: 5,
      lineCost: 1 * 5
    };

    this.cartService.addPurchasableItemToCart(purchasableObject);

    this.router.navigate(['/cart']);
  }

  addPlaylistSongCount(): void {
    // this needs to come from backend
    // pass here the purchasable object
    const purchasableObject: IPurchasableItem = {
      purchasableItemType: 1,
      purchasableItemName: 'Add an additional song playlist slots',
      id: '',
      quantity: 1,
      unitCost: 2,
      lineCost: 1 * 2
    };

    this.cartService.addPurchasableItemToCart(purchasableObject);

    this.router.navigate(['/cart']);
  }
}
