import { Injectable } from '@angular/core';
import { HttpClient, HttpResponse, HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { CreateTermModel } from '../models/term.model';

@Injectable({
  providedIn: 'root',
})
export class TermService {
  private apiUrl = environment.apiUrl + '/Term';
  constructor(private http: HttpClient) {} // Correct injection through

  createTerm(term: CreateTermModel): Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515';
    console.log(term);
    return this.http.post(this.apiUrl, term);
  }

  getAllTerms(): Observable<any> {
    return this.http.get(this.apiUrl + '/all');
  }

  deleteTerm(termId: string): Observable<number> {
    return this.http
      .delete(this.apiUrl + '/' + termId, {
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

  updateTerm(termId: string, term: CreateTermModel): Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515';
    return this.http.put(this.apiUrl + '/update/' + termId, term);
  }

  getTerm(termId: string): Observable<any> {
    return this.http.get(this.apiUrl + '/' + termId);
  }
}
