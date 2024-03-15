import { Injectable, inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { CreateTermModel } from '../models/term.model';

@Injectable({
  providedIn: 'root'
})

export class TermService {
  private apiUrl = environment.apiUrl + '/Term';
  constructor(private http: HttpClient) {
  } // Correct injection through

  createTerm(term: CreateTermModel): Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515';
    console.log(term);
    return this.http.post(this.apiUrl, term);
  }

  getAllTerms(): Observable<any> {
    return this.http.get(this.apiUrl + '/all');
  }

  deleteTerm(termId: string): Observable<any> {
    return this.http.delete(this.apiUrl + '/' + termId);
  }

  updateTerm(termId: string, term: CreateTermModel): Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515';
    return this.http.put(this.apiUrl + '/update/' + termId, term);
  }

  getTerm(termId: string): Observable<any> {
    return this.http.get(this.apiUrl + '/' + termId);
  }
}
