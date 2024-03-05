import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { LoginModel } from '../../models/loginModel.model';
import { AuthService } from '../../services/auth.service';
import jwt_decode from 'jwt-decode';
import { HttpClient } from '@angular/common/http';
import { response } from 'express';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule,CommonModule],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
  
  loginModel: LoginModel = {email: '',password: ''};
  loggedInUser: any; // Khai báo biến để lưu thông tin người dùng đã đăng nhập

  constructor(private authService: AuthService,private http: HttpClient) {}
  login(): void {
    this.http.post("https://localhost:7270/api/Auth/Login",this.loginModel).subscribe({
      next: (response) => {
        console.log(response); // Log response to the console
    
      },
      error: (error) => {
        console.error(error); // Log error to the console
     
      }
    });
  }
  }

