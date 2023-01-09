import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

export const forgotPasswordPageRoutes: Routes = [
  { path: 'forgot-password/.', loadChildren: () => import('../forgotPasswordPage/modules/home/forgotPasswordPage.module').then(m => m.forgotPasswordPage) },
];


