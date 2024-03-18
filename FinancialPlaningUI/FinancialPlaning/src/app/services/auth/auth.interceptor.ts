import { HttpInterceptorFn } from '@angular/common/http';
import { jwtDecode } from 'jwt-decode';

import { catchError, throwError } from 'rxjs';

export const authInterceptor: HttpInterceptorFn = (request, next) => {
  
  if (typeof localStorage !== 'undefined') {
    const token = localStorage.getItem('token') ?? '';
    // if (token) {
    //   const decodedToken: any = jwtDecode(token);
    //   const expirationTime = decodedToken.exp;      
    //   const currentTime = Math.floor(Date.now() / 1000); // Thời điểm hiện tại
    //   console.log(currentTime <= expirationTime);
    //   if(currentTime >= expirationTime){
    //     alert("Login session expired. Please log in again!");
    //     localStorage.removeItem('token');
    //     window.location.href = '/login';
    //   }
    // }
    request = request.clone({
      setHeaders: {
        Authorization: token ? `Bearer ${token}` : '',
      },
    });
    
    console.log('Local storage is available');
  } else{
    console.log('Local storage is not available');
  }
  console.log("my message: ", request);
  return next(request);
 
};
