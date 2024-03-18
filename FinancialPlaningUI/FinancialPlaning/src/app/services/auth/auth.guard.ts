import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Check if localStorage is defined
    var result = true;

    if (typeof localStorage !== 'undefined') {
      const localData = localStorage.getItem('token');
      if (localData) {
        const decodedToken: any = jwtDecode(localData);
        const expirationTime = decodedToken.exp;  // get exp time
        const currentTime = Math.floor(Date.now() / 1000); // Get time now

        console.log(currentTime <= expirationTime);
        // check token is exp
        if (currentTime > expirationTime) {
          localStorage.removeItem('token');
       window.location.href='/login';
          return false;
        } else {
          return true;
        }

      }
    }

    this.router.navigateByUrl('/login');
    return false;
  }
}
