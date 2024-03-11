
import { LoginModel } from '../models/loginModel.model';
import { Injectable } from '@angular/core';
import { observableToBeFn } from 'rxjs/internal/testing/TestScheduler';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
 
    constructor(private http: HttpClient) {}
    login(model: LoginModel): Observable<any> {
            return this.http.post("https://localhost:7270/api/Auth/Login", model);
          }
    
  }