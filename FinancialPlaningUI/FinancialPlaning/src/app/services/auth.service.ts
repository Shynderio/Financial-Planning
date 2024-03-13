import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { LoginModel } from '../models/loginModel.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private apiUrl = environment.apiUrl+'/Auth';
  constructor(private http: HttpClient) { }
  
  login(model: LoginModel): Observable<any> {
    return this.http.post(this.apiUrl+'/Login', model);
  }
  
  IsLoggedIn(){
    if (typeof localStorage !== 'undefined') {
      // Safe to use localStorage here
      // return false
      return localStorage.getItem('token')!=null;
    }
    return false
  }

  logout(): void{
      localStorage.removeItem('token');
     return;
  }
  
}
