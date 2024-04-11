import { withFetch, withInterceptors } from '@angular/common/http';
import { ApplicationConfig } from '@angular/core';
import { provideRouter, withComponentInputBinding } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient } from '@angular/common/http';
import { authInterceptor } from './services/auth/auth.interceptor';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { errorInterceptor } from './services/error.interceptor';
import { provideToastr } from 'ngx-toastr';
import { MAT_DATE_LOCALE } from '@angular/material/core';
import { HttpCoreInterceptor } from './http-core.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideHttpClient(withInterceptors([HttpCoreInterceptor])),
    provideHttpClient(withInterceptors([authInterceptor, errorInterceptor])),
    provideHttpClient(withFetch()),
    provideClientHydration(),
    provideRouter(routes, withComponentInputBinding()),
    provideAnimationsAsync(),
    provideToastr(),
    { provide: MAT_DATE_LOCALE, useValue: 'en-GB' },
  ],
};
