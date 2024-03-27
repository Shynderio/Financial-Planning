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
  ) { }


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
    return status === '1' ? 'Active' : 'Inactive';
  }
 
  getUserById(userId: string) {
    if (userId) {
      this.isEdit = true;
      this.userId = userId;
      
      this.httpService.getUserById(userId).subscribe({
        next: (userDetail: any) => {
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
            dob: this.convertDdMmYyyyToIsoDate(userDetail.dob),
            status: userDetail.status
           
          });
        },
        error: (error: any) => {
          console.log('Lỗi khi lấy thông tin người dùng:', error);
        }
      });
    } else {
      console.error('Invalid userId:', userId);
    }
  }
  //Get list departments
  getDepartments() {
    this.httpService.getAllDepartment().subscribe(
      (data: IDepartment[]) => {
        this.departments = data;
      },
      (error: any) => {
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
      (error: any) => {
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
      (error: any) => {
        console.log('Error fetching departments:', error);
      }
    );
  }

  // Validate data

  addUserF: FormGroup = new FormGroup({
    username: new FormControl(''),
    fullName: new FormControl('', [Validators.required, this.validateName, this.validateSpaces]),
    email: new FormControl('', [Validators.required, Validators.email, this.validateSpaces]),
    dob: new FormControl('', [Validators.required, this.validateDate]),
    department: new FormControl('', [Validators.required]),
    role: new FormControl('', [Validators.required]),
    notes: new FormControl(''),
    address: new FormControl(''),
    phoneNumber: new FormControl('', [Validators.minLength(3), Validators.maxLength(12), Validators.pattern('[0-9]*')]),
    position: new FormControl('', [Validators.required]),
    status: new FormControl('1'),
  });

  validateName(control: FormControl): { [key: string]: any } | null {
    const vietnameseCharactersRegex = /[^\x00-\x7F]+/; // Biểu thức chính quy để kiểm tra ký tự tiếng Việt
    const containsNumbers = /\d/.test(control.value); // Kiểm tra xem có chứa số không

    if (vietnameseCharactersRegex.test(control.value)) {
      return { containsVietnamese: true }; // Có chứa ký tự tiếng Việt
    }
    if (containsNumbers) {
      return { containsNumber: true }; // Có chứa số
    }

    return null;
  }
  validateSpaces(control: FormControl): { [key: string]: any } | null {
    const value = control.value;

    // Kiểm tra nếu có nhiều hơn một khoảng trắng giữa các từ hoặc có khoảng trắng ở đầu hoặc cuối
    if (/^\s|\s\s+|\s$/.test(value)) {
      return { excessSpaces: true };
    }

    return null;
  }

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
      dob: this.convertIsoDateToDdMmYyyy(this.addUserF.value.dob),
      address: this.addUserF.value.address,
      departmentId: this.addUserF.value.department,
      positionId: this.addUserF.value.position,
      roleId: this.addUserF.value.role,
      status: this.addUserF.value.status,
      notes: this.addUserF.value.notes,
    };
    if (this.isEdit) {
      this.httpService.editUser(this.userId, user).subscribe(() => {
        console.log('success');
        this.toastr.success('Updated user successful', 'Financial Planning');
        this.router.navigateByUrl("/user-list");
      });
    } else {
      this.httpService.addNewUser(user).subscribe(() => {
        this.toastr.success('Successfully created user', 'Financial Planning');
        this.router.navigateByUrl("/user-list");
      }, (error: any) => {
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
  //Convert date to dd/mm/yyyy
  convertIsoDateToDdMmYyyy(isoDate: string): string {
    if (!isoDate) return '';
    const dateParts = isoDate.split('T')[0].split('-');
    if (dateParts.length !== 3) return isoDate; // Trả về nguyên bản nếu không phải định dạng ISO 8601
    return `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;
  }
  convertDdMmYyyyToIsoDate(ddMmYyyyDate: string): string {
    if (!ddMmYyyyDate) return '';
    const dateParts = ddMmYyyyDate.split('/');
    if (dateParts.length !== 3) return ddMmYyyyDate; // Trả về nguyên bản nếu không phải định dạng dd/mm/yyyy
  
    const yyyy = dateParts[2];
    const mm = dateParts[1].padStart(2, '0'); // Đảm bảo mm luôn có 2 chữ số
    const dd = dateParts[0].padStart(2, '0'); // Đảm bảo dd luôn có 2 chữ số
  
    return `${yyyy}-${mm}-${dd}`;
  }
}
