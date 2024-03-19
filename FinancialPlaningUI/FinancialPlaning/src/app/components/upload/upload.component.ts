import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';
import { throwError } from 'rxjs';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [MatIconModule, CommonModule],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {
  status: "initial" | "uploading" | "success" | "fail" = "initial"; // Variable to store file status
  file: File | null = null; // Variable to store file

  constructor(private http: HttpClient) { }

  ngOnInit() {
  }

  onClick() 
  {
    const fileInput: HTMLInputElement = document.querySelector(".file-input") as HTMLInputElement;
    fileInput.click();
  }

  onFileChange(event: Event) {
    debugger;
    const target = event.target as HTMLInputElement;
    const file = target.files![0];
    if (file) {
      let fileName = file.name;
      if (fileName.length >= 12) {
        const splitName = fileName.split('.');
        fileName = splitName[0].substring(0, 13) + "... ." + splitName[1];
      }
      this.uploadFile(file, fileName);
    }
  }

  onChange(event: any) {
    const file: File = event.target.files[0];

    if (file) {
      this.status = "initial";
      this.file = file;
    }
  }

  onUpload() {
    if (this.file) {
      const formData = new FormData();
  
      formData.append('file', this.file, this.file.name);
  
      const upload$ = this.http.post("https://httpbin.org/post", formData);
  
      this.status = 'uploading';
  
      upload$.subscribe({
        next: () => {
          this.status = 'success';
        },
        error: (error: any) => {
          this.status = 'fail';
          return throwError(() => error);
        },
      });
    }
  }

  uploadFile(file: File, fileName: string) {
    const formData = new FormData();
    formData.append('file', file, fileName);
    this.http.post<any>('/import-plan', formData).subscribe(
      (response) => {
        // Handle successful upload response
        console.log('Upload successful:', response);
        // Optionally, you can perform actions like updating UI or showing success messages here
      },
      (error) => {
        // Handle upload error
        console.error('Upload error:', error);
        // Optionally, you can perform actions like showing error messages here
      });
  }
}
