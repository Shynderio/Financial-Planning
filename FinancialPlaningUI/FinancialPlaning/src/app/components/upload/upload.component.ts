import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [MatIconModule],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css']
})
export class UploadComponent implements OnInit {

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
      // this.uploadFile(fileName);
    }
  }

  uploadFile(file: File, fileName: string) {
    //
    // );
  }
}
