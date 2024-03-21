import { Component, ElementRef, ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatDialog, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { jwtDecode } from 'jwt-decode';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';
import { MatSnackBar } from '@angular/material/snack-bar';
import { concatMap } from 'rxjs/operators';
import { of } from 'rxjs';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-list-report',
  standalone: true,
  templateUrl: './list-report.component.html',
  styleUrl: './list-report.component.css',
  imports: [
    RouterLink,
    MatPaginatorModule,
    MatFormFieldModule,
    MatIconModule,
    CommonModule,
    ReactiveFormsModule,
    MatTableModule,
    MatSelectModule
  ],
})
export class ListReportComponent {
  displayedColumns: string[] = [
    'index',
    'reportName',
    'month',
    'termName',
    'departmentName',
    'status',
    'version',
    'action'];
   
  role: string = '';
  departmentName: string = '';
  dataSource: any = [];
  reports: any = [];

  searchValue: string = '';

  terms: any = [];
  selectedTerm = "All";

  departments: any = [];
  selectedDepartment = "All";

  selectstatus = "All";
  
  quarters: any[] = [];
  selectedQuarter = "All";

  //paging
  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;

  constructor(
    private reportService: ReportService,
    private fb: FormBuilder,
    private elementRef: ElementRef,
    private dialog: MatDialog,
    private messageBar: MatSnackBar) {

    this.dataSource = new MatTableDataSource<Report>();
  }

  ngOnInit(): void {
    if (typeof localStorage !== 'undefined') {
      const token = localStorage.getItem('token') ?? '';
      if (token) {
        const decodedToken: any = jwtDecode(token);
        this.role = decodedToken.role;
        this.departmentName = decodedToken.departmentName;
        console.log(this.departmentName);
        this.fetchData();
      }
    }

    this.getQuarters();

  }

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  //Get report
  fetchData() {
    this.reportService.getListReport().subscribe((data: any) => {
      this.reports = data.reports;
      this.terms = data.terms;
      this.departments = data.departments;

      this.dataSource = this.getPaginatedItems();
      console.log(data);
    });
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.dataSource = this.getPaginatedItems();
  }

  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    let filteredList = this.reports.filter(
      (data: any) =>
        data.reportName.toLowerCase().includes(this.searchValue)
        && (this.selectedDepartment == data.departmentName || this.selectedDepartment == "All")
        && (this.selectedTerm == data.termName || this.selectedTerm == "All")
        && (this.selectstatus == data.status || this.selectstatus == "All")
    );
    //get quarter and year from value select value exp: Q2 2023
    if (this.selectedQuarter != "All") {
      const selectedQuarter = this.selectedQuarter.split(' ');
      const selectedQuarterNumber = parseInt(selectedQuarter[0].substring(1));
      const selectedQuarterYear = parseInt(selectedQuarter[1]);

    //filter list by quater , year 
      filteredList = filteredList.filter((report: any) => {
        const [monthName, year] = report.month.split(' ');
        //parse month from string to number
        const monthIndex = new Date(Date.parse(`${monthName} 1, ${year}`)).getMonth() + 1;
        const quarterNumber = Math.ceil(monthIndex / 3);
       
        return quarterNumber === selectedQuarterNumber && parseInt(year) === selectedQuarterYear;
      });
    }

    this.listSize = filteredList.length;

    return filteredList.slice(startIndex, startIndex + this.pageSize);
  }

  changeSearchText(event: Event) {
    let target = event.target as HTMLInputElement;
    this.searchValue = target.value.trim();
    this.pageIndex = 0;
    this.dataSource = this.getPaginatedItems();
  }

  //Select Department
  onDepartmentSelected(event: any): void {
    console.log(event.value);
    this.selectedDepartment = event.value;
    this.dataSource = this.getPaginatedItems();
  }

  //Select Term
  onTermSelected(event: any): void {
    console.log(event.value);
    this.selectedTerm = event.value;
    this.dataSource = this.getPaginatedItems();
  }

  //Select Quater
  onQuarterSelected(event: any): void {
    this.selectedQuarter = event.value;
    console.log('Selected quarter ID:', this.selectedQuarter);
    this.dataSource = this.getPaginatedItems();

  }
  //Select status
  onStatusSelected(event: any): void {
    this.selectstatus = event.value;
    this.dataSource = this.getPaginatedItems();
  }

  //Get list quarter
  getQuarters() {
    const currentDate = new Date();
    const currentYear = currentDate.getFullYear();

    // Lặp từ năm trước đến năm tiếp theo và từ quý 1 đến quý 4
    for (let year = currentYear - 2; year <= currentYear + 1; year++) {
      for (let quarter = 1; quarter <= 4; quarter++) {
        // Thêm vào mảng
        this.quarters.push(`Q${quarter} ${year}`);
      }
    }
  }
  //


  openDeleteDialog(id: string) {
    const deleteDialog = this.dialog.open(DeleteReportDialog, {
      width: '400px',
      height: '250px',
    });

    deleteDialog
      .afterClosed()
      .pipe(
        concatMap((result) => {
          if (result === 'delete') {
            return this.reportService.deleteReport(id);
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
  selector: 'delete-report',
  standalone: true,
  templateUrl: '../delete-report/delete-report.component.html',
  styleUrls: ['../delete-report/delete-report.component.css'],
  imports: [MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
})
export class DeleteReportDialog {
  constructor(public dialogRef: MatDialogRef<DeleteReportDialog>) {}
}

