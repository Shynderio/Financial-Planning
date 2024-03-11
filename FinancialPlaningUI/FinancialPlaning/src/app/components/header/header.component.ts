import { Component } from '@angular/core';

@Component({
  selector: 'app-header',
  standalone: true,
  imports: [],
  templateUrl: './header.component.html',
  styleUrl: './header.component.css'
})
export class HeaderComponent {
  username = 'Bạn';
  userMenuMaxHeight = '0px';

  showUserMenu() {
    this.userMenuMaxHeight = this.userMenuMaxHeight === '0px' ? '200px' : '0px';
  }
}
