import { Component } from '@angular/core';
import { RouterModule, RouterOutlet } from '@angular/router';
// import { HttpClientModule } from '@angular/common/http';
import { LoginComponent } from './pages/login/login.component';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { SidenavComponent } from "./components/sidenav/sidenav.component";
import { AuthService } from './services/auth.service';

@Component({
    selector: 'app-root',
    standalone: true,
    templateUrl: './app.component.html',
    styleUrl: './app.component.css',
    imports: [
        RouterOutlet,
        LoginComponent,
        CommonModule,
        ReactiveFormsModule,
        SidenavComponent,
        RouterModule,
        // HttpClientModule
    ]
})
export class AppComponent {
  title = 'FinancialPlaning';
  logged = false;
  // constructor(private authService: AuthService) {
  //   this.logged = this.authService.IsLoggedIn();
  // }
}
