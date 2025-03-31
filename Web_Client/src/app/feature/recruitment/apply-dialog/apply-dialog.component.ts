import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApplicationService } from '../../../services/application.service';
import { MatDialogRef, MAT_DIALOG_DATA, MatDialogModule } from '@angular/material/dialog';
import { UploadCvComponent } from '../../upload-cv/upload-cv.component';

@Component({
  selector: 'app-apply-dialog',
  standalone: true,
  imports: [CommonModule, FormsModule, UploadCvComponent, MatDialogModule],
  templateUrl: './apply-dialog.component.html',
  styleUrl: './apply-dialog.component.css'
})
export class ApplyDialogComponent {
  selectedOption: 'existing' | 'new' = 'existing';
  introduction: string = '';
  newCvFile: File | null = null;
  showIntroductionError = false;

  private dialogRef = inject(MatDialogRef<ApplyDialogComponent>);

  private applicationService = inject(ApplicationService);
  data: { jobTitle: string, recruitmentId: number } = inject(MAT_DIALOG_DATA);

  onOptionChange() {
    this.newCvFile = null;
  }

  onCvUploaded(file: File) {
    this.newCvFile = file;
  }

  validateIntroduction() {
    this.showIntroductionError = this.introduction.length < 20;
  }

  isValid(): boolean {
    if (this.introduction.length < 20) {
      return false;
    }
    if (this.selectedOption === 'new') {
      return this.newCvFile !== null;
    }
    return true;
  }

  onCancel() {
    this.dialogRef.close();
  }

  onApply() {
    if (this.selectedOption === 'existing') {
      this.applicationService.applyWithExistingCV(
        this.data.recruitmentId,
        this.introduction
      ).subscribe({
        next: () => {
          alert('Bạn đã ứng tuyển thành công!');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Lỗi khi ứng tuyển:', error);
          alert(error.error.message);
        }
      });
    } else if (this.selectedOption === 'new' && this.newCvFile) {
      this.applicationService.applyWithNewCV(
        this.data.recruitmentId,
        this.newCvFile,
        this.introduction
      ).subscribe({
        next: () => {
          alert('Ứng tuyển thành công!');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Lỗi khi ứng tuyển:', error);
          alert(error.error.message);
        }
      });
    }
  }
} 