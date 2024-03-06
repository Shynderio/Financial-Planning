import { Component } from '@angular/core';
import { HeaderComponent } from "../../components/header/header.component";
import { SidenavComponent } from "../../components/sidenav/sidenav.component";

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
    imports: [HeaderComponent, SidenavComponent]
})
export class HomeComponent {
  title = 'FinancialPlaning';
  username = 'MINHNT70';
  userMenuMaxHeight = '0px';

  showUserMenu() {
    this.userMenuMaxHeight = this.userMenuMaxHeight === '0px' ? '200px' : '0px';
  }
}
