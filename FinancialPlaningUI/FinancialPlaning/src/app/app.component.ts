import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { LoginComponent } from './pages/login/login.component';
import { ReactiveFormsModule } from '@angular/forms';
import { SidenavComponent } from './components/sidenav/sidenav.component';
import { AuthService } from './services/auth.service';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-root',
  standalone: true,
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  imports: [
    RouterOutlet,
    LoginComponent,
    ReactiveFormsModule,
    SidenavComponent,
    RouterModule,
    CommonModule
  ],
})
export class AppComponent {
  title = 'FinancialPlaning';
  logged = false;
  constructor(private authService: AuthService) {}
  ngOnInit(): void {
    this.logged = this.authService.IsLoggedIn();
  }
}
