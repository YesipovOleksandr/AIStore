import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { mainPageRoutes } from 'src/app/mainPage/mainPage-routing.module';


var routes = [];

(function () {

  var routesToConcat = [mainPageRoutes];

  for (var moduleIndex = 0; moduleIndex < routesToConcat.length; moduleIndex++) {
    for (var routeIndex = 0; routeIndex < routesToConcat[moduleIndex].length; routeIndex++) {
      var currentModule = routesToConcat[moduleIndex];
      routes.push(currentModule[routeIndex]);
    }
  }

}());

@NgModule({
  imports: [RouterModule.forRoot(routes, { useHash: false, initialNavigation: 'enabled' })],
  exports: [RouterModule],
  providers: [
  ]
})

export class AppRoutingModule { }
