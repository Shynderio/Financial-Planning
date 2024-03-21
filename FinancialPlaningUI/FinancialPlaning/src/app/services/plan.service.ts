import { Injectable } from '@angular/core';
import { HttpClient, HttpErrorResponse, HttpParams, HttpResponse } from '@angular/common/http';
import { Observable, catchError, map } from 'rxjs';
import { Plan } from '../models/planviewlist.model';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})


@Injectable({
  providedIn: 'root'
})
export class PlanService {
  private apiUrl = environment.apiUrl + '/Plan';


  constructor(private http: HttpClient) { }

  getFinancialPlans(keyword: string = '', department: string = '', status: string = ''): Observable<Plan[]> {
    // Tạo các tham số query
    let params = new HttpParams();
    if (keyword) {
      params = params.set('keyword', keyword);
    }
    if (department) {
      params = params.set('department', department);
    }
    if (status) {
      params = params.set('status', status);
    }

    // Thực hiện gọi HTTP GET đến API endpoint
    return this.http.get<Plan[]>(`${this.apiUrl}`, { params });
  }
  getAllPlans(): Observable<any> {
    return this.http.get(this.apiUrl + '/Planlist');
  }
  importPlan(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(this.apiUrl + '/import', formData)
  }
  deletePlan(PlanId: string): Observable<number> {
    return this.http
      .delete(this.apiUrl + '/' + PlanId, {
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
}