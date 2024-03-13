import { Component, OnInit, inject, ViewChild } from '@angular/core';
import { SidenavComponent } from '../../components/sidenav/sidenav.component';
import { RouterLink } from '@angular/router';
import { HttpClient} from '@angular/common/http';
import { environment } from '../../../environments/environment';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';

@Component({
  selector: 'app-terms',
  standalone: true,
  templateUrl: './terms.component.html',
  styleUrl: './terms.component.css',
  imports: [
    SidenavComponent,
    RouterLink,
    // HttpClientModule,
    MatPaginatorModule,
    MatFormFieldModule,
    MatIconModule,
    ReactiveFormsModule,
    MatTableModule,
  ],
})
export class TermsComponent implements OnInit {
  httpClient = inject(HttpClient);
  termList: any = [];

  pageSize = 10;
  pageIndex = 0;

  searchValue = '';
  searchForm = this.fb.nonNullable.group({
    searchValue: '',
  });

  columnHeaders: string[] = [
    'term',
    'startDate',
    'endDate',
    'status',
    'action',
  ];

  constructor(private fb: FormBuilder) {}

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
    return this.termList.slice(startIndex, startIndex + this.pageSize);
  }

  onSearchSubmit() {
    this.searchValue = this.searchForm.value.searchValue ?? '';
  }
}
