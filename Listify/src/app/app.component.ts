import { GlobalsService } from './services/globals.service';
import { HubService } from './services/hub.service';
import { Subscription } from 'rxjs';
import { authConfig } from './authConfig';
import { OAuthService, OAuthEvent} from 'angular-oauth2-oidc';
import { JwksValidationHandler } from 'angular-oauth2-oidc-jwks';
import { Component, OnInit ,OnDestroy } from '@angular/core';
import { Router } from '@angular/router';
import { IApplicationUser } from './interfaces';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnDestroy, OnInit {
  title = 'Listify';
  claims: any;
  hasLoadedProfile: boolean;
  applicationUser: IApplicationUser = this.hubService.applicationUser;
  username = '';

  private _hasConnectedToHub: boolean;

  private $oauthSubscription: Subscription;
  private $disconnectSubscription: Subscription;
  private $pingSubscription: Subscription;
  private $applicationUserSubscription: Subscription;

  constructor(
    private oauthService: OAuthService,
    private router: Router,
    private globalsService: GlobalsService,
    private hubService: HubService) {
      this.$disconnectSubscription = this.hubService.getForceDisconnect().subscribe((data: string) => {
        // alert('Force disconnected');
        // if (data === 'Disconnect') {
        //   this.logout();
        // }
        this.hubService.disconnectFromHub();
        this.logout();
      });

      this.$pingSubscription = this.hubService.getPing().subscribe((ping: string) => {
        this.hubService.requestPing();
      });

      this.$applicationUserSubscription = this.hubService.getApplicationUser().subscribe(applicationUser => {
        this.username = applicationUser.username;
        this.applicationUser = applicationUser;
      });

      this.$oauthSubscription = this.oauthService.events.subscribe((event: OAuthEvent) => {

        this.oauthService.tryLogin({
          onTokenReceived: context => {
            if (!this.hasLoadedProfile) {
              this.hasLoadedProfile = true;
              this.oauthService.loadUserProfile();
            }
          }
        });
        const accessToken = this.oauthService.getAccessToken();
        if (accessToken !== undefined && accessToken !== null && accessToken !== '' && this.isAuthenticated && !this._hasConnectedToHub) {
          this._hasConnectedToHub = true;
          this.hubService.connectToHub(this.globalsService.developmentWebAPIUrl + 'listifyHub');
        }

        this.router.navigateByUrl('/');
        // if (event.type === 'token_received') {
        //   this.hubService.connectToHub(this.globalsService.developmentWebAPIUrl + 'listifyHub');
        // }
      });

      this.configureWithNewConfigApi();
      this.oauthService.postLogoutRedirectUri = this.globalsService.developmentClientSPAUrl;
  }

  ngOnInit(): void {
    const accessToken = this.oauthService.getAccessToken();

    if (accessToken !== undefined && accessToken !== null && this.isAuthenticated && accessToken !== '' && !this._hasConnectedToHub) {
      this._hasConnectedToHub = true;
      this.hubService.connectToHub(this.globalsService.developmentWebAPIUrl + 'listifyHub');
    }
    // if (this.claims !== null && this.claims !== undefined) {
    // }

  }

  ngOnDestroy(): void {
    this.$disconnectSubscription.unsubscribe();
    this.$oauthSubscription.unsubscribe();
    this.$pingSubscription.unsubscribe();
    this.$applicationUserSubscription.unsubscribe();
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
