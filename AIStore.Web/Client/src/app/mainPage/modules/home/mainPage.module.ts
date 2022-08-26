import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HomeComponent } from 'src/app/mainPage/modules/home/components/home/home.component';
import { CategoryComponent } from 'src/app/mainPage/modules/home/components/category/category.component';
import { RouterModule, Routes } from '@angular/router';

const homeRoutes: Routes = [
  { path: '', component: HomeComponent }
];

@NgModule({
  imports: [
    CommonModule,
    RouterModule.forChild(homeRoutes),
  ],
  declarations: [
    HomeComponent,
    CategoryComponent,
  ],
  exports: [
    HomeComponent
  ]
})
export class mainPageModule { }
