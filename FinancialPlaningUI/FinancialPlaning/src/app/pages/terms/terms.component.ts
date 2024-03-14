import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { SidenavComponent } from '../../components/sidenav/sidenav.component';
import { RouterLink } from '@angular/router';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatTableModule } from '@angular/material/table';
import { TermService } from '../../services/term.service';

@Component({
  selector: 'app-terms',
  standalone: true,
  templateUrl: './terms.component.html',
  styleUrl: './terms.component.css',
  imports: [
    SidenavComponent,
    RouterLink,
    MatPaginatorModule,
    MatFormFieldModule,
    MatIconModule,
    ReactiveFormsModule,
    MatTableModule,
  ],
})
export class TermsComponent implements OnInit {
  termList: any = [];

  statusOption: string = 'All';
  searchValue: string = '';

  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;
  dataSource: any = [];

  columnHeaders: string[] = [
    'term',
    'startDate',
    'endDate',
    'status',
    'action',
  ];

  constructor(
    private fb: FormBuilder,
    private termService: TermService,
    private elementRef: ElementRef
  ) {}

  ngOnInit(): void {
    this.fetchData();
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  fetchData() {
    this.termService.getAllTerms().subscribe((data: any) => {
      console.log(data);
      this.termList = data;
      this.dataSource = this.getPaginatedItems();
    });
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
  }

  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    let filteredList = this.termList.filter(
      (data: any) =>
        data.termName.toLowerCase().includes(this.searchValue) &&
        (this.statusOption === 'All' || data.status === this.statusOption)
    );
    this.listSize = filteredList.length;
    return filteredList.slice(startIndex, startIndex + this.pageSize);
  }

  changeSearchText(event: Event) {
    let target = event.target as HTMLInputElement;
    this.searchValue = target.value.trim();
    this.pageIndex = 0;
    this.dataSource = this.getPaginatedItems();
  }

  changeStatusFilter(event: Event) {
    let target = event.target as HTMLElement;
    let statusOptions =
      this.elementRef.nativeElement.querySelector('#status-filter');

    for (let button of statusOptions.querySelectorAll('button')) {
      button.classList.remove('chosen');
    }
    target.classList.add('chosen');

    this.statusOption = target.innerHTML;
    this.pageIndex = 0;
    this.dataSource = this.getPaginatedItems();
  }
}
