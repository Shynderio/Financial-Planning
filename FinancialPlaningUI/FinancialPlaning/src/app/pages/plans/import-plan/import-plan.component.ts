import { Component, OnInit } from '@angular/core';
import { UploadComponent } from '../../../components/upload/upload.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { TermService } from '../../../services/term.service';
import { MatPaginatorModule, PageEvent } from '@angular/material/paginator';
import { MatTableModule } from '@angular/material/table';
import { PlanService } from '../../../services/plan.service';

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
  ],
  templateUrl: './import-plan.component.html',
  styleUrls: ['./import-plan.component.css']
})
export class ImportPlanComponent implements OnInit {

  termService: TermService;
  planService: PlanService;
  termOptions: {value: string, viewValue: string}[] = [];
  // ];
  dataSource: any = [];
  listSize: number = 0;
  pageSize = 7;
  pageIndex = 0;
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

  constructor(termService: TermService, planService: PlanService) {
    this.termService = termService;
    this.planService = planService;
  }

  ngOnInit() {
    this.termService.getStartedTerms().subscribe(
      (data: any[]) =>{
        this.termOptions = data.map(term => {
          return {value: term.id, viewValue: term.termName};
        });
        // console.log(this.termOptions);
      },
      error => {
        console.log(error);
      }
      );
  };


  onFileSelected(event: any) {
    // debugger;
    this.file = event;
    // if (this.file) {
      // Handle the selected file here, for example:
    console.log('Selected file:', this.file);
      // You can trigger the file upload process here if needed
      // this.uploadFile(file);
    // }
  }

  onImport() {
    if (this.file){
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
    }
  }

  onPageChange(event: PageEvent) {
    this.pageIndex = event.pageIndex;
    // this.dataSource = this.getPaginatedItems();
  }


}
