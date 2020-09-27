import { IProfile } from './../interfaces';
import { Subscription } from 'rxjs';
import { HubService } from './../services/hub.service';
import { ActivatedRoute } from '@angular/router';
import { Component, OnInit, OnDestroy } from '@angular/core';

@Component({
  selector: 'app-profile',
  templateUrl: './profile.component.html',
  styleUrls: ['./profile.component.css']
})
export class ProfileComponent implements OnInit, OnDestroy {

  profile: IProfile;
  username: string;
  dateJoined: Date;

  $profileReceivedSubscription: Subscription;

  constructor(private route: ActivatedRoute, private hubService: HubService) {
    this.route.params.subscribe(params => {
      const id = params['id']; // + params converts id to numbers
      if (id != null) {
        this.hubService.requestProfile(id);
      }
    });

    this.$profileReceivedSubscription = this.hubService.getProfile().subscribe(profile => {
      this.profile = profile;
      this.username = profile.username;
      this.dateJoined = profile.dateJoined;
    });
  }

  ngOnInit(): void {
  }

  ngOnDestroy(): void {
    this.$profileReceivedSubscription.unsubscribe();
  }
}
