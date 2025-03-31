import { Component, EventEmitter, Input, Output } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CvService } from '../../services/cv.service';

@Component({
  selector: 'app-upload-cv',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './upload-cv.component.html',
  styleUrls: ['./upload-cv.component.css']

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