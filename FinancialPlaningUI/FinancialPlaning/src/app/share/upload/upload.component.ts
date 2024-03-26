import { CommonModule } from '@angular/common';
import { HttpClient, HttpEventType } from '@angular/common/http';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { catchError, map, throwError } from 'rxjs';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [MatIconModule, CommonModule, MatCardModule],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  @Output() fileSelected: EventEmitter<File> = new EventEmitter<File>();

  file: File | null = null; // Variable to store file
  progress?: number ; // Variable to store progress

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  onClick() 
  {
    const fileInput: HTMLInputElement = document.querySelector(".file-input") as HTMLInputElement;
    fileInput.click();
  }

  onChange(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      // this.status = "initial";
      this.file = file;
      this.loadForm(file, file.name);
      // debugger;
      this.fileSelected.emit(file);
    }
  }

  loadForm(file: File, fileName: string) {
    const formData = new FormData();
    formData.append('file', file, fileName);
  }
}
