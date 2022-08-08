import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainPageComponent } from 'src/app/mainPage/modules/home/components/mainPage.component';
import { RouterModule, Routes } from '@angular/router';

const homeRoutes: Routes = [
  { path: '', component: MainPageComponent }
];

@NgModule({
  imports: [
    RouterModule.forChild(homeRoutes),
  ],
  declarations: [
    MainPageComponent,
  ],
  exports: [
    MainPageComponent
  ]
})
export class mainPageModule { }
