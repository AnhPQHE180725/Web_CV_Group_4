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
  template: `
    <div class="apply-dialog">
      <h2>Ứng tuyển vị trí {{data.jobTitle}}</h2>
      
      <div class="form-group">
        <label for="cvOption">Chọn phương thức nộp CV:</label>
        <select id="cvOption" [(ngModel)]="selectedOption" (change)="onOptionChange()">
          <option value="existing">Dùng CV đã cập nhật</option>
          <option value="new">Tải lên CV mới</option>
        </select>
      </div>

      <div class="form-group">
        <label for="introduction">Giới thiệu (tối thiểu 20 ký tự):</label>
        <textarea 
          id="introduction" 
          [(ngModel)]="introduction" 
          rows="4" 
          placeholder="Nhập giới thiệu của bạn"
          (input)="validateIntroduction()"
        ></textarea>
        <span class="error-message" *ngIf="showIntroductionError">
          Giới thiệu phải có ít nhất 20 ký tự
        </span>
      </div>

      <div *ngIf="selectedOption === 'new'" class="upload-section">
        <app-upload-cv [isDialog]="true" (cvUploaded)="onCvUploaded($event)"></app-upload-cv>
      </div>

      <div class="dialog-actions">
        <button class="cancel-btn" (click)="onCancel()">Hủy</button>
        <button class="apply-btn" (click)="onApply()" [disabled]="!isValid()">Ứng tuyển</button>
      </div>
    </div>
  `,
  styles: [`
    .apply-dialog {
      padding: 20px;
      max-width: 500px;
    }

    h2 {
      margin-bottom: 20px;
      color: #333;
    }

    .form-group {
      margin-bottom: 20px;
    }

    label {
      display: block;
      margin-bottom: 8px;
      color: #555;
    }

    select, textarea {
      width: 100%;
      padding: 8px;
      border: 1px solid #ddd;
      border-radius: 4px;
      box-sizing: border-box;
    }

    form-group textarea {
      width: 100%;
    }
    textarea {
      resize: vertical;
      min-height: 100px;
    }

    .dialog-actions {
      display: flex;
      justify-content: flex-end;
      gap: 10px;
      margin-top: 20px;
    }

    button {
      padding: 8px 16px;
      border: none;
      border-radius: 4px;
      cursor: pointer;
    }

    .cancel-btn {
      background-color: #f0f0f0;
      color: #333;
    }

    .apply-btn {
      background-color: #007bff;
      color: white;
    }

    .apply-btn:disabled {
      background-color: #ccc;
      cursor: not-allowed;
    }

    .upload-section {
      margin-top: 20px;
      padding: 15px;
      border: 1px solid #ddd;
      border-radius: 4px;
    }

    .error-message {
      color: #dc3545;
      font-size: 0.875rem;
      margin-top: 5px;
      display: block;
    }
  `]
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
          alert('Ứng tuyển thành công!');
          this.dialogRef.close(true);
        },
        error: (error) => {
          console.error('Lỗi khi ứng tuyển:', error);
          alert('Ứng tuyển thất bại. ' + error.error.message);
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
          alert('Ứng tuyển thất bại. ' + error.error.message);
        }
      });
    }
  }
} 