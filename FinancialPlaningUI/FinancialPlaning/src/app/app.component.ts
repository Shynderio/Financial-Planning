import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
import { LoginComponent } from './pages/auth/login/login.component';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { SidenavComponent } from './share/sidenav/sidenav.component';
import { AuthService } from './services/auth/auth.service';
import { AdminSidenavComponent } from './share/admin-sidenav/admin-sidenav.component';

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
    CommonModule,
    AdminSidenavComponent,
  ],
})
export class AppComponent {
  title = 'Financial Plan System';
  logged = false;
  isAdmin = false;
  constructor(private authService: AuthService) {}
  ngOnInit(): void {
    this.isAdmin = this.authService.isAdmin();
    this.logged = this.authService.IsLoggedIn();
  }
}
