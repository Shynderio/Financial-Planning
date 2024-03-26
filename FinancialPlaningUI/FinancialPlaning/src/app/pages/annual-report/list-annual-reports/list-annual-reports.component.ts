import { CommonModule } from '@angular/common';
import { Component, ViewChild } from '@angular/core';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { RouterLink } from '@angular/router';
import { AnnualReportService } from '../../../services/annual-report.service';

@Component({
  selector: 'app-list-annual-reports',
  standalone: true,
  imports: [
    RouterLink,
    MatPaginatorModule,
    CommonModule,
    MatTableModule,
  ],
  templateUrl: './list-annual-reports.component.html',
  styleUrl: './list-annual-reports.component.css'
})
export class ListAnnualReportsComponent {
  displayedColumns: string[] = ['Index','Year','TotalExpense','TotalDepartment','Create-Date'];
  dataSource: any = [];
  annualReports: any = [];
  searchValue: string = '';
   //paging
   listSize: number = 0;
   pageSize = 7;
   pageIndex = 0;

   constructor(
    private annualReportService:AnnualReportService
   ){
       this.dataSource = new MatTableDataSource<any>();
   }

   ngOnInit(): void {
      this.fetchData();
   }
   @ViewChild(MatPaginator) paginator!: MatPaginator;

   fetchData() {
    this.annualReportService.getListAnnualReport().subscribe((data: any) => {
      this.annualReports = data;
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
    let filteredList = this.annualReports;
    this.listSize = filteredList.length;

    return filteredList.slice(startIndex, startIndex + this.pageSize);
  }
}
