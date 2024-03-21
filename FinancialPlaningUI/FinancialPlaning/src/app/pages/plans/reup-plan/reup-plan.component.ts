import { Component, ElementRef, OnInit } from '@angular/core';
import { UploadComponent } from '../../../components/upload/upload.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { CommonModule } from '@angular/common';
import { MatTableModule } from '@angular/material/table';
import { ReactiveFormsModule } from '@angular/forms';
import { PlanService } from '../../../services/plan.service';
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-reup-plan',
  standalone: true,
  imports: [
    UploadComponent,
    MatFormFieldModule,
    MatSelect,
    MatOption, CommonModule,
    MatPaginatorModule,
    MatTableModule,
    ReactiveFormsModule
  ],
  templateUrl: './reup-plan.component.html',
  styleUrls: ['./reup-plan.component.css']
})
export class ReupPlanComponent implements OnInit {
  planService: PlanService;
  dataSource: any = [];
  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;
  file: any;
  term: string = '';
  columnHeaders: string[] = [
    'expense',
    'costType',
    'unitPrice',
    'amount',
    'total',
    'projectName',
    'supplierName',
    'pic',
    'notes'
  ];
  constructor(planService: PlanService, private elementRef: ElementRef,
    private messageBar: MatSnackBar, private route: ActivatedRoute, private router: Router) {
    this.planService = planService;
  }

  ngOnInit() {
    // // get term from parent router
    // this.route.params.subscribe(params => {
    //   // Check if 'term' parameter exists
    //   if (params && params['term']) {
    //     this.term = params['term'];
    //   } else {
    //     // Redirect to 'planlist'
    //     this.router.navigate(['/plans']);
    //   }
    // });
  }

  onFileSelected(event: any) {
    // debugger;
    this.file = event;
    console.log('Selected file:', this.file);
  }

  onImport() {
    if (this.file) {
      console.log('Importing file:', this.file);
      this.planService.importPlan(this.file).subscribe(
        (data: any) => {
          this.dataSource = data;
          console.log(data);
        },
        error => {
          console.log(error);
        }
      );
    } else {
      this.messageBar.open(
        "Please select a file to preview.",
        undefined,
        {
          duration: 5000,
          panelClass: ['messageBar', 'successMessage'],
          verticalPosition: 'top',
          horizontalPosition: 'end',
        }
      );
    }
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    // this.dataSource = this.getPaginatedItems();
  }

  onSubmit() {
    debugger;

    if (this.file) {
      const token = localStorage.getItem('token') ?? '';

      const decodedToken: any = jwtDecode(token);
      var uid = decodedToken.userId;
      this.elementRef.nativeElement.querySelector('.submit-button').disabled = true;
      this.planService.uploadPlan(this.term, uid, this.dataSource).subscribe(
        (data: any) => {
          console.log('Plan uploaded:', data);
          this.messageBar.open(
            "Uploaded successfully.",
            undefined,
            {
              duration: 5000,
              panelClass: ['messageBar', 'successMessage'],
              verticalPosition: 'top',
              horizontalPosition: 'end',
            }
          );

        },
        error => {
          console.log('Error uploading plan:', error);
        }
      );
    } else {
      // console.log('Please select a file to upload.');
      this.messageBar.open(
        "Please select a file to upload.",
        undefined,
        {
          duration: 5000,
          panelClass: ['messageBar', 'successMessage'],
          verticalPosition: 'top',
          horizontalPosition: 'end',
        }
      );
      this.elementRef.nativeElement.querySelector('.submit-button').disabled = false;
    }

  }
}
