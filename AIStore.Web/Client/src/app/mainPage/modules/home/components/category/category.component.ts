import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { AuthService } from 'src/app/shared/src/app/core/services/auth.service';
import { ApiService } from "src/app/shared/src/app/core/modules/api/services/api.service";

@Component({
  selector: 'category',
  templateUrl: './category.component.html',
  styleUrls: ['./category.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class CategoryComponent implements OnInit {

  public Categories: string[];
  public isLoading = false;

  constructor(private authService: AuthService,
  private apiService: ApiService) { }

  ngOnInit(): void {
    let accessToken = this.authService.getAuthData().access_token;
    this.apiService.get(window.clientConfig.environmentconfig.apiurl, "api/Category/", accessToken).subscribe(async next => {
      this.Categories = next.toString().split(",");
      this.isLoading = true;
    });
  }

  public LoadMore() {

  }
}
