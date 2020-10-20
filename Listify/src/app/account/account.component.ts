import { HttpService } from './../services/http.service';
import { RoomHubService } from './../services/room-hub.service';
import { Router } from '@angular/router';
// tslint:disable-next-line:max-line-length
import { IApplicationUserRequest, IConfirmationModalData, IGenre, IPurchasableItem, IPurchasableLineItem, IValidatedTextRequest, IRoomGenre } from './../interfaces';
import { HubService } from './../services/hub.service';
import { Subscription } from 'rxjs';
import { Component, OnInit, OnDestroy, ViewChild } from '@angular/core';
import { CartService } from '../services/cart.service';
import { ToastrService } from 'ngx-toastr';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationmodalComponent } from '../shared/modals/confirmationmodal/confirmationmodal.component';
import { GlobalsService } from '../services/globals.service';

@Component({
  selector: 'app-account',
  templateUrl: './account.component.html',
  styleUrls: ['./account.component.css']
})
export class AccountComponent implements OnInit, OnDestroy {

  @ViewChild('file') file: any;
  files: Set<File> = new Set();
  @ViewChild('accountForm') accountForm: any;
  loading = false;

  id: string;
  username: string;
  roomCode: string;
  roomTitle: string;
  roomKey: string;
  playlistSongCount: number;
  playlistCount: number;
  allowRequests: boolean;
  isLocked: boolean;
  isPublic: boolean;
  matureContent: boolean;
  matureContentChat: boolean;
  chatColor: string;
  profileTitle: string;
  profileDescription: string;
  profileImageUrl: string;
  roomImageUrl: string;

  genres: IGenre[] = [];
  roomGenres: IRoomGenre[] = [];
  genreSelectedId: string;

  isUpdatingProfile: false;

  disableAttr = false;
  private _nextRequestType: NextRequestType;
  private _nextUploadType: NextUploadType;

  $applicationUserSubscription: Subscription;
  $purchasableItemsSubscription: Subscription;
  $purchasableItemSubscription: Subscription;
  $validatedTextReceivedSubscription: Subscription;
  $clearUserProfileImageSubscription: Subscription;
  $clearRoomImageSubscription: Subscription;
  $genreAccountSubscription: Subscription;

