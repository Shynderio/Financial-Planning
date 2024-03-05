import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './pages/login/login.component';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
<<<<<<< HEAD
import { FormsModule } from '@angular/forms';
=======
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
>>>>>>> UC01-Login

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    RouterOutlet,
    LoginComponent,
<<<<<<< HEAD
 
    CommonModule
=======
    CommonModule,
    ReactiveFormsModule
>>>>>>> UC01-Login
    ],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent {
  title = 'FinancialPlaning';
  
}
