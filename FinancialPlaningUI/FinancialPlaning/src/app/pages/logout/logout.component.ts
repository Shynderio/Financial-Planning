import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';


@Component({
  selector: 'app-logout',
  standalone: true,
  imports: [],
  templateUrl: './logout.component.html',
  styleUrl: './logout.component.css'
})
export class LogoutComponent {

  constructor(
    private authService: AuthService, 
    private http: HttpClient,
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