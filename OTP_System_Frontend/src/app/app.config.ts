import { ApplicationConfig } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import {HashLocationStrategy, LocationStrategy} from "@angular/common";
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';

export const appConfig: ApplicationConfig = {
  providers: [provideRouter(routes),
    {provide: LocationStrategy, useClass: HashLocationStrategy}, provideAnimationsAsync(),
  ]
};
