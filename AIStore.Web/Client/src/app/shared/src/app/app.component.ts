import { Component, ViewEncapsulation } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  public isShowFooter: boolean;
  constructor(public router: Router) {
    this.isShowFooter = true;
  }

  ngOnInit(): void {
    var mainSection = document.querySelector('.main-section .functional, [js-functional]') as HTMLDivElement;

    if (mainSection) {
      mainSection.style.display = "none";
      var appRoot = document.getElementsByTagName('app-root')[0] as HTMLDivElement;
      appRoot.style.display = "";
    }
  }

  ngAfterContentInit(): void {
    var useragent = window.navigator.userAgent.toLowerCase();

    if (useragent.indexOf('mac') != -1) {
      document.getElementsByTagName("html")[0].classList.add("mac-device");
    }
  }
}
