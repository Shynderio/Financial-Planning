import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, FormsModule, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { TermService } from '../../../services/term.service';
import { CommonModule } from '@angular/common';

import { Router, ActivatedRoute} from '@angular/router';
import { TermViewModel } from '../../../models/termview.model';

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

    this.termService = termService;
    this.termForm = this.fb.group({
      term: [''],
    });
    // this.termForm = this.fb.group({
    //   termName: [{ value: '', disabled: true }],
    //   startDate: [{ value: '', disabled: true }],
    //   duration: [{ value: '', disabled: true }],
    //   endDate: [{ value: '', disabled: true }],
    //   planDueDate: [{ value: '', disabled: true }],
    //   reportDueDate: [{ value: '', disabled: true }],
    // });
  }


  ngOnInit(): void {
    var termId = this.route.snapshot.params['id'];
    this.termForm = this.fb.group({
      termName: [{ value: '', disabled: true }],
      startDate: [{ value: '', disabled: true }],
      duration: [{ value: '', disabled: true }],
      endDate: [{ value: '', disabled: true }],
      planDueDate: [{ value: '', disabled: true }],
      reportDueDate: [{ value: '', disabled: true }],
    });
    this.termService.getTerm(termId).subscribe(
      (termData: TermViewModel) => {
        this.populateForm(termData);
        console.log(termData);
      },
      (error) => {
        console.error('Error fetching term details:', error);
      }
    );
  }

  populateForm(termData: TermViewModel): void {
    this.termForm.patchValue({
      termName: termData.termName.slice(0, 10),
      startDate: termData.startDate.slice(0, 10),
      duration: this.durationReverseMap[termData.duration],
      planDueDate: termData.planDueDate.slice(0, 10),
      reportDueDate: termData.reportDueDate.slice(0, 10),
      // Populate other form controls
    });
    this.updateEndDate();
  }

  durationMap: { [key: string]: number } = {
    '1_month': 1,
    quarter: 3,
    half_year: 6,
  };

  updateEndDate(): void {
    // Update end date based on start date and duration
    var startDate = this.termForm.get('startDate')!.value;
    var duration = this.termForm.get('duration')!.value;
    // if (startDate && duration) {
    var startDateObj = new Date(startDate);
    var endDateObj = new Date(startDateObj);
    endDateObj.setMonth(startDateObj.getMonth() + Number(this.durationMap[duration])); // Convert to number
    console.log("value", endDateObj);
    this.termForm.patchValue({
      endDate: endDateObj.toISOString().slice(0, 10)
    });
    // }
  }


  
  onSubmit() {
    // Handle form submission if needed
  }
}