import { HttpInterceptorFn } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';
import { catchError, throwError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  return next(req) .pipe(catchError((error) =>{
    if([403].includes(error.status)){
      console.log('Unauthrized request');
       window.location.href = '/login';
    }
    if([401].includes(error.status)){     
      console.log('Unauthrized request');
      const token = localStorage.getItem('token');
      if (token) {
        const decodedToken: any = jwtDecode(token);
        const expirationTime = decodedToken.exp;      
        const currentTime = Math.floor(Date.now() / 1000); // Thời điểm hiện tại
        console.log(currentTime <= expirationTime);
        if(currentTime >= expirationTime){
          alert("Login session expired. Please log in again!");
          localStorage.removeItem('token');
          window.location.href = '/login';
        }

    }
  }
    return throwError(() => error);
  }));
};
