import { Component, OnInit, inject } from '@angular/core';
import { AbstractControl, FormControl, FormGroup, FormsModule, NgForm, ReactiveFormsModule, ValidatorFn, Validators } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { IDepartment } from '../../../models/department-list';
import { IRole } from '../../../models/role-list';
import { IPosition } from '../../../models/position-list';
import { AddUser } from '../../../models/adduser.model';
import { UserService } from '../../../services/user.service';

@Component({
  selector: 'app-add-new-user',
  standalone: true,
  imports: [RouterLink, FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './add-new-user.component.html',
  styleUrl: './add-new-user.component.css'
})
export class AddNewUserComponent implements OnInit {
  departments: IDepartment[] = [];
  roles: IRole[] = [];
  positions: IPosition[] = [];
  userId!: string;
  isEdit = false;
 

  constructor(
    private httpService: UserService,
    private toastr: ToastrService,
    private route: ActivatedRoute,
    private router: Router
  ) {}


  ngOnInit(): void {
    this.getDepartments();
    this.getRoles();
    this.getPositions();

    this.route.params.subscribe(params => {
      const userId = params['id'];
      this.getUserById(userId);
    })
  }
  //Set default status is 1
  getStatusLabel(status: string): string {
    return status === '1' ? 'Active' : 'DeActive';
  }

  getUserById(userId: string) {
    if (userId) {
      this.isEdit = true;
      this.userId = userId;
    }
    this.httpService.getUserById(userId).subscribe(
      (userDetail: any) => {
        const formattedDate = this.convertDateFormat(userDetail.dob);
        const departmentId = this.departments.find(department => department.departmentName === userDetail.departmentName)?.id;
        const roleId = this.roles.find(role => role.roleName == userDetail.roleName)?.id;
        const positionId = this.positions.find(position => position.positionName == userDetail.positionName)?.id;
        this.addUserF.patchValue({
          username: userDetail.username,
          department: userDetail.departmentId,
          role: userDetail.roleId,
          position: userDetail.positionId,
          notes: userDetail.notes,
          email: userDetail.email,
          fullName: userDetail.fullName,
          phoneNumber: userDetail.phoneNumber,
          address: userDetail.address,
          dob: formattedDate
        })
      }
    )
  }
  convertDateFormat(inputDate: string): string {
    // Tách ngày, tháng và năm từ chuỗi đầu vào
    const parts = inputDate.split('-');
    const day = parts[0];
    const month = parts[1];
    const year = parts[2];

    // Chuyển đổi sang định dạng "yyyy-MM-dd"
    const formattedDate = year + '-' + month + '-' + day;

    return formattedDate;
  }


  //Get list departments
  getDepartments() {
    this.httpService.getAllDepartment().subscribe(
      (data: IDepartment[]) => {
        this.departments = data;
      },
      (error: any)  => {
        console.log('Error fetching departments:', error);
      }
    );
  }

  //Get list roles
  getRoles() {
    this.httpService.getRole().subscribe(
      (data: IRole[]) => {
        this.roles = data;
      },
      (error: any)  => {
        console.log('Error fetching departments:', error);
      }
    );
  }

  // Get list positions
  getPositions() {
    this.httpService.getPosition().subscribe(
      (data: IPosition[]) => {
        this.positions = data;
      },
      (error: any)  => {
        console.log('Error fetching departments:', error);
      }
    );
  }

  // Validate data
  
  addUserF: FormGroup = new FormGroup({
    username: new FormControl(''),
    fullName: new FormControl('', [Validators.required, Validators.pattern('^[A-Za-zÀ-ỹ ]+$')]),
    email: new FormControl('', [Validators.required, Validators.email]),
    dob: new FormControl('', [Validators.required, this.validateDate]),
    department: new FormControl('', [Validators.required]),
    role: new FormControl('', [Validators.required]),
    notes: new FormControl(''),
    address: new FormControl(''),
    phoneNumber: new FormControl('', [Validators.minLength(3), Validators.maxLength(12), Validators.pattern('[0-9]*')]),
    position: new FormControl('', [Validators.required]),
    status: new FormControl('1'),
  });

  // Compare date
  validateDate(control: FormControl): { [key: string]: any } | null {
    const selectedDate = new Date(control.value);
    const currentDate = new Date();

    if (selectedDate > currentDate) {
      return { futureDate: true };
    }

    return null;
  }

  //Submit form
  onSubmit() {
    if (this.addUserF.invalid) {
      return;
    }
    console.log(this.addUserF.value);
    const user: AddUser = {
      username: this.addUserF.value.username,
      fullName: this.addUserF.value.fullName,
      email: this.addUserF.value.email,
      phoneNumber: this.addUserF.value.phoneNumber,
      dob: this.addUserF.value.dob,
      address: this.addUserF.value.address,
      departmentId: this.addUserF.value.department,
      positionId: this.addUserF.value.position,
      roleId: this.addUserF.value.role,
      status: this.addUserF.value.status,
      notes: this.addUserF.value.notes,
    };
    if(this.isEdit){
      this.httpService.editUser(this.userId, user).subscribe(() =>{
        console.log('success');
        this.router.navigateByUrl("/user-list");
      });
    }else{
      this.httpService.addNewUser(user).subscribe(() => {
        this.toastr.success('Successfully created user', 'Financial Planning');
        this.router.navigateByUrl("/user-list");
      }, (error: any)  => {
        if (error.status === 400) {
          // Handle bad request error
          console.log('Bad Request Error:', error);
          this.toastr.error('Email is exist: Please check your email', 'Financial Planning');
        } else {
          // Handle other errors
          console.error('Error:', error);
          this.toastr.error('An error occurred while creating user', 'Financial Planning');
        }
      }
      );
    }

  }
}
