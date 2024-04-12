import { HttpInterceptorFn } from '@angular/common/http';

export const HttpCoreInterceptor: HttpInterceptorFn = (req, next) => {
  req = req.clone({
    setHeaders: {
      'Content-Type': 'application/json',
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods': 'GET, POST, PUT, DELETE, OPTIONS',
      'Access-Control-Allow-Headers': 'Origin, Content-Type, X-Auth-Token',
      'Access-Control-Allow-Credentials': 'true',
    },
  });
  return next(req);
};
