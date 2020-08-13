import { Injectable } from '@angular/core';
import { OAuthService } from 'angular-oauth2-oidc';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthguardService implements CanActivate {

  constructor(private oauthService: OAuthService, private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

    if (this.oauthService.hasValidAccessToken()) {
      return true;
    }

    this.router.navigate(['/']);
    return false;
  }
}
