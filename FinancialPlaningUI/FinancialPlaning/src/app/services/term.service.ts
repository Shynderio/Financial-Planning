import { Injectable, inject } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable, map, tap } from 'rxjs';
import { CreateTermModel } from '../models/term.model';
import { TermViewModel } from '../models/termview.model';

@Injectable({
  providedIn: 'root'
})
export class TermService {
  private apiUrl = environment.apiUrl + '/Term';
  http = inject(HttpClient);
  // constructor(private http: HttpClient) { }
  createTerm(term: CreateTermModel): Observable<any> {
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515'
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
    term.creatorId = 'BAAF33C7-5E18-49DD-A1A9-666EF8F11515'
    return this.http.put(this.apiUrl + '/update' + termId, term);
  }

  getTerm(termId: string): Observable<any> {
    var term = this.http.get(this.apiUrl + '/' + termId);
    
    // console.log(term);
    return term;
  }

}
