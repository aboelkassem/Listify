import { OAuthService } from 'angular-oauth2-oidc';
import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {

  claims: any;
  hasLoadedProfile: boolean;

  constructor(private oauthService: OAuthService) { }

  ngOnInit(): void {
  }

  login(): void {
    this.oauthService.initImplicitFlow();
  }

  logout(): void {
    this.oauthService.logOut();
  }

  get isAuthenticated(): boolean {
    this.claims = this.oauthService.getIdentityClaims();
    if (this.claims !== undefined && this.claims != null) {

      if (!this.hasLoadedProfile) {
        this.hasLoadedProfile = true;
        this.oauthService.loadUserProfile();
      }

      return true;
    }

    return false;
  }

}
