import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ApiService } from "../api/services/api.service";


@NgModule({
  declarations: [],
  imports: [
    CommonModule
  ],
  providers: [
    ApiService,
  ]
})
export class OAuthModule { }
