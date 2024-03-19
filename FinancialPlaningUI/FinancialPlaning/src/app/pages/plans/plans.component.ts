import { CommonModule } from '@angular/common';
import {
  Component,
  OnInit,
  ViewChild,
  ElementRef,
} from '@angular/core';
import { SidenavComponent } from '../../components/sidenav/sidenav.component';
import { RouterLink } from '@angular/router';
import {
  MatDialog,
  MatDialogRef,
  MatDialogActions,
  MatDialogClose,
  MatDialogTitle,
  MatDialogContent,
} from '@angular/material/dialog';
import { MatIconModule } from '@angular/material/icon';
import {
  MatPaginator,
  MatPaginatorModule,
  PageEvent,
} from '@angular/material/paginator';
import {
  MatSnackBar,
  MatSnackBarModule,
} from '@angular/material/snack-bar';
import { MatTableModule } from '@angular/material/table';
import { PlanService } from '../../services/plan.service';
import { jwtDecode } from 'jwt-decode';
import { of } from 'rxjs';
import { concatMap } from 'rxjs/operators';

@Component({
  selector: 'app-terms',
  standalone: true,
  templateUrl: './plans.component.html',
  styleUrl: './plans.component.css',
  imports: [
    CommonModule,
    SidenavComponent,
    RouterLink,
    MatPaginatorModule,
    MatIconModule,
    MatTableModule,
    MatSnackBarModule,
    MatPaginatorModule,
    MatIconModule,
    MatTableModule,
  ],
})
export class PlansComponent implements OnInit {
  planList: any = [];

  role: string = '';

  statusOption: string = 'All';
  searchValue: string = '';

  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;
  dataSource: any = [];

  columnHeaders: string[] = [
    'no',
    'plan',
    'term',
    'department',
    'status',
    'version',
    'action',
  ];

  constructor(
    private planService: PlanService,
    private elementRef: ElementRef,
    private dialog: MatDialog,
    private messageBar: MatSnackBar
  ) {}

  ngOnInit(): void {
    //Get role
    if (typeof localStorage !== 'undefined') {
      const token = localStorage.getItem('token') ?? '';
      if (token) {
        const decodedToken: any = jwtDecode(token);
        this.role = decodedToken.role;
        this.fetchData();
      }
    }
   
  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  fetchData() {
    this.planService.getFinancialPlans().subscribe((data: any) => {
      this.planList = data;
      this.dataSource = this.getPaginatedItems();
      console.log('Fetch data');
    });
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.dataSource = this.getPaginatedItems();
  }

  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    
    // return filteredList.slice(startIndex, startIndex + this.pageSize);
    this.listSize = this.planList.slice(startIndex, startIndex + this.pageSize).length;
    return this.planList.slice(startIndex, startIndex + this.pageSize);
  }

  changeSearchText(event: Event) {
    let target = event.target as HTMLInputElement;
    this.searchValue = target.value.trim();
    this.pageIndex = 0;
    this.dataSource = this.getPaginatedItems();
  }

  changeStatusFilter(event: Event) {
    //Toggle class 'chosen' of status filter button
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