import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [ReactiveFormsModule,FormsModule],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.css'
})
export class UserDetailComponent implements OnInit{
  userForm: FormGroup;
 
  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService
  ) {
    this.userForm = this.fb.group({
      username: [{ value: '', disabled: true }],
      fullname: [{ value: '', disabled: true }],
      dob: [{ value: '', disabled: true }],
      email: [{ value: '', disabled: true }],
      address: [{ value: '', disabled: true }],
      phonenumber: [{ value: '', disabled: true }],
      department: [{ value: '', disabled: true }],
      position: [{ value: '', disabled: true }],
      role: [{ value: '', disabled: true }],
      status: [{ value: '', disabled: true }],
      note: [{ value: '', disabled: true }],
    });
  }


  ngOnInit(): void {

    this.route.params.subscribe(params => {
      const userId = params['id']; // Assuming 'id' is the parameter name
      this.loadUserDetail(userId);
    }
    );
  }

  loadUserDetail(userId: string): void {
    // debugger;
    this.userService.getUserById(userId).subscribe({
      next: (userDetails: any) => {
        // Assuming termDetails contains the required data
        this.userForm.patchValue({
          username: userDetails.username,
          fullname: userDetails.fullName,
          dob: userDetails.dob,
          email: userDetails.email,
          department: userDetails.departmentName,
          position: userDetails.positionName,
          role: userDetails.roleName,
          status: userDetails.status,
          note: userDetails.notes,
          phonenumber: userDetails.phoneNumber,
          address: userDetails.address,
          
        });
        console.log(userDetails);
      },
      error: (error: any) => {
        // Handle error
        console.error('Error fetching term details:', error);
      }
    });
    
  }
  onSubmit() {
    // Handle form submission if needed
  }
  cancel(): void {
    // Điều hướng người dùng trở lại trang "user-list" khi nhấp vào nút "Cancel"
    this.router.navigate(['/user-list']);
  }
}