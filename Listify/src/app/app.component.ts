import { authConfig } from './authConfig';
import { OAuthService} from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { Component } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'Listify';
  claims: any;
  hasLoadedProfile: boolean;

  constructor(private oauthService: OAuthService, private router: Router) {
    this.configureWithNewConfigApi();
    this.oauthService.postLogoutRedirectUri = 'http://localhost:4200';
  }

  private configureWithNewConfigApi(): void {
    this.oauthService.configure(authConfig);
    this.oauthService.tokenValidationHandler = new JwksValidationHandler();
    this.oauthService.loadDiscoveryDocumentAndTryLogin();
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
