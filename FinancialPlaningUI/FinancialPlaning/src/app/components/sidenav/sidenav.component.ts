import { Component } from '@angular/core';
import { RouterLink, Routes } from '@angular/router';
import { HomeComponent } from '../../pages/home/home.component';
@Component({
  selector: 'app-sidenav',
  standalone: true,
  imports: [HomeComponent, RouterLink],
  templateUrl: './sidenav.component.html',
  styleUrl: './sidenav.component.css'
})
export class SidenavComponent {

}
