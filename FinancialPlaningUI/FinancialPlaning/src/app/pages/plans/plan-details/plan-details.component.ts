import { Component, Inject, ViewChild } from '@angular/core';
import { PlanService } from '../../../services/plan.service';
import { ActivatedRoute } from '@angular/router';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MAT_DIALOG_DATA, MatDialog, MatDialogActions, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';


@Component({
  selector: 'app-plan-details',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatSelectModule,
    MatPaginatorModule],
  templateUrl: './plan-details.component.html',
  styleUrl: './plan-details.component.css'
})
export class PlanDetailsComponent {

  displayedColumns: string[] = [
    'No', 'Expense', 'CostType', 'Unit Price (VND)', 'Amount', 'Currency', 'Exchange rate',
    'Total', 'Project name', 'Supplier name',
    'PIC', 'Notes', 'Expense Status'
  ];

  dataSource: any = [];
  dataFile: any = [];
  plan: any;
  planVersions: any;
  uploadedBy: any;
  planDueDate: any;


  totalExpense: number = 0;
  biggestExpenditure: number = 0;


  //paging
  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;

  constructor(
    private planService: PlanService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
  ) {
    this.dataSource = new MatTableDataSource<any>();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const planId = params['id']; // Assuming 'id' is the parameter name
      this.getplan(planId);

    });
  }
  @ViewChild(MatPaginator) paginator!: MatPaginator;
  getplan(planId: string) {
    this.planService.getPlan(planId).subscribe((data: any) => {

      //List expenses
      this.dataFile = data.expenses;
      //data of plan
      this.plan = data.plan;
      this.planVersions = data.planVersions;
      //Name of account uploaded
      this.uploadedBy = data.uploadedBy;
      this.planDueDate= data.planDueDate;
      //filter
      this.dataSource = this.getPaginatedItems();

      // Caculate totalExpense and biggestExpenditure
      this.biggestExpenditure = Math.max(...this.dataFile.map((element: any) => element.unitPrice * element.amount));
      this.totalExpense = this.dataFile.reduce((total: any, element: any) => total + (element.totalAmount), 0);

    });
  }
  //filter page
  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    let filteredList = this.dataFile;
    this.listSize = filteredList.length;
    return filteredList.slice(startIndex, startIndex + this.pageSize);
  }
  //paging
  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.dataSource = this.getPaginatedItems();
  }

 //Convert date to dd/mm/yyyy
 convertIsoDateToDdMmYyyy(isoDate: string): string {
  if (!isoDate) return '';
  const dateParts = isoDate.split('T')[0].split('-');
  if (dateParts.length !== 3) return isoDate; // Trả về nguyên bản nếu không phải định dạng ISO 8601
  return `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;
}
 //open dialog plan history
  openPlanVersionsDialog() {
    const planVersionsDialog = this.dialog.open(PlanVersionsDialog, {
      width: '500px',
      height: '350px',
      data: this.planVersions,
 
    });
    PlanVersionsDialog
  }
}

@Component({
  selector: 'planVersions',
  standalone: true,
  templateUrl: '../planVersions/planVersions.component.html',
  styleUrls: ['../planVersions/planVersions.component.css'],
  imports: [MatDialogActions, MatDialogTitle, MatDialogContent, MatTableModule],
})
export class PlanVersionsDialog {
  
  displayedColumns: string[] = ['Version', 'Published data', 'Changed by'];
  dataSource: any = [];
  currentVersion: any;
  isFirstRow: boolean = true;

  constructor(
    public planService: PlanService,
    public dialogRef: MatDialogRef<PlanVersionsDialog>,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.dataSource = new MatTableDataSource<any>(data);
    this.currentVersion = data.currentVersion; 
  }

  closeDialog() {
    this.dialogRef.close();
  }
  //current vesion 
  getVersionLabel(element: any): string {
    if (this.isFirstRow) {
      this.isFirstRow = false;
      return 'currentVersion ' + element.version;
    } else {
      return 'v.' + element.version;
    }
  }
  //Convert date to dd/mm/yyyy
  convertIsoDateToDdMmYyyy(isoDate: string): string {
    if (!isoDate) return '';
    const dateParts = isoDate.split('T')[0].split('-');
    if (dateParts.length !== 3) return isoDate; // Trả về nguyên bản nếu không phải định dạng ISO 8601
    return `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;
  }  

}
