import { Component } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { Router } from 'express';
import { ActivatedRoute } from '@angular/router';
import { MatTableDataSource, MatTableModule } from '@angular/material/table';
import { MatSelectModule } from '@angular/material/select';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-report-details',
  standalone: true,
  imports: [  
    CommonModule,
    MatTableModule,
    MatSelectModule],
  templateUrl: './report-details.component.html',
  styleUrl: './report-details.component.css'
})
export class ReportDetailsComponent {

  displayedColumns: string[] = [
   'No','Expense','CostType','Unit Price (VND)','Total',
   'Amount','Project name','Supplier name',
   'PIC','Notes'
  ];

  dataSource: any = [];
  report : any;

   constructor(
    private reportService:ReportService,
    private route: ActivatedRoute,
    
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

   getReport(reportId: string){
    this.reportService.getReport(reportId).subscribe((data: any) => {

      this.dataSource =data.expenses;
      this.report = data.report;
      console.log(data);
      
    });
  }
}
