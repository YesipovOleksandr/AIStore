import { Component, Input, ViewEncapsulation } from '@angular/core';
import { ApiService } from "src/app/shared/src/app/core/modules/api/services/api.service";

@Component({
  selector: 'home-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class HomeComponent {
  private login: string;
  private code: string;
  private newPassword: string;
  private newPasswordConfirm: string;
  public isGetCode = false;
  public isSendCode = false;
  public isLoader = false;
  public errorMessage: string;
  constructor(private apiService: ApiService) { }

  public getCode() {
    this.login = (<HTMLInputElement>document.getElementById("login")).value;
    if (this.login != "") {
      this.clearErrorMessage();
      this.isLoader = true;
      this.apiService.get<any>(window.clientConfig.environmentconfig.apiurl, "api/Account/forgot-password?login=" + this.login).subscribe(res => {
        this.isLoader = false;
        this.isGetCode = true;
      }, error => {
        this.isLoader = false;
        this.showErrorCode(error.error);
      });
    }
  }

  public sendCode() {
    this.isLoader = true;
    this.code = (<HTMLInputElement>document.getElementById("code")).value;
    if (this.code != "") {
      this.isLoader = false;
      this.isLoader = true;
      this.apiService.get<any>(window.clientConfig.environmentconfig.apiurl, "api/Account/verify-recover-password-code?login=" + this.login + "&code=" + this.code).subscribe(res => {
        this.clearErrorMessage();
        this.isLoader = false;
        this.isSendCode = true;
      }, error => {
        this.isLoader = false;
        this.showErrorCode(error.error);
      });
    }
  }

  public updatePassword() {
    this.newPassword = (<HTMLInputElement>document.getElementById("newPassword")).value;
    this.newPasswordConfirm = (<HTMLInputElement>document.getElementById("newPasswordConfirm")).value;
    if (this.newPassword == this.newPasswordConfirm) {

      const headers = {}; 
      let json = {};
      json["login"] = this.login;
      json["code"] = this.code;
      json["newPassword"] = this.newPassword;

      this.isLoader = true;
      this.apiService.post<any>(window.clientConfig.environmentconfig.apiurl, "api/Account/reset-password", json, true, headers, false).subscribe(res => {
        this.clearErrorMessage();
        this.isLoader = false;
        window.location.href = window.clientConfig.environmentconfig.weburl;
      }, error => {
        var responceCode;

        if (error.error?.code) {
          responceCode = error.error.code;
        }
        this.showErrorCode(responceCode);
      });
    } else {
      console.log("error");
    }
  }

  public showErrorCode(error: string) {
    this.errorMessage = error;
  }

  public clearErrorMessage() {
    this.errorMessage = '';
  }
}
