

export class AuthResponseModel {
  public id: number;
  public role: string;
  public access_token: string;
  public email: string;
  public lastName: string;
  public refresh_token: string;
  public token_type: string;
  public expires: string;
  public updateData(model: AuthResponseModel) {
    this.id = model.id || this.id;
    this.access_token = model.access_token || this.access_token;
    this.expires = model.expires || this.expires;
    this.role = model.role || this.role;
    this.email = model.email || this.email;
    this.lastName = model.lastName || this.lastName;
    this.role = model.role || this.role;
    this.refresh_token = model.refresh_token || this.refresh_token;
  }
}
