import { Component, OnInit, ViewChild, inject } from '@angular/core';
import { HttpClient, HttpClientModule } from '@angular/common/http';
import { CommonModule } from '@angular/common';
import { Router, RouterLink, RouterOutlet } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { NgxPaginationModule } from 'ngx-pagination';
import { SearchListUserPipe } from './search-list-user.pipe';
import { UserModel } from '../../../models/user.model';



@Component({
  selector: 'app-user-list',
  standalone: true,
  templateUrl: './user-list.component.html',
  styleUrl: './user-list.component.css',
  imports: [RouterLink,CommonModule, RouterOutlet, HttpClientModule, FormsModule, SearchListUserPipe, NgxPaginationModule]
})
export class UserListComponent implements OnInit {
  userList: UserModel[] = [];
  router = inject(Router);

  // To store unfiltered user data
  originalUserList: UserModel[] = [];

  // Define roles array to store list of roles
  roles: string[] = [];

  selectedRole: string = '';

  searchQuery = '';

  //basic page
  p = 1;

  httpClient = inject(HttpClient);

  //Get list users
  getListUsers(): void {
    this.httpClient.get('http://localhost:5085/api/User').subscribe((data: any) => {
      this.userList = data;

      // Save original data for resetting
      this.originalUserList = data;

      console.log(this.userList);

      // Populate roles array with unique role names from userList
      this.roles = Array.from(new Set(data.map((user: UserModel) => user.roleName)));
    });
  }


  //Filter user by role
  filterUsers() {
    // Reset with a copy
    this.userList = this.originalUserList.slice();

    //  Apply filtering based on roleName
    if (this.selectedRole) {
      this.userList = this.userList.filter(user => user.roleName === this.selectedRole);
    }
  }

  ngOnInit(): void {
    this.getListUsers();
  }

  edit(id:string){
    console.log(id);
    this.router.navigateByUrl("/edit-user/"+id)
  }
}


