import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginModel } from '../models/loginModel.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  constructor(private http: HttpClient) { }
  
  login(model: LoginModel): Observable<any> {
    return this.http.post("https://localhost:7270/api/Auth/Login", model);
  }

  IsLoggedIn(){
    if (typeof localStorage !== 'undefined') {
      // Safe to use localStorage here
      // return false
      return localStorage.getItem('token')!=null;
    }
    return false
  }
}
