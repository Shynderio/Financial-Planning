import { HttpInterceptorFn } from '@angular/common/http';
 
export const authInterceptor: HttpInterceptorFn = (request, next) => {
  // const localStorage = document.defaultView?.localStorage;
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
 
  return next(request);
};
