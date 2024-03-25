import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from '../models/user.model';
import { EditUser } from '../models/edituser.model';
import { AddUser } from '../models/adduser.model';
import { IDepartment } from '../models/department-list';
import { IRole } from '../models/role-list';
import { IPosition } from '../models/position-list';
import { Observable, catchError, map } from 'rxjs';
import { environment } from '../../environments/environment';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = environment.apiUrl + '/User';

  constructor(private http: HttpClient) { }

  getAllUser() {
    return this.http.get<UserModel[]>(this.apiUrl)
  }

  getUserById(userId: string) {
    return this.http.get<EditUser[]>(this.apiUrl +'/'+ userId)
  }

  editUser(userId: string, user: AddUser) {
    return this.http.put<AddUser>(this.apiUrl + '/' + userId, user)
  }
  changeUserStatus(userId: string, status: number): Observable<number> {
    return this.http.put(this.apiUrl + '/' + userId + '/' + status,{},{ observe: 'response', responseType: 'text' }).pipe(
      map((response: HttpResponse<any>) => response.status),
      catchError((error: HttpErrorResponse) => {
        console.error('Error occurred:', error);
        throw error;
      })
    );
    // return this.http.put(`http://localhost:5085/api/User/${userId}/${status}`, {}, { observe: 'response', responseType: 'text' })
    //   .pipe(
    //     map((response: HttpResponse<any>) => response.status),
    //     catchError((error: HttpErrorResponse) => {
    //       console.error('Error occurred:', error);
    //       throw error;
    //     })
    //   );
  }

  addNewUser(user: AddUser) {
    return this.http.post(this.apiUrl, user);
  }

  getAllDepartment() {
    return this.http.get<IDepartment[]>(this.apiUrl + '/'+'AllDepartments')
  }

  getRole() {
    return this.http.get<IRole[]>(this.apiUrl + '/'+'AllRoles')
  }

  getPosition() {
    return this.http.get<IPosition[]>(this.apiUrl + '/'+'AllPositions')
  }
}
