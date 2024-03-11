import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { TermService } from '../../../services/term.service';
import { CreateTermModel } from '../../../models/term.model';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-edit-term',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './edit-term.component.html',
  styleUrl: './edit-term.component.css'
})
export class EditTermComponent implements OnInit {
  termForm: FormGroup;
  termService: TermService;
  constructor(private fb: FormBuilder, termService: TermService) {
    this.termForm = this.fb.group({
      term: ['']
    });
    this.termService = termService;
  }
  ngOnInit() {
    this.termForm = this.fb.group({
      termName: ['', Validators.required],
      startDate: ['', Validators.required],
      duration: ['1_month', Validators.required],
      endDate: [{ value: '', disabled: true }],
      planDueDate: ['', [Validators.required, this.planDueDateValidator]],
      reportDueDate: ['', [Validators.required, this.reportDueDateValidator]]
    });

    this.termForm.get('startDate')?.valueChanges.subscribe(() => {
      this.updateEndDate();
    });

    this.termForm.get('duration')?.valueChanges.subscribe(() => {
      this.updateEndDate();
    });
  }

  updateEndDate(): void {
    const startDateControl = this.termForm.get('startDate');
    const durationControl = this.termForm.get('duration');
    const endDateControl = this.termForm.get('endDate');
    if (startDateControl && startDateControl.valid && durationControl && durationControl.valid && endDateControl) {
      const startDate = new Date(startDateControl.value);
      const duration = durationControl.value;
      let monthsToAdd: number;

      switch (duration) {
        case '1_month':
          monthsToAdd = 1;
          break;
        case 'quarter':
          monthsToAdd = 3;
          break;
        case 'half_year':
          monthsToAdd = 6;
          break;
        default:
          monthsToAdd = 0; // Default to 0 if duration is not recognized
          break;
      }

      const endDate = new Date(startDate);
      endDate.setMonth(endDate.getMonth() + monthsToAdd);

      endDateControl.setValue(this.formatDate(endDate));
    }
  }

  planDueDateValidator(control: any): { [key: string]: boolean } | null {
    const planDueDate = new Date(control.value);
    const startDate = new Date(control?.parent?.controls.startDate.value);

    if (isNaN(planDueDate.getTime()) || planDueDate < startDate) {
      return { 'invalidPlanDueDate': true };
    }
    return null;
  }

  getPlanDueDateErrorMessage(): string {
    const planDueDateControl = this.termForm.get('planDueDate');
    if (planDueDateControl?.errors) {
      if (planDueDateControl.errors['required']) {
        return 'Plan Due Date is required';
      }
      if (planDueDateControl.errors['invalidPlanDueDate']) {
        return 'Plan Due Date must be after the Start Date';
      }
    }
    return '';
  }
  reportDueDateValidator(control: any): { [key: string]: boolean } | null {
    const reportDueDate = new Date(control.value);
    const startDate = new Date(control?.parent?.controls.startDate.value);
    const endDate = new Date(control?.parent?.controls.endDate.value);

    if (isNaN(reportDueDate.getTime()) || reportDueDate < startDate || reportDueDate > endDate) {
      return { 'invalidReportDueDate': true };
    }
    return null;
  }

  getReportDueDateErrorMessage(): string {
    const reportDueDateControl = this.termForm.get('reportDueDate');
    if (reportDueDateControl?.errors) {
      if (reportDueDateControl.errors['required']) {
        return 'Report Due Date is required';
      }
      if (reportDueDateControl.errors['invalidReportDueDate']) {
        return 'Report Due Date must be within the Start Date and End Date';
      }
    }
    return '';

  }

  formatDate(date: Date): string {
    return `${date.getFullYear()}-${(date.getMonth() + 1).toString().padStart(2, '0')}-${date.getDate().toString().padStart(2, '0')}`;
  }

  editTerm() {
    const durationMap: { [key: string]: number } = {
      '1_month': 1,
      'quarter': 3,
      'half_year': 6
    };
    const durationValue = this.termForm.get('duration')?.value;
    const duration = durationMap[durationValue];
    const termData = new CreateTermModel({
      termName: this.termForm.get('termName')?.value,
      creatorId: '', // You need to set the creatorId
      duration: duration,
      startDate: this.termForm.get('startDate')?.value,
      planDueDate: this.termForm.get('planDueDate')?.value,
      reportDueDate: this.termForm.get('reportDueDate')?.value
    });
    const termId = ''; // You need to set the termId
    this.termService.updateTerm(termId, termData).subscribe((response) => {
      console.log(response);
    });
  }

  onSubmit(): void {
    if (this.termForm.valid) {
      // Submit form data
      const startDate = new Date(this.termForm.get('startDate')?.value);
      const endDate = new Date(this.termForm.get('endDate')?.value);
      const planDueDate = new Date(this.termForm.get('planDueDate')?.value);
      const reportDueDate = new Date(this.termForm.get('reportDueDate')?.value);

      if (planDueDate < startDate || planDueDate > endDate) {
        // Plan due date is not within the range
        // Handle error or display message
        console.log('Plan due date is not within the range.');
        return;
      }

      if (reportDueDate < startDate || reportDueDate > endDate) {
        // Report due date is not within the range
        // Handle error or display message
        console.log('Report due date is not within the range.');
        return;
      }
      console.log(this.termForm.value);
      // Call the service to create the term
      this.editTerm();

    } else {
      // Mark all fields as touched to trigger validation messages
      this.termForm.markAllAsTouched();
    }
  }

  onCancel(): void {
    // Handle cancel action
    console.log('Cancel');
  }

}
