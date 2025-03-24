import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CvService } from '../../services/cv.service';

@Component({
  selector: 'app-upload-cv',
  standalone: true,
  imports: [CommonModule],
  template: `
    <div class="upload-area" (dragover)="onDragOver($event)" (drop)="onDrop($event)">
      <p>Kéo thả file PDF vào đây</p>
      <p>Hoặc</p>
      <div class="file-input-container">
        <input type="file" id="cv-file" accept=".pdf" (change)="onFileSelected($event)" class="file-input">
        <label for="cv-file" class="choose-file-btn">Chọn file</label>
      </div>
      <p *ngIf="selectedFile" class="selected-file">File đã chọn: {{ selectedFile.name }}</p>
      
      <!-- Chỉ hiển thị nút upload khi không ở chế độ dialog -->
      <button *ngIf="!isDialog" class="upload-btn" (click)="onUpload()" [disabled]="!selectedFile">
        Upload CV
      </button>
    </div>
  `,
  styles: [`
    .upload-area {
      border: 2px dashed #ccc;
      border-radius: 8px;
      padding: 30px;
      text-align: center;
      background-color: #fafafa;
      transition: border-color 0.3s ease;
    }

    .upload-area:hover {
      border-color: #007bff;
    }

    .file-input-container {
      margin: 20px 0;
    }

    .file-input {
      display: none;
    }

    .choose-file-btn {
      background-color: #007bff;
      color: white;
      padding: 10px 20px;
      border-radius: 5px;
      cursor: pointer;
      display: inline-block;
      transition: background-color 0.3s ease;
    }

    .choose-file-btn:hover {
      background-color: #0056b3;
    }

    .selected-file {
      margin-top: 10px;
      color: #666;
    }

    .upload-btn {
      background-color: #007bff;
      color: white;
      padding: 10px 20px;
      border-radius: 5px;
      cursor: pointer;
      display: inline-block;
      transition: background-color 0.3s ease;
      margin-top: 20px;
    }

    .upload-btn:hover {
      background-color: #0056b3;
    }
  `]
})
export class UploadCvComponent {
  @Input() isDialog = false;
  @Output() cvUploaded = new EventEmitter<File>();
  selectedFile: File | null = null;

  constructor(private cvService: CvService) { }

  onDragOver(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();
  }

  onDrop(event: DragEvent) {
    event.preventDefault();
    event.stopPropagation();

    const files = event.dataTransfer?.files;
    if (files && files.length > 0) {
      this.handleFile(files[0]);
    }
  }

  onFileSelected(event: Event) {
    const input = event.target as HTMLInputElement;
    if (input.files && input.files.length > 0) {
      this.handleFile(input.files[0]);
    }
  }

  private handleFile(file: File) {
    if (file.type === 'application/pdf') {
      this.selectedFile = file;
      if (this.isDialog) {
        this.cvUploaded.emit(file);
      }
    } else {
      alert('Vui lòng chọn file PDF!');
    }
  }

  onUpload() {
    if (this.selectedFile) {
      this.cvService.uploadCV(this.selectedFile).subscribe({
        next: () => {
          alert('Upload CV thành công!');
          this.selectedFile = null;
          const fileInput = document.querySelector('input[type="file"]') as HTMLInputElement;
          if (fileInput) fileInput.value = '';
        },
        error: (error) => {
          console.error('Upload error:', error);
          alert('Upload CV thất bại. Vui lòng thử lại!');
        }
      });
    } else {
      alert('Vui lòng chọn file PDF trước khi upload!');
    }
  }
} 