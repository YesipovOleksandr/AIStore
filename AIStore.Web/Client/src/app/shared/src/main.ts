/// <reference path="./app/globals/globals.d.ts" />

import 'core-js/modules/es.array.includes';
import 'core-js/modules/es.weak-set';
import 'zone.js/dist/zone';
import '../../../polyfills';
import 'svgxuse';

import { enableProdMode } from '@angular/core';
import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';

enableProdMode();

platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
