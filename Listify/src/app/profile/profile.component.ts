import { ProfileService } from './../services/profile.service';
import { MatDialog } from '@angular/material/dialog';
import { IProfile, IPlaylist } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';
import { ApplicationusersfollowingmodalComponent } from '../shared/modals/applicationusersfollowingmodal/applicationusersfollowingmodal.component';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

  loading = false;
  profile: IProfile;

  username: string;
  profileImageUrl: string;
  profileTitle: string;
  profileDescription: string;
  dateJoined: Date;
  roomImageUrl: string;
  roomCode: string;
  roomTitle: string;
  roomId: string;
  playlists: IPlaylist[] = [];
  numberOfFollows: number;
  id: string;

  $profileReceivedSubscription: Subscription;

  constructor(
    private route: ActivatedRoute,
    private matDialog: MatDialog,
    private profileService: ProfileService,
    private hubService: HubService) {
    this.route.params.subscribe(params => {
      this.id = params['id']; // + params converts id to numbers
      if (this.id !== null && this.id !== undefined) {
        this.loading = true;
        this.hubService.requestProfile(this.id);
      }
    });

    this.$profileReceivedSubscription = this.hubService.getProfile().subscribe(profile => {
      this.loading = false;
      this.profile = profile;
      this.username = profile.username;
      this.dateJoined = profile.dateJoined;
      this.profileImageUrl = profile.profileImageUrl;
      this.profileTitle = profile.profileTitle;
      this.profileDescription = profile.profileDescription;
      this.playlists = profile.playlists;
      this.roomCode = profile.room.roomCode;
      this.roomTitle = profile.room.roomTitle;
      this.roomImageUrl = profile.room.roomImageUrl;
      this.numberOfFollows = profile.numberFollows;

      this.profileService.setApplicationUserId(this.profile.id);
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.$profileReceivedSubscription.unsubscribe();
  }

  viewProfileFollows(): void {
    this.matDialog.open(ApplicationusersfollowingmodalComponent, {
      width: '400px',
      height: '500px',
      data: this.profile.room.follows
    });
  }
}
