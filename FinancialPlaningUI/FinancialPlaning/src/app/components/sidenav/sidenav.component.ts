import { Component } from '@angular/core';
import { RouterLink, Routes } from '@angular/router';
import { HomeComponent } from '../../pages/home/home.component';
import { LogoutComponent } from '../../pages/logout/logout.component';
@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [HomeComponent, RouterLink,LogoutComponent],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.css'
})
export class SidenavComponent {

}
