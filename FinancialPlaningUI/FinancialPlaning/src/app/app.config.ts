import { HTTP_INTERCEPTORS, HttpClient, HttpClientModule, withFetch, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient} from '@angular/common/http';
import { authInterceptor } from './services/auth.interceptor';
import { errorInterceptor } from './services/error.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    // importProvidersFrom(HttpClientModule),
   
    provideHttpClient(withInterceptors([authInterceptor,errorInterceptor])),
    provideHttpClient(withFetch()), 
    provideClientHydration(),
    provideRouter(routes),  
  ],
    
};
