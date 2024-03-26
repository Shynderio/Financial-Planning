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
import { RouterLink } from '@angular/router';
import { MatCard } from '@angular/material/card';

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
  templateUrl: './import-report.component.html',
  styleUrls: ['./import-report.component.css']
})
export class ImportReportComponent implements OnInit {

  termService: TermService;
  reportService: ReportService;
  termOptions: { value: string, viewValue: string }[] = [];
  monthOptions: string[] = [];
  reportForm: FormGroup;
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
  selectedTermId: string = '';
  constructor(termService: TermService, 
    reportService: ReportService, 
    private fb: FormBuilder, 
    private elementRef: ElementRef,
    private messageBar: MatSnackBar) {
    this.termService = termService;
    this.reportService = reportService;
    this.reportForm = this.fb.group({
      term: ['', Validators.required],
      month: ['', Validators.required],
      // fileInput: [null, Validators.required]
    });
  }

  ngOnInit() {
    this.termService.getStartedTerms().subscribe(
      (data: any[]) => {
        this.termOptions = data.map(term => {
          return { value: term.id, viewValue: term.termName };
        });
        console.log(this.termOptions);
      },
      error => {
        console.log(error);
      }
    );

    this.monthOptions = [
      "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
    ];
  };


  onFileSelected(event: any) {
    // debugger;
    this.file = event;
    console.log('Selected file:', this.file);
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
    debugger;
    if (this.reportForm.valid) {
      if (this.file){
        var term = this.reportForm.value.term;
        var month = this.reportForm.value.month;
        this.elementRef.nativeElement.querySelector('.submit-button').disabled = true;
        this.reportService.uploadReport(this.dataSource, term, month).subscribe(
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
            
          },
          error => {
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
    } else {
      // console.log('Form is invalid.');
      this.messageBar.open(
        "Please select a term.",
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

  onTermSelect(termId: string) {
    this.selectedTermId = termId;
    console.log('Selected term:', termId);  
  }

}
