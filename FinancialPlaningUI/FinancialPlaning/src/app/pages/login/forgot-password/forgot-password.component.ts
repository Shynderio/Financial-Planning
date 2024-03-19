import { CommonModule } from '@angular/common';
import { Component, ElementRef } from '@angular/core';
import {
  FormControl,
  FormGroupDirective,
  NgForm,
  Validators,
  FormsModule,
  ReactiveFormsModule,
} from '@angular/forms';
import { ErrorStateMatcher } from '@angular/material/core';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { RouterLink } from '@angular/router';
import { AuthService } from '../../../services/auth/auth.service';
import { MatSnackBar } from '@angular/material/snack-bar';

export class EmailErrorStateMatcher implements ErrorStateMatcher {
  isErrorState(
    control: FormControl | null,
    form: FormGroupDirective | NgForm | null
  ): boolean {
    const isSmmubitted = form && form.submitted;
    return !!(
      control &&
      control.invalid &&
      (control.dirty || control.touched || isSmmubitted)
    );
  }
}

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
    RouterLink
  ],
})
export class ForgotPasswordComponent {
  emailFormControl = new FormControl('', [
    Validators.required,
    Validators.email,
  ]);

  matcher = new EmailErrorStateMatcher();

  constructor(
    private authService: AuthService,
    private messageBar: MatSnackBar,
    private elementRef: ElementRef,
  ) {}

  onSubmit() {
    this.elementRef.nativeElement.querySelector('button').disabled = true;
    if (this.emailFormControl.value === null) {
      return;
    }
    this.authService
      .forgotPassword(this.emailFormControl.value!)
      .subscribe(response => {
        if (response == 200) {
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
        } else {
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
        }
        this.elementRef.nativeElement.querySelector('button').disabled = false;
        console.log(response);
      });
  }
}
