import { Component, ElementRef, ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { jwtDecode } from 'jwt-decode';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';
import { MatSelectModule } from '@angular/material/select';

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

  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;

  constructor(
    private reportService: ReportService,
    private fb: FormBuilder,
    private elementRef: ElementRef,
    private dialog: MatDialog) {
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
   this.getQuaters();

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
    const selectedQuarterId = event.value;
    console.log('Selected quarter ID:', selectedQuarterId);
    this.dataSource = this.getPaginatedItems();

  }
  //Select status
  onStatusSelected(event: any): void {
   this.selectstatus = event.value;
    
    this.dataSource = this.getPaginatedItems();
  }
 //Get list quarter
 getQuaters(){
  const currentDate = new Date();
 const currentYear = currentDate.getFullYear();
 const currentQuarter = Math.floor((currentDate.getMonth() / 3)) + 1;

 // Lặp từ năm trước đến năm tiếp theo và từ quý 1 đến quý 4
 for (let year = currentYear - 2; year <= currentYear + 1; year++) {
   for (let quarter = 1; quarter <= 4; quarter++) {
     // Thêm vào mảng
     this.quarters.push({ id: `${year}-Q${quarter}`, name: `Q${quarter} ${year}` });
   }
 }
 }
 

}
