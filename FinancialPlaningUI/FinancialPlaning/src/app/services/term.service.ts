import { Injectable } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CreateTermModel } from '../models/term.model';

@Injectable({
  providedIn: 'root'
})
export class  TermService {
  private apiUrl = environment.apiUrl + '/Term';

  constructor(private http: HttpClient) {}
  createTerm(term: CreateTermModel) : Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515'
    console.log(term);
    return this.http.post(this.apiUrl, term);
  }

  getAllTerms() : Observable<any> {
    return this.http.get(this.apiUrl);
  }

  deleteTerm(termId: string) : Observable<any> {
    return this.http.delete(this.apiUrl + '/' + termId);
  }

  updateTerm(termId: string, term: CreateTermModel): Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515'
    return this.http.put(this.apiUrl + '/' + termId + '/update', term);
  }

  closeTerms(): Observable<any> {
    return this.http.put(`${this.apiUrl}/close`, {}); // You might need to adjust this URL based on your backend implementation
  }

  getTerm(termId: string): Observable<any> {
    return this.http.get(this.apiUrl + '/' + termId);
  }
}
