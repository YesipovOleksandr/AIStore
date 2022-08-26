import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss'],
  encapsulation: ViewEncapsulation.None
})
export class AppComponent {
  constructor() {
  }

  ngOnInit(): void {
    var mainSection = document.querySelector('.main-section') as HTMLDivElement;

    if (mainSection) {
      mainSection.style.display = "none";
      var appRoot = document.getElementsByTagName('app-root')[0] as HTMLDivElement;
      appRoot.style.display = "";
    }
  }
}
