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
import { TermService } from '../../services/term.service';
import { jwtDecode } from 'jwt-decode';
import { of } from 'rxjs';
import { concatMap } from 'rxjs/operators';

@Component({
  selector: 'app-terms',
  standalone: true,
  templateUrl: './terms.component.html',
  styleUrl: './terms.component.css',
  imports: [
    CommonModule,
    SidenavComponent,
    RouterLink,
    MatPaginatorModule,
    MatIconModule,
    MatTableModule,
    MatSnackBarModule
  ],
})
export class TermsComponent implements OnInit {
  termList: any = [];

  role: string = '';

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
    private termService: TermService,
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
    this.termService.getAllTerms().subscribe((data: any) => {
      this.termList = data;
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

  openDeleteDialog(id: string) {
    const deleteDialog = this.dialog.open(DeleteTermDialog, {
      width: '400px',
      height: '250px',
    });

    deleteDialog
      .afterClosed()
      .pipe(
        concatMap((result) => {
          if (result === 'delete') {
            return this.termService.deleteTerm(id);
          } else {
            return of(null);
          }
        })
      )
      .subscribe((response) => {
        this.messageBar.open(response == 200 ? 'Deleted successfully' : 'Something went wrong', 'Close', {
          
          panelClass: ['success'],
        });
        this.pageIndex = 0;
        this.fetchData();
      });
  }
}

@Component({
  selector: 'delete-term',
  standalone: true,
  templateUrl: './delete-term/delete-term.component.html',
  styleUrls: ['./delete-term/delete-term.component.css'],
  imports: [MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
})
export class DeleteTermDialog {
  constructor(public dialogRef: MatDialogRef<DeleteTermDialog>) {}
}
