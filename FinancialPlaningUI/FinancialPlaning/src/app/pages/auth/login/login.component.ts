import { CommonModule } from '@angular/common';
import { Component, ElementRef } from '@angular/core';
import { AuthService } from '../../../services/auth/auth.service';
import { Router, RouterLink } from '@angular/router';
import {
  FormBuilder,
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  Validators,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MESSAGE_CONSTANTS } from '../../../../constants/message.constants';
import { MatProgressSpinnerModule } from '@angular/material/progress-spinner';

@Component({
  selector: 'app-login',
  standalone: true,
  imports: [
    FormsModule,
    CommonModule,
    ReactiveFormsModule,
    RouterLink,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    MatProgressSpinnerModule
  ],
  templateUrl: './login.component.html',
  styleUrl: './login.component.css',
})
export class LoginComponent {
  MESSAGE_CONSTANTS = MESSAGE_CONSTANTS;

  isLoading = false;

  loggedInUser: any; // Khai báo biến để lưu thông tin người dùng đã đăng nhập
  errorMessage = '';
  loginForm!: FormGroup;
  loginClicked = false;
  looged = false;

  constructor(
    private authService: AuthService,
    private router: Router,
    private formBuilder: FormBuilder,
    private elementRef: ElementRef
  ) {
    // localStorage.clear();
  }

  ngOnInit(): void {
    // check Islogged
    if (this.authService.IsLoggedIn()) {
      console.log(this.authService.IsLoggedIn());
      this.router.navigate(['/home']);
    }

    this.loginForm = this.formBuilder.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', Validators.required],
    });
  }

  //Login
  login(): void {
    if (this.loginForm.invalid) return;
    this.isLoading = true;
    this.authService.login(this.loginForm.value).subscribe({
      next: (response: any) => {
        console.log(response); // Log response to the console
        console.log('statuscode: ', response.statusCode); // Log response to the console
        //login ok
        if (response.statusCode == 200) {
          //Save token
          const token = response?.value?.token;
          localStorage.setItem('token', token);

          //Go to home page
          this.router.navigateByUrl('/home').then(() => {
            window.location.reload();
          });
        } else {
          this.errorMessage = MESSAGE_CONSTANTS.ME001;
          this.isLoading =
            false;
        }
      },
      error: (error) => {
        this.errorMessage = MESSAGE_CONSTANTS.ME000;
        this.isLoading = false;
        console.error(error); // Log error to the console
      },
    });
  }
}
