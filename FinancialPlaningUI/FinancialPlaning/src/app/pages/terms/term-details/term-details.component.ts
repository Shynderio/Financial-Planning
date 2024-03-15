import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { TermService } from '../../../services/term.service';
import { CommonModule } from '@angular/common';

import { Router, ActivatedRoute, ParamMap } from '@angular/router';
import { report } from 'process';

@Component({
  selector: 'app-edit-term',
  standalone: true,
  imports: [FormsModule, ReactiveFormsModule, CommonModule],
  templateUrl: './term-details.component.html',
  styleUrl: './term-details.component.css'
})

export class TermDetailsComponent implements OnInit {
  termForm: FormGroup;

  durationReverseMap: { [key: number]: string } = {
    1: '1_month',
    3: 'quarter',
    6: 'half_year'
  };
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
    }
    );
  }

  loadTermDetails(termId: string): void {
    // debugger;
    this.termService.getTerm(termId).subscribe({
      next: (termDetails: any) => {
        // Assuming termDetails contains the required data
        this.termForm.patchValue({
          termName: termDetails.termName,
          startDate: termDetails.startDate.slice(0, 10),
          duration: this.durationReverseMap[termDetails.duration],
    
          endDate: '',
    
          planDueDate: termDetails.planDueDate.slice(0, 10),
          reportDueDate: termDetails.reportDueDate.slice(0, 10),
        });
        console.log(termDetails);
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
}