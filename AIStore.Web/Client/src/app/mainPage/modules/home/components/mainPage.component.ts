import { Component, ViewEncapsulation } from '@angular/core';

@Component({
  selector: 'mainPage',
  templateUrl: './mainPage.component.html',
  styleUrls: ['./mainPage.component.scss'],
  encapsulation: ViewEncapsulation.None
})

export class MainPageComponent {
  title = 'mainPage';
}
