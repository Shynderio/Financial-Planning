import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { error } from 'console';
import { CookieService } from 'ngx-cookie-service';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  
  if (typeof localStorage !== 'undefined') {
    const token = localStorage.getItem('token') ?? '';
    request = request.clone({
      setHeaders: {
        Authorization: token ? `Bearer ${token}` : '',
      },
    });
  }
  console.log("my message: ", request);
  return next(request)
  .pipe(catchError((error) =>{
    if([401, 403].includes(error.statusCode)){
      console.log('Unauthrized request');
      window.location.href = '/login';
    }
    return throwError(() => error);
  }));
};
