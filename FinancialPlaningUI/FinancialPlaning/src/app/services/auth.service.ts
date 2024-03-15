import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginModel } from '../models/loginModel.model';
import { environment } from '../../environments/environment';
import { jwtDecode } from 'jwt-decode';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/Auth';
  constructor(
    private http: HttpClient,

  ) { }

  login(model: LoginModel): Observable<any> {
    return this.http.post(this.apiUrl + '/Login', model);
  }

  IsLoggedIn() {
    if (typeof localStorage !== 'undefined') {
      const token = localStorage.getItem('token');
      if (token) {
        const decodedToken: any = jwtDecode(token);
        const expirationTime = decodedToken.exp; 
        console.log(expirationTime);
        const currentTime = Math.floor(Date.now() / 1000); // Thời điểm hiện tại
        console.log(currentTime <= expirationTime)
        // if(currentTime > expirationTime){
        //   const token = localStorage.removeItem('token');
        //   window.location.href = '/login';
        // }
        return currentTime <= expirationTime;
    }
   
  }
  return false;
  }

  logout(): void {
    localStorage.removeItem('token');
    return;
  }

}