  constructor(
    private router: Router,
    private hubService: HubService,
    private roomService: RoomHubService,
    private httpService: HttpService,
    private cartService: CartService,
    private globalsService: GlobalsService,
    private toastrService: ToastrService,
    private confirmationModal: MatDialog) {

      this.$genreAccountSubscription = this.hubService.getGenresRoom().subscribe(genres => {
        this.genres = genres;
      });

      this.$clearUserProfileImageSubscription = this.hubService.getClearUserProfileImage().subscribe(isSuccess => {
        this.loading = false;

        if (isSuccess) {
          this.toastrService.success('You have successfully cleared your profile image.', 'Deleted Profile Image Success');
        }else {
          this.toastrService.error('There was a problem communicating with the server, please try again', 'Deleted Failed');
        }
      });

      this.$clearRoomImageSubscription = this.hubService.getClearRoomImage().subscribe(isSuccess => {
        this.loading = false;

        if (isSuccess) {
          this.toastrService.success('You have successfully cleared your Room image.', 'Deleted Room Image Success');
        }else {
          this.toastrService.error('There was a problem communicating with the server, please try again', 'Deleted Failed');
        }
      });

      this.$validatedTextReceivedSubscription = this.hubService.getValidatedTextReceived().subscribe(validatedTextResponse => {
        switch (this.globalsService.getValidatedTextTypes()[validatedTextResponse.validatedTextType]) {
          case 'Username':
            if (validatedTextResponse.isAvailable) {
              // Username available, validate the next thing and update the information
              const validatedTextRequest: IValidatedTextRequest = {
                content: this.roomCode,
                validatedTextType: this.globalsService.getValidatedTextType('RoomCode')
              };
              this.hubService.requestValidatedText(validatedTextRequest);
            }else {
              this.toastrService.error('This username is not available, Please try another one', 'Username Unavailable');
            }
            break;
          case 'RoomCode':
            if (validatedTextResponse.isAvailable) {
              // Username available, validate the next thing and update the information
              const validatedTextRequest: IValidatedTextRequest = {
                content: this.roomTitle,
                validatedTextType: this.globalsService.getValidatedTextType('RoomTitle')
              };
              this.hubService.requestValidatedText(validatedTextRequest);
            }else {
              this.toastrService.error('This RoomCode is not available, Please try another one', 'RoomCode Unavailable');
            }
            break;
          case 'RoomTitle':
            if (validatedTextResponse.isAvailable) {
              const validatedTextRequest: IValidatedTextRequest = {
                content: this.profileDescription,
                validatedTextType: this.globalsService.getValidatedTextType('ProfileDescription')
              };
              this.hubService.requestValidatedText(validatedTextRequest);
            }else {
              this.toastrService.error('This RoomTitle is not available, Please try another one', 'RoomTitle Unavailable');
            }
            break;
          case 'ProfileDescription':
            if (validatedTextResponse.isAvailable) {
              // Update the user information
              const request: IApplicationUserRequest = {
                id: this.id,
                username: this.username,
                roomCode: this.roomCode,
                roomTitle: this.roomTitle,
                roomKey: this.roomKey,
                allowRequests: this.allowRequests,
                isRoomLocked: this.isLocked,
                isRoomPublic: this.isPublic,
                matureContent: this.matureContent,
                matureContentChat: this.matureContentChat,
                chatColor: this.chatColor,
                profileTitle: this.profileTitle,
                profileDescription: this.profileDescription,
                roomGenres: this.roomGenres
              };

              this.roomService.updateApplicationUser(request);
              this.roomService.requestUpdatedChatColor();

              this.router.navigate(['/', 'account']);

              this.toastrService.success('You have updated your account information successfully', 'Updated Successfully');
            }else {
              this.toastrService.error('This Room Description is not available, Please try another one', 'Description Unavailable');
            }
            break;
        }
      });

      this.$applicationUserSubscription = this.roomService.getApplicationUser().subscribe(user => {
        this.id = user.id;
        this.username = user.username;
        this.playlistSongCount = user.playlistSongCount;
        this.playlistCount = user.playlistCountMax;
        this.roomCode = user.room.roomCode;
        this.isLocked = user.room.isRoomLocked;
        this.isPublic = user.room.isRoomPublic;
        this.roomKey = user.room.roomKey;
        this.roomTitle = user.room.roomTitle;
        this.allowRequests = user.room.allowRequests;
        this.matureContent = user.room.matureContent;
        this.matureContentChat = user.room.matureContentChat;
        this.chatColor = user.chatColor;
        this.profileTitle = user.profileTitle;
        this.profileDescription = user.profileDescription;
        this.profileImageUrl = user.profileImageUrl;
        this.roomImageUrl = user.room.roomImageUrl;
        this.roomGenres = user.room.roomGenres;

        if (this.isUpdatingProfile) {
          this.router.navigateByUrl('/' + this.roomService.room.roomCode);

          this.toastrService.success('You have updated your Account information successfully', 'Update Success');
        }

        this.loading = false;
        this.isUpdatingProfile = false;
      });

      this.$purchasableItemsSubscription = this.hubService.getPurchasableItems().subscribe(purchasableItems => {
        let selectedItem: IPurchasableItem;

        switch (this._nextRequestType) {
          case NextRequestType.Playlist:
          // tslint:disable-next-line:max-line-length
          selectedItem = purchasableItems.filter(x => x.purchasableItemType === this.globalsService.getPurchasableItemType('Playlist') && x.quantity === 1)[0];
          break;

          case NextRequestType.PlaylistSongs:
          // tslint:disable-next-line:max-line-length
          selectedItem = purchasableItems.filter(x => x.purchasableItemType === this.globalsService.getPurchasableItemType('PlaylistSongs') && x.quantity === 15)[0];
          break;
        }

        if (selectedItem) {
          this.hubService.requestPurchasableItem(selectedItem.id);
        }
      });

      this.$purchasableItemSubscription = this.hubService.getPurchasableItem().subscribe(purchasableItem => {
        this.loading = false;
        const purchasableLineItem: IPurchasableLineItem = {
          purchasableItem: purchasableItem,
          orderQuantity: 1
        };
        this.addPurchasableItemToCart(purchasableLineItem);
      });
    }

  ngOnInit(): void {
    this.loading = true;
    this.roomService.requestApplicationUser();
    this.hubService.requestGenresRoom();
  }

  ngOnDestroy(): void {
    this.$applicationUserSubscription.unsubscribe();
    this.$purchasableItemsSubscription.unsubscribe();
    this.$purchasableItemSubscription.unsubscribe();
    this.$validatedTextReceivedSubscription.unsubscribe();
    this.$clearUserProfileImageSubscription.unsubscribe();
    this.$clearRoomImageSubscription.unsubscribe();
    this.$genreAccountSubscription.unsubscribe();
  }

  changeUsername(): void {
    // just make the input field editable and save the request
    this.disableAttr = true;
  }

  uploadProfileImage(): void {
    this._nextUploadType = NextUploadType.ProfileImage;
    this.file.nativeElement.click();
  }

  uploadRoomImage(): void {
    this._nextUploadType = NextUploadType.RoomImage;
    this.file.nativeElement.click();
  }

  onFilesAdded(): void {
    const file = this.file.nativeElement.files[0];

    if (file !== undefined) {
      if (file.size > 500000) {
        this.loading = false;
        this.toastrService.error('That picture is too large - the maximum size is 500 kb.', 'Image too large');
        return;
      }else {
        if (this._nextUploadType === NextUploadType.ProfileImage) {
          this.httpService.requestProfileImageUploaded(file)
          .subscribe(profile => {
            this.loading = false;
            if (profile !== undefined && profile !== null) {
              this.toastrService.success('You have successfully updated your Profile Image', 'Uploaded Success');

              this.loading = true;
              this.roomService.requestApplicationUser();
            }else {
              this.toastrService.error('Your upload was not successful, please try again.', 'Upload Failed');
            }
          });
        }else if (this._nextUploadType === NextUploadType.RoomImage) {
          this.httpService.requestRoomImageUploaded(file).subscribe(profile => {
            this.loading = false;
            if (profile !== undefined && profile !== null) {
              this.toastrService.success('You have successfully updated your room Image', 'Uploaded Success');

              this.loading = true;
              this.roomService.requestApplicationUser();
            }else {
              this.toastrService.error('Your upload was not successful, please try again.', 'Upload Failed');
            }
          });
        }
      }
    }
  }

