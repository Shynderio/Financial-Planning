import { HttpClient, HttpClientModule, withFetch, withInterceptors } from '@angular/common/http';
import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';
import { routes } from './app.routes';
import { provideClientHydration } from '@angular/platform-browser';
import { provideHttpClient} from '@angular/common/http';
import { authInterceptor } from './services/auth.interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    // importProvidersFrom(HttpClientModule),
    provideHttpClient(withFetch()), 
    provideClientHydration(),
    provideHttpClient(withInterceptors([authInterceptor])),
    provideRouter(routes),  
  ],
    
};
