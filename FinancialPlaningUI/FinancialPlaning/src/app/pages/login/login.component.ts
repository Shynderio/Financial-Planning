import { CommonModule } from '@angular/common';
import { Component, ElementRef, Renderer2 } from '@angular/core';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginModel } from '../../models/loginModel.model';
import { AuthService } from '../../services/auth/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { FormGroup, FormBuilder } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [FormsModule, CommonModule, ReactiveFormsModule, RouterLink],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  loggedInUser: any; // Khai báo biến để lưu thông tin người dùng đã đăng nhập
  errorMessage = '';
  loginForm!: FormGroup;
  loginClicked = false;
  looged = false;

  constructor(
    private authService: AuthService,
    private http: HttpClient,
    private router: Router,
    private formBuilder: FormBuilder,
    private renderer: Renderer2,
    private el: ElementRef,
    private cookieService: CookieService
  ) {
    // localStorage.clear();
  }

  ngOnInit(): void {
    // check Islogged
    if(this.authService.IsLoggedIn()){
      console.log(this.authService.IsLoggedIn()); 
      this.router.navigate(['/home']);       
    }

    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });

    this.renderer.addClass(this.el.nativeElement, 'backgroundLogin');
  }

  //Login
  login(): void {
    if (this.loginForm.invalid) {
      this.errorMessage = '';
      this.loginClicked = true;
      return;
    }
    this.authService.login(this.loginForm.value).subscribe({
      next: (response: any) => {
        console.log(response); // Log response to the console
        console.log('statuscode: ', response.statusCode); // Log response to the console
        //login ok
        if (response.statusCode == 200) {
          //Save token
          const token = response?.value?.token;
          localStorage.setItem('token',token);
         
          //Go to home page
          this.router.navigateByUrl('/home').then(() => {
            window.location.reload();
          });
        } else {
          this.loginClicked = false;
          this.errorMessage =
            'Either email address or password is incorrect. Please try again';
          console.log(this.errorMessage);
        }
      },
      error: (error) => {
        console.error(error); // Log error to the console
      },
    });
  }
}
