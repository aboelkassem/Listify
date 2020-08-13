import { AuthConfig } from 'angular-oauth2-oidc';

export const authConfig: AuthConfig = {
  issuer: 'http://localhost:5000',
  redirectUri: window.location.origin,
  clientId: 'listify.webapp',
  scope: 'profile openid roles listifyWebAPI',
  postLogoutRedirectUri: 'http://localhost:4200'
};
