import { Injectable } from '@angular/core';
import { CanActivate, Router, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {
  constructor(private router: Router) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {

     const localData = localStorage.getItem('token');   // get token in locap storage
      if (localData) {
        const decodedToken: any = jwtDecode(localData);
        const role = decodedToken.role;   //get role from token
     
        if(role == 'Admin'){
            return true;
        }        
      } 
    // ROle isn't Accountant
    this.router.navigateByUrl('/home');
    return false;
  }
}
