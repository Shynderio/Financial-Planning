import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable, catchError, map } from 'rxjs';
import { Report } from '../models/report.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ReportService {
  private apiUrl = environment.apiUrl + '/Report';
  constructor(private http: HttpClient) { }

  getListReport(): Observable<any> {
    return this.http.get<any>(this.apiUrl+'/reports');
  }
 
  deleteReport(reportId: string): Observable<number> {
    return this.http
      .delete(this.apiUrl + '/' + reportId, {
        observe: 'response',
        responseType: 'text',
      })
      .pipe(
        map((response: HttpResponse<any>) => response.status),
        catchError((error: HttpErrorResponse) => {
          console.error('Error occurred:', error);
          throw error;
        })
      );
  }

  getReport(reportId: string): Observable<any> {
    return this.http.get(this.apiUrl + '/details/' + reportId);
  }
  
  exportSinglereport(reportId: string,version:number){
    return this.http.get(this.apiUrl+'/export/'+reportId+'/'+version)
  }



}
