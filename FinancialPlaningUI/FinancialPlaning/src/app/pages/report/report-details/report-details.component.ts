import { Component } from '@angular/core';
import { ReportService } from '../../../services/report.service';
import { Router } from 'express';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-report-details',
  standalone: true,
  imports: [],
  templateUrl: './report-details.component.html',
  styleUrl: './report-details.component.css'
})
export class ReportDetailsComponent {

  displayedColumns: string[] = [];
  dataSource: any = [];

   constructor(
    private reportService:ReportService,
    private route: ActivatedRoute,
    ){}

   ngOnInit(): void {
    this.route.params.subscribe(params => {
      const reportId = params['id']; // Assuming 'id' is the parameter name
      this.getReport(reportId);
    }
    );
    
   }

   getReport(reportId: string){
    this.reportService.getReport(reportId).subscribe((data: any) => {
      this.dataSource =data;
      console.log(data);
    });
  }
}
