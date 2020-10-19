import { MatPaginator } from '@angular/material/paginator';
import { IConfirmationModalData, IInputModalData } from 'src/app/interfaces';
import { ConfirmationmodalComponent } from './../shared/modals/confirmationmodal/confirmationmodal.component';
import { IPurchasableLineItem, IPlaylistCreateRequest } from './../interfaces';
import { CartService } from './../services/cart.service';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { Component, OnInit, OnDestroy, ViewChild, AfterViewInit } from '@angular/core';
import { IPlaylist } from '../interfaces';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { GlobalsService } from '../services/globals.service';
import { MatSort } from '@angular/material/sort';
import { InputmodalComponent } from '../shared/modals/inputmodal/inputmodal.component';

@Component({
  selector: 'app-playlists',
  templateUrl: './playlists.component.html',
  styleUrls: ['./playlists.component.css']
})
export class PlaylistsComponent implements OnInit, OnDestroy, AfterViewInit {
  @ViewChild(MatPaginator) paginator: MatPaginator;
  @ViewChild(MatSort) sort: MatSort;

  displayedColumns: string[] = [
    'playlistImageUrl',
    'playlistName',
    'isSelected',
    'isPublic',
    'genreName',
    'deletePlaylist',
    'queuePlaylist'];
  dataSource = new MatTableDataSource<IPlaylist>();

  loading = false;

  playlists: IPlaylist[] = [];
  numberOfPlaylist = this.hubService.applicationUser.playlistCountMax;

  $playlistsSubscription: Subscription;
  $playlistSubscription: Subscription;
  $purchasableItemsSubscription: Subscription;

  constructor(
    private hubService: HubService,
    private cartService: CartService,
    private globalsService: GlobalsService,
    private toastrService: ToastrService,
    private router: Router,
    private modalDialog: MatDialog) {
    this.$playlistSubscription = this.hubService.getPlaylist().subscribe(playlist => {
      this.router.navigateByUrl('/playlist/' + playlist.id);
    });

    this.$playlistsSubscription = this.hubService.getPlaylists().subscribe(playlists => {
      this.playlists = playlists;
      this.loading = false;
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
    this.loading = true;
    this.hubService.requestPlaylists();
  }

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
    this.dataSource.sort = this.sort;
  }

  ngOnDestroy(): void {
    this.$playlistsSubscription.unsubscribe();
    this.$playlistSubscription.unsubscribe();
    this.$purchasableItemsSubscription.unsubscribe();
  }

  createPlaylist(): void {
    // we need logic to test if can create a new playlist - if not, navigate to the cart
    // ToDo: this data need to come from the back end
    if (this.hubService.applicationUser.playlistCountMax <= this.playlists.length) {
      this.hubService.requestPurchasableItems();
    }else {
      const inputData: IInputModalData = {
        title: 'Playlist Name',
        message: 'Please enter the new Playlist Name',
        placeholder: 'Enter Playlist Name ....',
        data: ''
      };

      const inputModal = this.modalDialog.open(InputmodalComponent, {
        width: '350px',
        data: inputData
      });

      inputModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          const request: IPlaylistCreateRequest = {
            id: undefined,
            playlistName: result,
            isSelected: false,
            isPublic: false,
            playlistGenres: []
          };
          this.hubService.savePlaylist(request);

          this.toastrService.success('You have successfully Created playlist',
            'Creating ' + request.playlistName);
        }
      });
    }
  }

  deletePlaylist(id: string): void {
    if (this.playlists.length <= 1) {
      this.toastrService.error('You Must have at least 1 playlist, so you can not delete this playlist', 'Cannot Deleted');
    }else {
      const confirmationModalData: IConfirmationModalData = {
        title: 'Are your sure ?',
        message: 'Are your sure you want to delete this playlist?',
        isConfirmed: false,
        confirmMessage: 'Confirm',
        cancelMessage: 'Cancel'
      };
      const confirmationModal = this.modalDialog.open(ConfirmationmodalComponent, {
        width: '250px',
        data: confirmationModalData
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

  queuePlaylist(id: string): void {
    let confirmationModalData: IConfirmationModalData = {
      title: 'Are your sure ?',
      message: 'Are your sure you want to add the entire playlist to your queue?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    let confirmationModal = this.modalDialog.open(ConfirmationmodalComponent, {
      width: '250px',
      data: confirmationModalData
    });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {

        confirmationModalData = {
          title: 'Randomize the playlist?',
          message: 'would you like to randomize the playlist in the queue?',
          isConfirmed: false,
          confirmMessage: 'Randomize',
          cancelMessage: 'Do not Randomize'
        };

        confirmationModal = this.modalDialog.open(ConfirmationmodalComponent, {
          width: '350px',
          data: confirmationModalData
        });

        confirmationModal.afterClosed().subscribe(randomizeResult => {
          this.loading = true;
          if (randomizeResult !== undefined) {
            this.hubService.requestQueuePlaylistInRoomHome(id, true);
          }else {
            this.hubService.requestQueuePlaylistInRoomHome(id, false);
          }
        });
      }
    });
  }
}
