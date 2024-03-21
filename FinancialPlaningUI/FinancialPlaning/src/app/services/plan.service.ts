import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
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
  getAllTerms(): Observable<any> {
    return this.http.get(this.apiUrl);
  }
  importPlan(file: File): Observable<any> {
    const formData = new FormData();
    formData.append('file', file);
    return this.http.post(this.apiUrl + '/import', formData)
  }

  uploadPlan(termId: string, uid: string, expenses: []): Observable<any> {
    let urlParams = new URLSearchParams();
    urlParams.append('termId', termId);
    urlParams.append('uid', uid);
    return this.http.post(this.apiUrl + '/upload?' + urlParams, expenses)
  }


}