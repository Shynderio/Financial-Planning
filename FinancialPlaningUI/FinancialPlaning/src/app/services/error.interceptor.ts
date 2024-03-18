import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { Router } from 'express';
import { jwtDecode } from 'jwt-decode';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req) .pipe(catchError((error) =>{
    if([403, 401].includes(error.status)){
      console.log('Unauthrized request ');
      const token = localStorage.getItem('token');
      if(token){
        window.location.href = '/home';
      }else{
        window.location.href = '/login';
      }
      
    }
    return throwError(() => error);
  }));
};
