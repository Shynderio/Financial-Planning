import {
  HttpClient,
  HttpErrorResponse,
  HttpResponse,
} from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { LoginModel } from '../../models/loginModel.model';
import { environment } from '../../../environments/environment';
import { jwtDecode } from 'jwt-decode';
import { response } from 'express';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = environment.apiUrl + '/Auth';
  constructor(private http: HttpClient) {}

  login(model: LoginModel): Observable<any> {
    return this.http.post(this.apiUrl + '/Login', model);
  }

  IsLoggedIn() {
    if (typeof localStorage !== 'undefined') {
      const token = localStorage.getItem('token');
      if (token) {
        const decodedToken: any = jwtDecode(token);
        const expirationTime = decodedToken.exp;
        const currentTime = Math.floor(Date.now() / 1000); // Thời điểm hiện tại
        if (currentTime > expirationTime) {
          const token = localStorage.removeItem('token');
          window.location.href = '/login';
        }
        return currentTime <= expirationTime;
      }
    }
    return false;
  }

  logout(): void {
    localStorage.removeItem('token');
    return;
  }

  forgotPassword(email: string): Observable<number> {
    let urlParams = new URLSearchParams();
    urlParams.append('email', email);
    return this.http
      .post(this.apiUrl + '/ForgotPassword?' + urlParams, null, {
        observe: 'response',
        responseType: 'text',
      })
      .pipe(
        map((response: HttpResponse<any>) => response.status),
        catchError((error: HttpErrorResponse) => {
          console.error('Error occurred:', error);
          throw error;
        })
      );
  }
}
