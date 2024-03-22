import { HttpClient, HttpErrorResponse, HttpResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { UserModel } from '../models/user.model';
import { EditUser } from '../models/edituser.model';
import { AddUser } from '../models/adduser.model';
import { IDepartment } from '../models/department-list';
import { IRole } from '../models/role-list';
import { IPosition } from '../models/position-list';
import { Observable, catchError, map } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  apiUrl = "http://localhost:5085";

  constructor(private http: HttpClient) { }

  getAllUser() {
    return this.http.get<UserModel[]>(this.apiUrl + "/api/User")
  }

  getUserById(userId: string) {
    return this.http.get<EditUser[]>(this.apiUrl + '/api/User/' + userId)
  }

  editUser(userId: string, user: AddUser) {
    return this.http.put<AddUser>(this.apiUrl + '/api/User/' + userId, user)
  }
  changeUserStatus(userId: string, status: number): Observable<number> {
    return this.http.put(`http://localhost:5085/api/User/${userId}/${status}`, {}, { observe: 'response', responseType: 'text' })
      .pipe(
        map((response: HttpResponse<any>) => response.status),
        catchError((error: HttpErrorResponse) => {
          console.error('Error occurred:', error);
          throw error;
        })
      );
  }

  addNewUser(user: AddUser) {
    return this.http.post(this.apiUrl + "/api/User", user);
  }

  getAllDepartment() {
    return this.http.get<IDepartment[]>(this.apiUrl + "/api/User/AllDepartments")
  }

  getRole() {
    return this.http.get<IRole[]>(this.apiUrl + "/api/User/AllRoles")
  }

  getPosition() {
    return this.http.get<IPosition[]>(this.apiUrl + "/api/User/AllPositions")
  }
}
