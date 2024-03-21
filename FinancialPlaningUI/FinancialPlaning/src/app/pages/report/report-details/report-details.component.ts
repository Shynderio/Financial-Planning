import { Component, Inject, ViewChild } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { Router } from 'express';
import { ActivatedRoute } from '@angular/router';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MAT_DIALOG_DATA, MatDialog, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { concatMap, of } from 'rxjs';

@Component({
  selector: 'app-report-details',
  standalone: true,
  imports: [  
    CommonModule,
    MatTableModule,
    MatSelectModule,
    MatPaginatorModule],
  templateUrl: './report-details.component.html',
  styleUrl: './report-details.component.css'
})
export class ReportDetailsComponent {

  displayedColumns: string[] = [
   'No','Expense','CostType','Unit Price (VND)','Amount',
   'Total','Project name','Supplier name',
   'PIC','Notes'
  ];

  dataSource: any = [];
  dataFile: any = [];
  report : any;
  reportVersions : any;
  uploadedBy : any;

   //paging
   listSize: number = 0;
   pageSize = 5;
   pageIndex = 0;

   constructor(
    private reportService:ReportService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private messageBar: MatSnackBar
    
    ){
      this.dataSource = new MatTableDataSource<any>();
    }

   ngOnInit(): void {
    this.route.params.subscribe(params => {
      const reportId = params['id']; // Assuming 'id' is the parameter name
      this.getReport(reportId);
    }
    );
   
   }
   @ViewChild(MatPaginator) paginator!: MatPaginator;
   getReport(reportId: string){
    this.reportService.getReport(reportId).subscribe((data: any) => {

      this.dataFile =data.expenses;
      this.report = data.report;
      this.reportVersions = data.reportVersions;  
      this.uploadedBy = data.uploadedBy;
     this.dataSource = this.getPaginatedItems();
     
      console.log(data);
      
    });
  }
  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    let filteredList = this.dataFile;
    this.listSize = filteredList.length;
    return filteredList.slice(startIndex, startIndex + this.pageSize);
  }
  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.dataSource = this.getPaginatedItems();
  }
  openReportVersionsDialog() {
    const reportVersionsDialog = this.dialog.open(ReportVersionsDialog, {
      width: '450px',
      height: '350px',
      data: this.reportVersions,
    });
    reportVersionsDialog
}
}


@Component({
  selector: 'reportVersions',
  standalone: true,
  templateUrl: '../reportVersions/reportVersions.component.html',
  styleUrls: ['../reportVersions/reportVersions.component.css'],
  imports: [MatDialogActions, MatDialogTitle, MatDialogContent ,MatTableModule],
})
export class ReportVersionsDialog {
  displayedColumns: string[] = ['Version','Published data','Changed by'];
  dataSource:any = [];
  constructor(
    public dialogRef: MatDialogRef<ReportVersionsDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.dataSource = new MatTableDataSource<any>(data);
  }

  closeDialog() {
    this.dialogRef.close();
  }
}
