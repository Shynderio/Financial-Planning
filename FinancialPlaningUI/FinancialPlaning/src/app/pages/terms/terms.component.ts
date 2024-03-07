import { Component } from '@angular/core';
import { HeaderComponent } from "../../components/header/header.component";
import { SidenavComponent } from "../../components/sidenav/sidenav.component";
import { RouterLink } from '@angular/router';

@Component({
    selector: 'app-terms',
    standalone: true,
    templateUrl: './terms.component.html',
    styleUrl: './terms.component.css',
    imports: [HeaderComponent, SidenavComponent, RouterLink]
})
export class TermsComponent {

}
