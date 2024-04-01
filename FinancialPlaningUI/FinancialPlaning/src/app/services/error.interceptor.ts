import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from 'express';
import { jwtDecode } from 'jwt-decode';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req) .pipe(catchError((error) =>{
    console.log(error.status);

    if([404,500].includes(error.status)){
      window.history.back(); 
    }

    if([403].includes(error.status)){
      console.log('Unauthrized request ');
      const token = localStorage.getItem('token');
      if(token){
        window.location.href = '/home';
      }else{
        window.location.href = '/login';
      }
    }

    if([401].includes(error.status)){
      localStorage.removeItem('token');
      window.location.href = '/login';
    }
    return throwError(() => error);
  }));
};
