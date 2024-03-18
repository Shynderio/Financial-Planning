import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Plan } from '../models/planviewlist.model';

@Injectable({
  providedIn: 'root'
})
export class PlanService {
  private apiUrl = 'api/Plan/Planlist'; // Đường dẫn tới API

  constructor(private http: HttpClient) { }

  getFinancialPlans(keyword: string, department: string, status: string): Observable<Plan[]> {
    // Gọi API để lấy danh sách kế hoạch tài chính dựa trên các tham số đầu vào
    return this.http.get<Plan[]>(`${this.apiUrl}`);
  }
}
