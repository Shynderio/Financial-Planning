import { Component } from '@angular/core';
import { ReportService } from '../../services/report.service';

@Component({
  selector: 'app-list-report',
  standalone: true,
  imports: [],
  templateUrl: './list-report.component.html',
  styleUrl: './list-report.component.css',
})
export class ListReportComponent {
  constructor(private reportService: ReportService) {}

  ngOnInit(): void {
    this.getListReport();
  }

  getListReport(): void {
    this.reportService.getListReport().subscribe({
      next: (response: any) => {
        console.log(response); // Log response to the console
      },
    });
  }
}
