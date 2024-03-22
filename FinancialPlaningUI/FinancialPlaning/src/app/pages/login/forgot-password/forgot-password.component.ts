import { CommonModule } from '@angular/common';
import { Component, ElementRef, inject } from '@angular/core';
import {
  FormControl,
  FormGroupDirective,
  NgForm,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-forgot-password',
  standalone: true,
  templateUrl: './forgot-password.component.html',
  styleUrls: ['./forgot-password.component.css'],
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
export class ForgotPasswordComponent {
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  constructor(
    private authService: AuthService,
    private messageBar: MatSnackBar,
    private elementRef: ElementRef
  ) {}

  onSubmit() {
    if (this.emailFormControl.invalid) return;
    this.elementRef.nativeElement.querySelector('button').disabled = true;
    this.authService.forgotPassword(this.emailFormControl.value!).subscribe({
      next: (response) => {
        this.messageBar.open(
          "We've sent an email with the link to reset your password.",
          undefined,
          {
            duration: 500000,
            panelClass: ['messageBar', 'successMessage'],
            verticalPosition: 'top',
            horizontalPosition: 'end',
          }
        );
        this.elementRef.nativeElement.querySelector('button').disabled = false;
        console.log(response);
      },
      error: (error) => {
        this.messageBar.open(
          'The email address doesn’t exist. Please try again.',
          undefined,
          {
            duration: 5000,
            panelClass: ['messageBar', 'failMessage'],
            verticalPosition: 'top',
            horizontalPosition: 'end',
          }
        );
        this.elementRef.nativeElement.querySelector('button').disabled = false;
      },
    });
  }
}
