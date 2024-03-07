import { Component } from '@angular/core';

@Component({
    selector: 'app-home',
    standalone: true,
    templateUrl: './home.component.html',
    styleUrl: './home.component.css',
})
export class HomeComponent {
  title = 'FinancialPlaning';
  username = 'MINHNT70';
  userMenuMaxHeight = '0px';

  // showUserMenu() {
  //   this.userMenuMaxHeight = this.userMenuMaxHeight === '0px' ? '200px' : '0px';
  // }
}
