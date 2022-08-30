import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

export const settingsPageRoutes: Routes = [
  { path: 'settings/.', loadChildren: () => import('../settingsPage/modules/home/settingsPage.module').then(m => m.settingsPageModule) },
];

