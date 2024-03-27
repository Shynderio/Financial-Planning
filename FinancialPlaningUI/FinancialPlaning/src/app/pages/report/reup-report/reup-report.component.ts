import { Component, ElementRef, OnInit } from '@angular/core';
import { UploadComponent } from '../../../share/upload/upload.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { TermService } from '../../../services/term.service';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { ReportService } from '../../../services/report.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';
import { Router, RouterLink } from '@angular/router';
import { MatCard } from '@angular/material/card';
import { start } from 'repl';
import { SelectTermModel } from '../../../models/select-term.model';
import { ActivatedRoute } from '@angular/router';
import { MessageBarComponent } from '../../../share/message-bar/message-bar.component';

@Component({
  selector: 'app-import-report',
  standalone: true,
  imports: [
    UploadComponent,
    MatFormFieldModule,
    MatSelect,
    MatOption, CommonModule,
    MatPaginatorModule,
    MatTableModule,
    ReactiveFormsModule,
    RouterLink,
    MatCard
  ],
  templateUrl: './reup-report.component.html',
  styleUrls: ['./reup-report.component.css']
})
export class ReupReportComponent implements OnInit {

  reportService: ReportService;
  // reportForm: FormGroup;
  reportId: string = '';
  dataSource: any = [];
  //paging
  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;
  filedata: any = [];
  file: any;
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

  constructor(
    reportService: ReportService,
    private fb: FormBuilder,
    private elementRef: ElementRef,
    private messageBar: MatSnackBar,
    private route: ActivatedRoute,
    private router: Router) {
    this.reportService = reportService;
    // this.reportForm = this.fb.group({
    //   term: ['', Validators.required],
    //   month: ['', Validators.required],
    //   // fileInput: [null, Validators.required]
    // });
  }

  ngOnInit() {
    this.route.params.subscribe(params => {
      const reportId = params['id'];
      this.reportService.getReport(reportId).subscribe((data: any) => {
        if (!data) {
          // Redirect to 'reportlist'
          this.messageBar.openFromComponent(MessageBarComponent, {
            duration: 5000,
            data: {
              success: false,
              message:
                'Report not found. Redirecting to report list.'
            },
          });
        } else {
          this.reportId = reportId;
        }
      });
      // Use the reportId as needed
    });
  }

  //filter page
  getPaginatedItems() {
    const startIndex = this.pageIndex * this.pageSize;
    let filteredList = this.filedata;
    this.listSize = filteredList.length;
    return filteredList.slice(startIndex, startIndex + this.pageSize);
  }
  
  onImport() {
    if (this.file) {
      console.log('Importing file:', this.file);
      this.reportService.importReport(this.file).subscribe(
        (data: any) => {
          this.filedata = data;
          this.dataSource = this.getPaginatedItems();
          console.log(data);
        },
        error => {
          console.log(error);
          this.messageBar.open(
            error.error.message,
            undefined,
            {
              duration: 5000,
              panelClass: ['messageBar', 'successMessage'],
              verticalPosition: 'top',
              horizontalPosition: 'end',
            }
          );
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
    this.dataSource = this.getPaginatedItems();
  }

  onSubmit() {
    // debugger;
    // var term = this.reportForm.value.term;
    if (this.file) {
      this.elementRef.nativeElement.querySelector('.submit-button').disabled = true;
      this.reportService.reupReport(this.filedata, this.reportId).subscribe(
        (data: any) => {
          console.log('report uploaded:', data);
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
          this.router.navigate(['/reports']);
        },
        error => {
          this.elementRef.nativeElement.querySelector('.submit-button').disabled = false;
          console.log('Error uploading report:', error);
          this.messageBar.open(
            error.error.message,
            undefined,
            {
              duration: 5000,
              panelClass: ['messageBar', 'successMessage'],
              verticalPosition: 'top',
              horizontalPosition: 'end',
            }
          );
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

  onFileSelected(event: any) {
    // debugger;
    this.file = event;
    console.log('Selected file:', this.file);
  }
}


