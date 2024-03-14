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
<<<<<<< HEAD
       
=======
        // HttpClientModule
>>>>>>> a1e43e29ea81e0482a78b5d3ca8a230de947bed4
    ]
})
export class AppComponent {
  title = 'FinancialPlaning';
 logged = false;
  constructor(private authService: AuthService) {
   
  }
  ngOnInit(): void {
    this.logged = this.authService.IsLoggedIn();
  }

  

}
