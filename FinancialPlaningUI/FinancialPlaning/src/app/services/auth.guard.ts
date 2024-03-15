import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private router: Router) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    // Check if localStorage is defined
    if (typeof localStorage !== 'undefined') {
      const localData = localStorage.getItem('token');
      if (localData !== null) {
        return true;
      }
    }

    // If localStorage is not available or token is not found, redirect to login page
    this.router.navigateByUrl('/login');
    return false;
  }
}
