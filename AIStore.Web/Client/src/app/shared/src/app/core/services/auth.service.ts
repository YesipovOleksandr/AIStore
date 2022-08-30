import { Injectable, EventEmitter } from '@angular/core';
import { AuthResponseModel } from "../models/authResponse.model";
import { UserToken } from '../models/user.token';
import { CookieService } from './cookie.service';

@Injectable()
export class AuthService {
  private readonly AuthenticatedUserCookieKey = 'auth_user';

  constructor( private cookieService: CookieService) { }

  //public setAuthResponseData(resp: AuthResponseModel) {
  //  if (resp && resp.access_token) {
  //    var isSecure = location.protocol === 'https:';
  //    var response = new AuthResponseModel();
  //    response.updateData(resp);
  //    this.cookieService.remove(this.AuthenticatedUserCookieKey);
  //    this.cookieService.set(this.AuthenticatedUserCookieKey, JSON.stringify(response), 1, isSecure);
  //  }
  //}

  //public clearAuthCookies() {
  //  this.cookieService.remove(this.AuthenticatedUserCookieKey);
  //}

  public isAuthorized(): boolean {
    var userToken = this.getAuthUser();
    return userToken != null;
  }

  public getAuthUser(): UserToken {

    var userToken: UserToken = null;

    var user;

    try {
      user = JSON.parse(this.cookieService.get(this.AuthenticatedUserCookieKey)) as AuthResponseModel;
    } catch (e) {}

    if (user && (user.access_token || user.refresh_token)) {
      userToken = new UserToken();
      userToken.accessToken = user.access_token;
      userToken.refreshToken = user.refresh_token;
      userToken.expires = user.expires;
    }

    return userToken;
  }

  public getIdUser(): number {

    var user;

    try {
      user = JSON.parse(this.cookieService.get(this.AuthenticatedUserCookieKey)) as AuthResponseModel;
    } catch (e) { }

    var id: number;
    if (user && user.id) {
      id = user.id;
    }

    return id || 0;
  }

  public getUserEmail(): string {

    var user;

    try {
      user = JSON.parse(this.cookieService.get(this.AuthenticatedUserCookieKey)) as AuthResponseModel;
    } catch (e) {}

    var email: string;
    if (user && user.email) {
       email = user.email;
    }

    return email || '';
  }

  public getAuthData(): AuthResponseModel {

    let authData: AuthResponseModel = new AuthResponseModel();

    try {
      authData.updateData(JSON.parse(this.cookieService.get(this.AuthenticatedUserCookieKey)) as AuthResponseModel);
    } catch (e) {
      authData = null;
    }

    return authData;
  }
}
