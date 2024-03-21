import { CommonModule } from '@angular/common';
import { Component, ElementRef, Input, inject } from '@angular/core';
import {
  FormControl,
  FormsModule,
  ReactiveFormsModule,
  FormGroup,
  Validators,
  ValidatorFn,
  AbstractControl,
  ValidationErrors,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { Router, RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { delay } from 'rxjs';

export function passwordStrengthValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const value = control.value;

    if (!value) {
      return null;
    }

    const hasUpperCase = /[A-Z]+/.test(value);

    const hasLowerCase = /[a-z]+/.test(value);

    const hasNumeric = /[0-9]+/.test(value);

    const passwordValid = (hasUpperCase || hasLowerCase) && hasNumeric;

    return !passwordValid ? { weakPassword: true } : null;
  };
}

export function passwordMatchValidator(): ValidatorFn {
  return (control: AbstractControl): ValidationErrors | null => {
    const password = control.parent?.get('password');
    const confirmPassword = control.parent?.get('confirmPassword');

    return password &&
      confirmPassword &&
      password.value !== confirmPassword.value
      ? { passwordMismatch: true }
      : null;
  };
}

@Component({
  selector: 'app-reset-password',
  standalone: true,
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.css'],
  imports: [
    CommonModule,
    FormsModule,
    MatCardModule,
    MatButtonModule,
    MatFormFieldModule,
    MatInputModule,
    ReactiveFormsModule,
    RouterLink,
  ],
})
export class ResetPasswordComponent {
  @Input() token: string = '';

  resetPasswordForm = new FormGroup({
    password: new FormControl(null, [
      Validators.required,
      Validators.minLength(7),
      passwordStrengthValidator(),
    ]),
    confirmPassword: new FormControl(null, [
      Validators.required,
      passwordMatchValidator(),
    ]),
  });

  constructor(
    private authService: AuthService,
    private messageBar: MatSnackBar,
    private elementRef: ElementRef,
    private router: Router
  ) {}

  ngOnInit(): void {}

  onSubmit() {
    if (!this.resetPasswordForm.valid) return;
    this.elementRef.nativeElement.querySelector('button').disabled = true;
    this.authService
      .resetPassword(this.resetPasswordForm.value.password!, this.token)
      .subscribe({
        next: (response) => {
          this.elementRef.nativeElement.querySelector('button').disabled =
            false;
          if (response == 200) {
            this.messageBar.open('Password reset successfully', '', {
              duration: 5000,
              panelClass: ['messageBar', 'successMessage'],
              verticalPosition: 'top',
              horizontalPosition: 'end',
            });
            delay(2000);
            this.router.navigate(['/login']);
          } else {
            this.messageBar.open(
              'This link has expired. Please go back to Homepage and try again.',
              '',
              {
                duration: 5000,
                panelClass: ['messageBar', 'failMessage'],
                verticalPosition: 'top',
                horizontalPosition: 'end',
              }
            );
            delay(2000);
            this.router.navigate(['/login']);
          }
        },
        error: (error) => {
          this.elementRef.nativeElement.querySelector('button').disabled =
            false;
          this.messageBar.open(
            'Something went wrong. Please try again later.',
            '',
            {
              duration: 5000,
              panelClass: ['messageBar', 'failMessage'],
              verticalPosition: 'top',
              horizontalPosition: 'end',
            }
          );
        },
      });
  }
}
