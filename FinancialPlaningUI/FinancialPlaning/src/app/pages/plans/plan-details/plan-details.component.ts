import { Component, Inject, ViewChild } from '@angular/core';
import { PlanService } from '../../../services/plan.service';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { MatPaginator, MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MAT_DIALOG_DATA, MatDialog, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogRef, MatDialogTitle } from '@angular/material/dialog';
import { MatSnackBar } from '@angular/material/snack-bar';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { concatMap, of } from 'rxjs';
import { Location } from '@angular/common';
import { Plan } from '../../../models/planviewlist.model';
import { jwtDecode } from 'jwt-decode';

@Component({
  selector: 'app-plan-details',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatSelectModule,
    MatCheckboxModule,
    RouterLink,
    MatPaginatorModule
  ],
  templateUrl: './plan-details.component.html',
  styleUrl: './plan-details.component.css'
})
export class PlanDetailsComponent {
  displayedColumns: string[] = [
    'Checkbox', 'No', 'Expense', 'CostType', 'Unit Price (VND)', 'Amount', 'Currency', 'Exchange rate',
    'Total', 'Project name', 'Supplier name',
    'PIC', 'Notes', 'Expense Status'
  ];

  dataSource: any = [];
  dataFile: any = [];
  plan: any;
  planVersions: any;
  uploadedBy: any;
  planDueDate: any;
  date: any;
  status: any;
  role: string = '';  

  isPlanNew: boolean = false;
  isPlanApproved: boolean = false;
  isApprove: boolean= false;
  approvedExpenses: number[] = [];
  isSubmitting: boolean = false;
  showCheckbox: boolean = false;



  totalExpense: number = 0;
  biggestExpenditure: number = 0;


  //paging
  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;
  router: any;

  constructor(
    private planService: PlanService,
    private route: ActivatedRoute,
    private dialog: MatDialog,
    private messageBar: MatSnackBar,
    private location: Location
  ) {
    this.dataSource = new MatTableDataSource<any>();
  }

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const planId = params['id']; // Assuming 'id' is the parameter name
      this.getplan(planId);
        //Get role
        if (typeof localStorage !== 'undefined') {
          const token = localStorage.getItem('token') ?? '';
          if (token) {
            const decodedToken: any = jwtDecode(token);
            this.role = decodedToken.role;
          }
        }
       
        
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
      this.date = data.date;
      //Name of account uploaded
      this.uploadedBy = data.uploadedBy;
      this.planDueDate = data.planDueDate;

      //filter
      this.dataSource = this.getPaginatedItems();

      // status Expense
      this.isApprove= (this.plan.status === 'WaitingForApproval' && this.role !== 'FinancialStaff' );
      console.log(this.isApprove);


      this.isPlanNew = this.plan.status === 'New';
      this.isPlanApproved = this.plan.plan.status === 'Approved';
      this.approvedExpenses = this.plan.approvedExpenses ? JSON.parse(this.plan.approvedExpenses) : [];
      this.showCheckbox = !(this.plan.status === 'New')  && !(this.plan.status === 'Approved');

