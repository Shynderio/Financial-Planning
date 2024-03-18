import { Component, ElementRef, ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';

import { MatDialog } from '@angular/material/dialog';
import { FormBuilder, ReactiveFormsModule } from '@angular/forms';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { jwtDecode } from 'jwt-decode';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { RouterLink } from '@angular/router';

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
  dataSource: MatTableDataSource<Report>;
  reports: any = [];
 

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
        console.log('aa');
        this.fetchData();
      }
    }
  }

  // getListReport(): void {
  //   this.reportService.getListReport().subscribe({
  //     next: (response: any) => {
  //       console.log(response); // Log response to the console
  //        this.dataSource = response;
  //     }}
  //   );
  // }

  @ViewChild(MatPaginator) paginator!: MatPaginator;
  //Get report
  fetchData() {
    this.reportService.getListReport().subscribe((data: any) => {
      this.reports = data;
      this.dataSource = data;
      console.log(data);
    });
  }


}
