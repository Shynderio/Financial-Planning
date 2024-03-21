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
    'No', 'Expense', 'CostType', 'Unit Price (VND)', 'Amount',
    'Total', 'Project name', 'Supplier name',
    'PIC', 'Notes'
  ];

  dataSource: any = [];
  dataFile: any = [];
  report: any;
  reportVersions: any;
  uploadedBy: any;

  totalExpense: number = 0;
  biggestExpenditure: number = 0;


  //paging
  listSize: number = 0;
  pageSize = 5;
  pageIndex = 0;

  constructor(
    private reportService: ReportService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
  ) {
    this.dataSource = new MatTableDataSource<any>();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const reportId = params['id']; // Assuming 'id' is the parameter name
      this.getReport(reportId);

    });
  }
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  getReport(reportId: string) {
    this.reportService.getReport(reportId).subscribe((data: any) => {

      //List expenses
      this.dataFile = data.expenses;
      //data of report
      this.report = data.report;
      this.reportVersions = data.reportVersions;
      //Name of account uploaded
      this.uploadedBy = data.uploadedBy;
      //filter
      this.dataSource = this.getPaginatedItems();

      // Caculate totalExpense and biggestExpenditure
      this.biggestExpenditure = Math.max(...this.dataFile.map((element: any) => element.unitPrice * element.amount));
      this.totalExpense = this.dataFile.reduce((total: any, element: any) => total + (element.totalAmount), 0);

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

  convertIsoDateToDdMmYyyy(isoDate: string): string {
  if (!isoDate) return '';
  const dateParts = isoDate.split('T')[0].split('-');
  if (dateParts.length !== 3) return isoDate; // Trả về nguyên bản nếu không phải định dạng ISO 8601
  return `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;
}

  openReportVersionsDialog() {
    const reportVersionsDialog = this.dialog.open(ReportVersionsDialog, {
      width: '500px',
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
  imports: [MatDialogActions, MatDialogTitle, MatDialogContent, MatTableModule],
})
export class ReportVersionsDialog {
  displayedColumns: string[] = ['Version', 'Published data', 'Changed by'];
  dataSource: any = [];
  currentVersion: any;
  isFirstRow: boolean = true;
  constructor(
    public reportService: ReportService,
    public dialogRef: MatDialogRef<ReportVersionsDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.dataSource = new MatTableDataSource<any>(data);
    this.currentVersion = data.currentVersion; 
  }

  closeDialog() {
    this.dialogRef.close();
  }
  getVersionLabel(element: any): string {
    if (this.isFirstRow) {
      this.isFirstRow = false;
      return 'currentVersion ' + element.version;
    } else {
      return 'v.' + element.version;
    }
  }
  convertIsoDateToDdMmYyyy(isoDate: string): string {
    if (!isoDate) return '';
    const dateParts = isoDate.split('T')[0].split('-');
    if (dateParts.length !== 3) return isoDate; // Trả về nguyên bản nếu không phải định dạng ISO 8601
    return `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;
  }

  downloadFile(reportId: string, version: number) {
    this.reportService.exportSinglereport(reportId, version).subscribe((data: any) => {
        const downloadUrl = data.downloadUrl;
         // Tạo một link ẩn để download file
         const link = document.createElement('a');
         link.href = downloadUrl;
         link.setAttribute('download', '');

         // Thêm link vào trang web và click vào nó để bắt đầu download
         document.body.appendChild(link);
         link.click();

         // Xóa link sau khi download hoàn tất
         document.body.removeChild(link)
      }, 
    );
  }
}
