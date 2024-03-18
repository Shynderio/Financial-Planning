import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AccountanGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Check if localStorage is defined
    if (typeof localStorage !== 'undefined') {
      const localData = localStorage.getItem('token');
      if (localData) {
        const decodedToken: any = jwtDecode(localData);
        const role = decodedToken.role;
        console.log('role')
        if(role == 'Accountant'){
            return true;
        }
        
      }
    }
    // If localStorage is not available or token is not found, redirect to login page
    this.router.navigateByUrl('/home');
    return false;
  }
}
