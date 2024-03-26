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
  templateUrl: './import-report.component.html',
  styleUrls: ['./import-report.component.css']
})
export class ImportReportComponent implements OnInit {


  termService: TermService;
  reportService: ReportService;
  termOptions: SelectTermModel[] = [];
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

  months: string[] = [
    "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December"
  ];
  selectedTermId: string = '';
  constructor(termService: TermService,
    reportService: ReportService,
    private fb: FormBuilder,
    private elementRef: ElementRef,
    private messageBar: MatSnackBar, private router: Router) {
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
      (data: SelectTermModel[]) => {
        this.termOptions = data;
        console.log(this.termOptions);
      },
      error => {
        console.log(error);
      }
    );

  };

  changeTerm() {
    // debugger;
    var term = this.reportForm.value.term;
    if (term) {
      var startMonth = new Date(term.startDate).getMonth();
      var startYear = new Date(term.startDate).getFullYear();

      var monthOptions = [];
      for (let i = 0; i < term.duration; i++) {
        const currentMonthIndex = (startMonth + i) % 12;
        const currentYear = startYear + Math.floor((startMonth + i) / 12);
        monthOptions.push(this.months[currentMonthIndex] + ' ' + currentYear);
      }

      this.monthOptions = monthOptions;

      console.log('Selected term:', term);
      console.log('Month options:', this.monthOptions);
      // this.monthOptions = this.months.slice(0, selectedTerm.duration);
    }
  }

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
          this.messageBar.openFromComponent(MessageBarComponent, {
            data: { 
              message: error.error.message,
              success: false},

            })
        }
      );
    } else {
      this.messageBar.openFromComponent(MessageBarComponent, {
        data: { 
          message: 'Please select a file to preview.' ,
          success: false},
        })
    }
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    this.dataSource = this.getPaginatedItems();
  }

  onSubmit() {
    // debugger;
    var term = this.reportForm.value.term;
    if (this.reportForm.valid && term) {
      if (this.file) {
        var id = term.id;
        var month = this.reportForm.value.month;
        this.elementRef.nativeElement.querySelector('.submit-button').disabled = true;
        this.reportService.uploadReport(this.dataSource, id, month).subscribe(
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
            console.log('Error uploading report:', error);
            this.messageBar.openFromComponent(MessageBarComponent, {
              data: { 
                message: error.error.message ,
                success: false},
              })
          }
        );
      } else {
        // console.log('Please select a file to upload.');
        this.messageBar.openFromComponent(MessageBarComponent, {
          data: {
            message: 'Please select a file to upload.',
            success: false
          },
        })
        this.elementRef.nativeElement.querySelector('.submit-button').disabled = false;
      }
    } else {
      // console.log('Form is invalid.');
      this.messageBar.openFromComponent(MessageBarComponent, {
        data: {
          message: 'Please select a term.',
          success: false
        },
      })
    }
  }

  onTermSelect(termId: string) {
    this.selectedTermId = termId;
    console.log('Selected term:', termId);
  }

}
