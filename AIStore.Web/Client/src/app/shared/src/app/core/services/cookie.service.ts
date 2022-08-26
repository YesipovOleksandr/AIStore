import { Injectable } from '@angular/core';
import * as Cookies from 'js-cookie';

@Injectable()
export class CookieService {

  private readonly cookie: any;

  constructor() {

    this.cookie = Cookies.withConverter({
      write: function (value) {
        return encodeURIComponent(value);
      },
      read: function (value) {
        return decodeURIComponent(value);
      }
    });
  }

  public set(key: string, value: string, expires: number | Date, secure = false) {
    this.cookie.set(key, value, { expires: expires, secure: secure });
  }

  public get(key: string): string {
    var value = this.cookie.get(key) as string;

    return value;
  }

  public remove(key: string): void {
    this.cookie.remove(key);
  }
}
