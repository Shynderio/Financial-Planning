import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarRef } from '@angular/material/snack-bar';
import { MESSAGE_CONSTANTS } from '../../../constants/message.constants';
import { MatProgressSpinner } from '@angular/material/progress-spinner';
import { MessageBarComponent } from '../message-bar/message-bar.component';

@Component({
  selector: 'app-upload',
  standalone: true,
  imports: [MatIconModule, CommonModule, MatCardModule, MatProgressSpinner],
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.css'],
})
export class UploadComponent implements OnInit {
  @Output() change = new EventEmitter<any>();
  @Output() fileSelected: EventEmitter<File> = new EventEmitter<File>();
  @Input() isTermSelected: boolean = true; // Variable to store validName
  @Input() isMonthSelected: boolean = true; // Variable to store validName
  @Input() validName: string = ''; // Variable to store validName
  @Input() loading: boolean = false;
  file: File | null = null; // Variable to store file
  progress?: number; // Variable to store progress

  constructor(private http: HttpClient, private messagebar: MatSnackBar) { }
  ngOnInit() { }

  reset() {
    this.file = null;
  }

  onClick() {
    if (this.isTermSelected == false) {

      this.messagebar.openFromComponent(MessageBarComponent, {
        duration: 5000,
        data: {
          rmclose: true,
          message: 'Please select term first',
        },
      });
      return;
    }
    if (this.isMonthSelected == false) {
      this.messagebar.openFromComponent(MessageBarComponent, {
        duration: 5000,
        data: {
          rmclose: true,
          message: 'Please select month first',
        },
      });
      return;
    }
    const fileInput: HTMLInputElement = document.querySelector(
      '.file-input'
    ) as HTMLInputElement;
    fileInput.click();
  }

  onChange(event: any) {
    const file: File = event.target.files[0];
    if (file) {
      if (file.name.split('.')[0] != this.validName) {
        this.messagebar.openFromComponent(MessageBarComponent, {
          duration: 5000,
          data: {
            message: MESSAGE_CONSTANTS.ME016,
          },
        });
        return;
      }

      if (file.size > 500 * 1024 * 1024) {
        this.messagebar.openFromComponent(MessageBarComponent, {
          duration: 5000,
          data: {
            message: MESSAGE_CONSTANTS.ME017,
          },
        });
        return;
      }

      this.file = file;
      this.loadForm(file, file.name);
      this.fileSelected.emit(file);
      this.change.emit(file);
    }
  }

  loadForm(file: File, fileName: string) {
    const formData = new FormData();
    formData.append('file', file, fileName);
  }
}
