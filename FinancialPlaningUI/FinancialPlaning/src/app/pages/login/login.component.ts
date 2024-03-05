import { CommonModule } from '@angular/common';
<<<<<<< HEAD
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
=======
import { Component, model } from '@angular/core';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
>>>>>>> UC01-Login
import { LoginModel } from '../../models/loginModel.model';
import { AuthService } from '../../services/auth.service';
import jwt_decode from 'jwt-decode';
import { HttpClient } from '@angular/common/http';
import { response } from 'express';
<<<<<<< HEAD
=======
import { Router } from '@angular/router';
import { FormGroup, FormControl } from '@angular/forms';



>>>>>>> UC01-Login

@Component({
  selector: 'app-login',
  standalone: true,
<<<<<<< HEAD
  imports: [FormsModule,CommonModule],
=======
  imports: [FormsModule, CommonModule,ReactiveFormsModule],
>>>>>>> UC01-Login
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent {
<<<<<<< HEAD
  
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
=======

  loginModel: LoginModel = { email: '', password: '' };
  loggedInUser: any; // Khai báo biến để lưu thông tin người dùng đã đăng nhập
  errorMessage ='';
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email
  ]);
  constructor(
    private authService: AuthService,
    private http: HttpClient,
    private router: Router) { }
   

  login(): void {
    if (this.emailFormControl.hasError('email')) {
      this.errorMessage = 'Please enter a valid email address';
      console.log(this.errorMessage);
      return;
    }
    this.authService.login(this.loginModel).subscribe({
      next: (response: any) => {
        console.log(response); // Log response to the console
        const message = response?.value?.message;

        //login ok 
        if (response.statusCode == 200) {
          const token = response?.value?.token;
          localStorage.setItem('token', token)
          this.router.navigate(['/home']);

        }else{
          console.log(message);
          this.errorMessage = message;
        }

      },
      error: (error) => {
        console.error(error); // Log error to the console

      }
    });
  }
}
>>>>>>> UC01-Login

