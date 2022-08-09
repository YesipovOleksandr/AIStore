import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

export const mainPageRoutes: Routes = [
  { path: '', loadChildren: () => import('../mainPage/modules/home/mainPage.module').then(m => m.mainPageModule) },
];


