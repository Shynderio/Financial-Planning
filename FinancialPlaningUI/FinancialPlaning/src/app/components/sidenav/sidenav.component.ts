import { CommonModule } from '@angular/common';
import { Component, ElementRef, Renderer2, model } from '@angular/core';
import { FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { LoginModel } from '../../models/loginModel.model';
import { AuthService } from '../../services/auth.service';
// import { HttpClient } from '@angular/common/http';
import { Router, RouterLink } from '@angular/router';
import { FormGroup, FormControl,FormBuilder,AbstractControl } from '@angular/forms';
import { HomeComponent } from '../../pages/home/home.component';


@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [HomeComponent, RouterLink],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.css'
})
export class SidenavComponent {

  constructor(
    private authService: AuthService, 
    // private http: HttpClient,
    private router: Router, 
  ) { 
  }
  logout() {
    if (confirm('Are you sure you want to logout?')) {
    this.authService.logout();
    this.router.navigateByUrl('/login');
    }
  }
  ngOnInit(): void {}
}
