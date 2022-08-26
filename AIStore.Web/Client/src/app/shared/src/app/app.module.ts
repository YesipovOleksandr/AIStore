import { BrowserModule } from '@angular/platform-browser';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { Location } from '@angular/common';
import { NgModule } from '@angular/core';
import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { mainPageModule } from '../../../mainPage/modules/home/mainPage.module';
import { CoreModule } from './core/core.module';

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    BrowserAnimationsModule,
    CoreModule,
    AppRoutingModule,
    mainPageModule
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }

const __stripTrailingSlash = (Location as any).stripTrailingSlash;
Location.stripTrailingSlash = function (url) {
  if (url.endsWith('/') || url.indexOf('?') >= 0) {
    url = url;
  }
  else {
    url = url + '/';
  }
  const queryString$ = url.match(/([^?]*)?(.*)/);
  if (queryString$[2].length > 0) {
    return /[^\/]\/$/.test(queryString$[1]) ? queryString$[1] + '.' + queryString$[2] : __stripTrailingSlash(url);
  }
  return /[^\/]\/$/.test(url) ? url + '.' : __stripTrailingSlash(url);
};