      // Caculate totalExpense and biggestExpenditure
      this.biggestExpenditure = Math.max(...this.dataFile.map((element: any) => element.unitPrice * element.amount));
      this.totalExpense = this.dataFile.reduce((total: any, element: any) => total + (element.totalAmount), 0);
      // Load dữ liệu về các ô đã chọn từ ApprovedExpense
      this.dataFile.forEach((expense: any) => {
        // Kiểm tra xem expense có trong mảng approvedExpenses không
        if (this.approvedExpenses.includes(expense.no)) {
          // Nếu có, đặt thuộc tính checked thành true và cập nhật trạng thái sang "Approved"
          expense.checked = true;
          expense.status = "Approved";
        }
      });


    });
  }

  toggleCheckbox(expenseId: number): void {
    if (!this.isPlanNew && !this.isPlanApproved) {
      const index = this.approvedExpenses.indexOf(expenseId);
      if (index === -1) {
        // Add expenseId to approvedExpenses array if not already present
        this.approvedExpenses.push(expenseId);
      } else {
        // Remove expenseId from approvedExpenses array if already present
        this.approvedExpenses.splice(index, 1);
      }
    }

  }


  isExpenseApproved(expenseId: number): boolean {
    return this.approvedExpenses.includes(expenseId);
  }
  getExpenseStatus(expenseId: number): string {
    if (this.isPlanNew || this.isPlanApproved) {
      return this.isPlanNew ? 'New' : 'Approved';
    } else {
      return this.isExpenseApproved(expenseId) ? 'Approved' : 'Waiting for Approval';
    }
  }
  getStatus(status: string): string {
    if (status === 'WaitingForApproval')
      return 'Waiting for Approval';
    else return status;
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
    PlanVersionsDialog;
  }
  downloadFile(reportId: string, version: number) {
    this.planService.exportPlan(reportId, version).subscribe((data: any) => {
      const downloadUrl = data.downloadUrl;
      // create hidden link to download
      const link = document.createElement('a');
      link.href = downloadUrl;
      link.setAttribute('download', '');

      // Add link into web and click it to download
      document.body.appendChild(link);
      link.click();

      // remove link after download 
      document.body.removeChild(link);
    }
    );
  }


  openSubmitDialog(id: string) {
    const SubmitDialog = this.dialog.open(SubmitPlanDialog, {
      width: '400px',
      height: '250px',
    });

    SubmitDialog
      .afterClosed()
      .pipe(
        concatMap((result) => {
          if (result === 'Submit') {
            this.status=1;
            return this.planService.submitPlan(id, this.status );
          } else {
            return of(null);
          }
        })
      )
      .subscribe((response) => {
        // Check if response is null, if yes, it means user cancelled, so don't open any message bar
        if (response !== null && response === 200) {
          this.messageBar.open('Change status successfully', 'Close', {
            duration: 1000,
            panelClass: ['success'],
          });
          window.location.reload();
        }
      });
  }

  openApproveDialog(id: string) {
    const SubmitDialog = this.dialog.open(SubmitPlanDialog, {
      width: '400px',
      height: '250px',
    });

    SubmitDialog
      .afterClosed()
      .pipe(
        concatMap((result) => {
          if (result === 'Submit') {
          this.approvedExpenses = this.dataFile.map((expense: any) => expense.no);
          // Chuyển danh sách các expense đã duyệt thành JSON để gửi đi
          this.plan.approvedExpenses = JSON.stringify(this.approvedExpenses);
          console.log(this.plan.approvedExpenses);
          // Gửi yêu cầu cập nhật expense đã duyệt và duyệt kế hoạch
          this.status = 2; 
          return this.planService.submitPlan(id, this.status).pipe(
            concatMap(() => this.planService.submitExpense(id, this.plan.approvedExpenses))
          );;

          } else {
            return of(null);
          }
        })
      )
      .subscribe((response) => {
        // Check if response is null, if yes, it means user cancelled, so don't open any message bar
        if (response !== null && response === 200) {
          this.messageBar.open('Change status successfully', 'Close', {
            duration: 1000,
            panelClass: ['success'],
          });
          window.location.reload();

        }
      });
  }

  openSubmitExpenseDialog(id: string) {
    const SubmitDialog1 = this.dialog.open(SubmitExpenseDialog, {
      width: '400px',
      height: '250px',
    });

    SubmitDialog1
      .afterClosed()
      .pipe(
        concatMap((result) => {
          if (result === 'Submit') {
            console.log(this.plan.approvedExpenses);
            this.plan.approvedExpenses = JSON.stringify(this.approvedExpenses);
            console.log(this.areAllExpensesApproved());
            if (this.areAllExpensesApproved()) {
              this.status = 2;
              return this.planService.submitPlan(id, this.status).pipe(
                concatMap(() => this.planService.submitExpense(id, this.plan.approvedExpenses))
              );
            } else {
            return this.planService.submitExpense(id, this.plan.approvedExpenses);
             
            }
          } else {
            return of(null);
          }
        })
      )
      .subscribe((response) => {
        // Check if response is null, if yes, it means user cancelled, so don't open any message bar
        if (response !== null && response === 200) {
          this.messageBar.open('Change status successfully', 'Close', {
            duration: 1000,
            panelClass: ['success'],
          });
          window.location.reload();

        }
      });
  }
  areAllExpensesApproved(): boolean {
    return this.dataFile.every((expense: any) => this.isExpenseApproved(expense.no));
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

  downloadFile(planId: string, version: number) {
    this.planService.exportPlan(planId, version).subscribe((data: any) => {
        const downloadUrl = data.downloadUrl;
         // create hidden link to download
         const link = document.createElement('a');
         link.href = downloadUrl;
         link.setAttribute('download', '');

         // Add link into web and click it to download
         document.body.appendChild(link);
         link.click();

         // remove link after download 
         document.body.removeChild(link)
      }, 
    );
  }

}
@Component({
  selector: 'submit-plan',
  standalone: true,
  templateUrl: '../submit-plan/submit-plan.component.html',
  styleUrls: ['../submit-plan/submit-plan.component.css'],
  imports: [MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
})

export class SubmitPlanDialog {
  constructor(public dialogRef: MatDialogRef<SubmitPlanDialog>) {}
}

@Component({
  selector: 'submit-expense',
  standalone: true,
  templateUrl: '../submit-expense/submit-expense.component.html',
  styleUrls: ['../submit-expense/submit-expense.component.css'],
  imports: [MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
})

export class SubmitExpenseDialog {
  constructor(public dialogRef: MatDialogRef<SubmitExpenseDialog>) {}
}
@Component({
  selector: 'approve-expense',
  standalone: true,
  templateUrl: '../approve-plan/approve-plan.component.html',
  styleUrls: ['../approve-plan/approve-plan.component.css'],
  imports: [MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
})

export class ApproveExpenseDialog {
  constructor(public dialogRef: MatDialogRef<ApproveExpenseDialog>) {}
}
