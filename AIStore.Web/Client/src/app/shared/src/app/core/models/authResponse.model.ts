

export class AuthResponseModel {
  public access_token: string;
  public email: string;
  public lastName: string;
  public refresh_token: string;
  public token_type: string;
  public updateData(model: AuthResponseModel) {
    this.access_token = model.access_token || this.access_token;
    this.email = model.email || this.email;
    this.lastName = model.lastName || this.lastName;
    this.refresh_token = model.refresh_token || this.refresh_token;
  }
}
