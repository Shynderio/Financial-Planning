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
termId: string = 'B0A9C7B9-9B1C-49FD-B4B3-ED769F72C7F2';
// id: any|string;

}
