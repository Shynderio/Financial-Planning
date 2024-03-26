import { Component, OnInit, inject } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { UserService } from '../../../services/user.service';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule } from '@angular/forms';
import { MatDialog, MatDialogActions, MatDialogClose, MatDialogContent, MatDialogModule, MatDialogRef, MatDialogTitle} from '@angular/material/dialog';
import { concatMap, of } from 'rxjs';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-user-detail',
  standalone: true,
  imports: [ReactiveFormsModule, FormsModule, CommonModule],
  templateUrl: './user-detail.component.html',
  styleUrl: './user-detail.component.css'
})
export class UserDetailComponent implements OnInit {
  userForm: FormGroup;
  pageIndex = 0;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private userService: UserService,
    private dialog: MatDialog,
    private messageBar: MatSnackBar
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
    // Kích hoạt control
    this.userForm.get('status')?.enable();

    // Vô hiệu hóa control
    this.userForm.get('status')?.disable();

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
  openUpdateDialog():void {
    const userId = this.route.snapshot.paramMap.get('id');
    if (!userId) {
        console.error('User ID is null');
        return;
    }

    const currentStatus = this.userForm.get('status')?.value;
    console.log(currentStatus);
    const newStatus = currentStatus == 1 ? 0 : 1;
    console.log("new" + newStatus);

    const updateDialog = this.dialog.open(UpdateUserStatusDialog, {
      width: '400px',
      height: '250px',
    });

    updateDialog
      .afterClosed()
      .pipe(
        concatMap((result) => {
          if (result === 'update') {
            return this.userService.changeUserStatus(userId, newStatus);
          } else {
            return of(null);
          }
        })
      )
      .subscribe((response) => {
        this.messageBar.open(response == 200 ? 'Updated successfully' : 'Something went wrong', 'Close', {
          panelClass: ['success'],
        });
        this.loadUserDetail(userId);
      });
  }
     //Convert date to dd/mm/yyyy
 convertIsoDateToDdMmYyyy(isoDate: string): string {
  if (!isoDate) return '';
  const dateParts = isoDate.split('T')[0].split('-');
  if (dateParts.length !== 3) return isoDate; // Trả về nguyên bản nếu không phải định dạng ISO 8601
  return `${dateParts[2]}/${dateParts[1]}/${dateParts[0]}`;
}

}
@Component({
  selector: 'app-update-user-status',
  standalone: true,
  imports: [ MatDialogActions, MatDialogClose, MatDialogTitle, MatDialogContent],
  templateUrl: '../update-user-status/update-user-status.component.html',
  styleUrl: '../update-user-status/update-user-status.component.css'
})
export class UpdateUserStatusDialog {
  constructor(public dialogRef: MatDialogRef<UpdateUserStatusDialog>) {}
}










