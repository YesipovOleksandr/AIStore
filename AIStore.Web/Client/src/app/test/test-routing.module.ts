import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

export const testRoutes: Routes = [
  { path: 'test/.', loadChildren: () => import('../test/modules/home/test.module').then(m => m.testModule) },
];


