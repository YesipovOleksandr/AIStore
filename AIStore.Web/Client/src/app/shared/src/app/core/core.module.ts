import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { AuthService } from "./services/auth.service";
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { CookieService } from './services/cookie.service';
import { ApiModule } from './modules/api/api.module'

@NgModule({
  declarations: [],
  imports: [
    BrowserModule,
    HttpClientModule,
    ApiModule
  ],
  providers: [
    AuthService,
    CookieService
  ]
})
export class CoreModule { }