  saveApplicationUserInfo(): void {
    if (this.roomCode === undefined || this.roomCode === null || this.roomCode === '') {
      this.toastrService.error('You have selected and invalid room code, please change the room code and try to save again', 'Invalid Room Code');

    }else if (this.roomTitle === undefined || this.roomTitle === null || this.roomTitle === '') {
      this.toastrService.error('You have entered and invalid room Title, please change the room title and try to save again', 'Invalid Room Title');

    }else if (this.isLocked && (this.roomKey === undefined || this.roomKey === null || this.roomKey === '')) {
      this.toastrService.error('You have selected to lock the room but have not provided a room Key, please enter room key', 'Invalid Room Key');

    }else {
      const confirmationModalData: IConfirmationModalData = {
        title: 'Are your sure ?',
        message: 'Are your sure you want to save the new Settings?',
        isConfirmed: false,
        confirmMessage: 'Confirm',
        cancelMessage: 'Cancel'
      };

      const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent, {
        width: '250px',
        data: confirmationModalData
      });

      confirmationModal.afterClosed().subscribe(result => {
        if (result !== undefined) {
          const validatedTextRequest: IValidatedTextRequest = {
            content: this.username,
            validatedTextType: this.globalsService.getValidatedTextType('Username')
          };
          this.hubService.requestValidatedText(validatedTextRequest);
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

  addPurchasableItemToCart(purchasableLineItem: IPurchasableLineItem): void {
    this.cartService.addPurchasableItemToCart(purchasableLineItem);

    this.router.navigate(['/', 'cart']);

    // tslint:disable-next-line:max-line-length
    this.toastrService.success('You have added a ' + purchasableLineItem.purchasableItem.purchasableItemName + ' to your cat', 'Add Success');
  }

  makeTheFormDirty(): void {
    this.accountForm.control.markAsTouched();
    this.accountForm.control.markAsDirty();
  }

  clearProfileImage(): void {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Clear Your Profile Image ?',
      message: 'Are you sure you would like to delete your profile image?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent,
      {
        width: '300px',
        data: confirmationModalData
      });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.requestClearProfileImage(this.hubService.applicationUser.id);
      }
    });
  }

  clearRoomImage(): void {
    const confirmationModalData: IConfirmationModalData = {
      title: 'Clear Your room Image ?',
      message: 'Are you sure you would like to delete your room image?',
      isConfirmed: false,
      confirmMessage: 'Confirm',
      cancelMessage: 'Cancel'
    };

    const confirmationModal = this.confirmationModal.open(ConfirmationmodalComponent,
      {
        width: '300px',
        data: confirmationModalData
      });

    confirmationModal.afterClosed().subscribe(result => {
      if (result !== undefined) {
        this.hubService.requestClearRoomImage(this.hubService.applicationUser.room.id);
      }
    });
  }

  addGenre(): void {
    if (this.genreSelectedId === undefined || this.genreSelectedId === null) {
      this.toastrService.error('You have not selected any genres, please choose one.', 'No Genre Selected');
    }else {
      let roomGenre = this.roomGenres.filter(x => x.genre.id === this.genreSelectedId)[0];
      if (roomGenre) {
        this.toastrService.error('Genre already selected for this room, you have choose different one.', 'Genre already selected');
      }else {
        if (this.roomGenres.length >= 5) {
          this.toastrService.warning('There are already 5 genres for your room', 'Max 5 Genres Per Room');
        }else {
          const genreSelected = this.genres.filter(x => x.id === this.genreSelectedId)[0];
          roomGenre = {
            genre : genreSelected
          };

          this.roomGenres.push(roomGenre);
          this.toastrService.success('Genre added to your room successfully', genreSelected.name + ' Added');
        }
      }
    }
  }

  removeGenre(id: string): void {
    // Confirmation modal to make sure to remove the genre
    const selectedRoomGenre = this.roomGenres.filter(x => x.genre.id === id)[0];

    if (selectedRoomGenre) {
      this.roomGenres.splice(this.roomGenres.indexOf(selectedRoomGenre), 1);
      this.toastrService.success(selectedRoomGenre.genre.name + ' has been deleted from the your room successfully', 'Genre Removed');
    }else {
      this.toastrService.error('There was an error while deleting the genre, please try again.', 'Could not remove genre');
    }
  }

  viewPastPurchases(): void {
    // ToDo: need to complete in both backend-frontend
  }
}

enum NextRequestType {
  Playlist,
  PlaylistSongs
}

enum NextUploadType {
  ProfileImage,
  RoomImage,
}
