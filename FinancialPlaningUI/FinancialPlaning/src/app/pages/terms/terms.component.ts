import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { SidenavComponent } from '../../components/sidenav/sidenav.component';
import { RouterLink } from '@angular/router';
import { HttpClient} from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { start } from 'repl';
import { NgFor } from '@angular/common';

@Component({
  selector: 'app-terms',
  standalone: true,
  templateUrl: './terms.component.html',
  styleUrl: './terms.component.css',
  imports: [SidenavComponent, RouterLink, MatPaginatorModule, MatPaginator, NgFor],
})
export class TermsComponent implements OnInit {
  termId: string = 'B0A9C7B9-9B1C-49FD-B4B3-ED769F72C7F2';

  httpClient = inject(HttpClient);
  termList: any = [];

  pageSize = 10;
  pageIndex = 0;

  ngOnInit(): void {
    this.fetchData();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  fetchData() {
    this.httpClient
      .get(environment.apiUrl + '/term/all')
      .subscribe((data: any) => {
        console.log(data);
        this.termList = data;
      });
    
    // this.httpClient.get(environment.apiUrl + '/term/' + '6E32ABB0-827C-4EAB-B51A-7A2A52E51438').subscribe({});
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
  }

  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    return this.termList;
  }
}
