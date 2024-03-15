import { HttpInterceptorFn } from '@angular/common/http';
<<<<<<< HEAD
import { inject } from '@angular/core';
import { error } from 'console';
import { CookieService } from 'ngx-cookie-service';
import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  
=======
 
export const authInterceptor: HttpInterceptorFn = (request, next) => {
  // const localStorage = document.defaultView?.localStorage;
>>>>>>> f6ed1ee746cbca924198a3fdd8a4bec1bb82a9ae
  if (typeof localStorage !== 'undefined') {
    const token = localStorage.getItem('token') ?? '';
    request = request.clone({
      setHeaders: {
        Authorization: token ? `Bearer ${token}` : '',
      },
    });
    console.log('Local storage is available');
  } else{
    console.log('Local storage is not available');
  }
<<<<<<< HEAD
  console.log("my message: ", request);
  return next(request)
  .pipe(catchError((error) =>{
    if([401, 403].includes(error.statusCode)){
      console.log('Unauthrized request');
      window.location.href = '/login';
    }
    return throwError(() => error);
  }));
=======
 
  return next(request);
>>>>>>> f6ed1ee746cbca924198a3fdd8a4bec1bb82a9ae
};
