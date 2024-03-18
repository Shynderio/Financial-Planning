import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { Report } from '../models/report.model';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  constructor(private http: HttpClient) { }
  getListReport(): Observable<any> {
    return this.http.get<any>('http://localhost:5085/api/Report/reports');
  }
  getReports(): Observable<Report[]> {
    return this.http.get<Report[]>('http://localhost:5085/api/Report/reports');
  }
}
