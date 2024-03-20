import { Component, OnInit } from '@angular/core';
import { UploadComponent } from '../../../components/upload/upload.component';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatOption, MatSelect } from '@angular/material/select';
import { CommonModule } from '@angular/common';
import { TermService } from '../../../services/term.service';
import e from 'express';

@Component({
  selector: 'app-import-plan',
  standalone: true,
  imports: [
    UploadComponent,
    MatFormFieldModule,
    MatSelect,
    MatOption, CommonModule],
  templateUrl: './import-plan.component.html',
  styleUrls: ['./import-plan.component.css']
})
export class ImportPlanComponent implements OnInit {
  termService: TermService;
  termOptions: {value: string, viewValue: string}[] = [];
  // ];
  constructor(termService: TermService) {
    this.termService = termService;
  }

  ngOnInit() {
    this.termService.getStartedTerms().subscribe(
      (data: any[]) =>{
        this.termOptions = data.map(term => {
          return {value: term.id, viewValue: term.termName};
        });
        // console.log(this.termOptions);
      },
      error => {
        console.log(error);
      }
      );
  };


  onFileSelected(event: any) {
    debugger;
    const file: File = event;
    if (file) {
      // Handle the selected file here, for example:
      console.log('Selected file:', file);
      // You can trigger the file upload process here if needed
      // this.uploadFile(file);
    }
  }

}
