import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { TermService } from '../../../services/term.service';
import { CommonModule } from '@angular/common';
// import { ActivatedRoute } from '@angular/router';
import { switchMap } from 'rxjs/operators';
import { Router, ActivatedRoute, ParamMap } from '@angular/router';

@Component({
  selector: 'app-edit-term',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './term-details.component.html',
  styleUrl: './term-details.component.css'
})

export class TermDetailsComponent implements OnInit {
  termForm: FormGroup;

  constructor(
    private fb: FormBuilder,
    private route: ActivatedRoute,
    private router: Router,
    private termService: TermService
  ) {
    this.termForm = this.fb.group({
      termName: [{ value: '', disabled: true }],
      startDate: [{ value: '', disabled: true }],
      duration: [{ value: '', disabled: true }],
      endDate: [{ value: '', disabled: true }],
      planDueDate: [{ value: '', disabled: true }],
      reportDueDate: [{ value: '', disabled: true }],
    });
  }


  ngOnInit(): void {
    this.route.params.subscribe(params => {
      const termId = params['id']; // Assuming 'id' is the parameter name
      this.loadTermDetails(termId);
    });
  }

  loadTermDetails(termId: string): void {
    this.termService.getTerm(termId).subscribe(
      (termDetails: any) => {
        // Assuming termDetails contains the required data
        this.termForm.patchValue({
          termName: termDetails.termName,
          startDate: termDetails.startDate,
          duration: termDetails.duration,
          endDate: termDetails.endDate,
          planDueDate: termDetails.planDueDate,
          reportDueDate: termDetails.reportDueDate
        });
      },
      error => {
        // Handle error
        console.error('Error fetching term details:', error);
      }
    );
  }

  onSubmit() {
    // Handle form submission if needed
  }
}