import { Component, ElementRef, OnInit } from '@angular/core';
import { UploadComponent } from '../../../share/upload/upload.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { TermService } from '../../../services/term.service';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { PlanService } from '../../../services/plan.service';
import { FormBuilder, FormGroup, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatSnackBar } from '@angular/material/snack-bar';
import { jwtDecode } from 'jwt-decode';
import { RouterLink } from '@angular/router';
import { MatCard } from '@angular/material/card';
import e from 'express';

@Component({
  selector: 'app-import-plan',
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
  templateUrl: './import-plan.component.html',
  styleUrls: ['./import-plan.component.css']
})
export class ImportPlanComponent implements OnInit {


  termService: TermService;
  planService: PlanService;
  termOptions: { value: string, viewValue: string }[] = [];
  planForm: FormGroup;
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
  isTermSelected: boolean = false;
  validFileName: string = '';
  constructor(termService: TermService, 
    planService: PlanService, 
    private fb: FormBuilder, 
    private elementRef: ElementRef,
    private messageBar: MatSnackBar) {
    this.termService = termService;
    this.planService = planService;
    this.planForm = this.fb.group({
      term: ['', Validators.required],
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
      this.planService.importPlan(this.file).subscribe(
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
    if (this.planForm.valid) {
      if (this.file){
        var term = this.planForm.value.term.value;
        this.elementRef.nativeElement.querySelector('.submit-button').disabled = true;
        this.planService.createPlan(term, this.dataSource).subscribe(
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
            this.messageBar.open(
              'Error uploading plan.',
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

  onTermSelect(event: any) {
    debugger;
    this.selectedTermId = event.value.value;
    this.isTermSelected = true;
    var token = localStorage.getItem('token') ?? '';
    var decodedToken: any = jwtDecode(token);
    this.validFileName = decodedToken.departmentName + '_' + event.value.viewValue + '_Plan';
    // console.log('Selected term:', event.viewValue); 
    console.log('Valid filename:', this.validFileName); 
  }

  exportPlanTemplate() {
    throw new Error('Method not implemented.');
    }
}
