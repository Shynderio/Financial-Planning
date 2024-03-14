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
<<<<<<< HEAD
    return this.http.post("http://localhost:5085/api/Auth/Login", model);
=======
    return this.http.post(this.apiUrl+'/Login', model);
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4
  }
  
  IsLoggedIn(){
    if (typeof localStorage !== 'undefined') {
      // Safe to use localStorage here
      // return false
      return localStorage.getItem('token')!=null;
    }
    return false
  }
<<<<<<< HEAD
  logout(): void{
    localStorage.removeItem('token');
   return;
}


=======

  logout(): void{
      localStorage.removeItem('token');
     return;
  }
  
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4
}
