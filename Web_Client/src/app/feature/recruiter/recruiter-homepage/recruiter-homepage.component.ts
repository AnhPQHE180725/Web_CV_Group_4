import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { Recruitment } from '../../../models/Recruitment';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-recruiter-homepage',
  standalone: true,
  imports: [NgFor, CurrencyPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './recruiter-homepage.component.html',
  styleUrl: './recruiter-homepage.component.css'
})
export class RecruiterHomepageComponent implements OnInit {
  recruitmentForm: FormGroup;
  isEditMode = false;
  selectedRecruitmentId: number | null = null;
  recruitments: Recruitment[] = [];

  constructor(
    private recruitmentService: RecruitmentService,
    private fb: FormBuilder
  ) {
    // ✅ Fixed Initialization Issue
    this.recruitmentForm = this.fb.group({
      id: [null],
      title: ['', Validators.required],
      description: ['', Validators.required],
      salary: [0, [Validators.required, Validators.min(0)]],
      address: ['', Validators.required],
      deadline: ['', Validators.required],
      status: [1, Validators.required],
      experience: ['', Validators.required],
      quantity: [1, [Validators.required, Validators.min(1)]],
      rank: ['', Validators.required],
      type: ['', Validators.required],
      companyId: [null, Validators.required],
      categoryId: [null, Validators.required]
    });
  }

  ngOnInit(): void {
    this.loadRecruitments();
  }

  loadRecruitments(): void {
    this.recruitmentService.getAllRecruitments().subscribe({
      next: (data) => (this.recruitments = data),
      error: (err) => console.error('Error loading recruitments:', err)
    });
  }

  onAddRecruitment(): void {
    if (this.recruitmentForm.valid) {
      const formData = this.recruitmentForm.value;

      console.log('🟡 Data Sent for Add:', formData);
      this.recruitmentService.addRecruitment(formData).subscribe({
        next: () => {
          alert('Recruitment added successfully!');
          this.loadRecruitments();
          this.resetForm();
        },
        error: (error) => console.error('🔴 Error adding recruitment:', error)
      });
    }
  }

  onEditRecruitment(id: number): void {
    this.isEditMode = true;
    this.selectedRecruitmentId = id;

    const selectedRecruitment = this.recruitments.find((r) => r.id === id);
    if (selectedRecruitment) {
      this.recruitmentForm.patchValue(selectedRecruitment);
    }
  }

  onUpdateRecruitment(): void {
    if (this.recruitmentForm.valid && this.selectedRecruitmentId) {
      const formData = this.recruitmentForm.value;

      console.log('🟡 Data Sent for Update:', formData);
      this.recruitmentService.editRecruitment(this.selectedRecruitmentId, formData).subscribe({
        next: () => {
          alert('Recruitment updated successfully!');
          this.loadRecruitments();
          this.resetForm();
        },
        error: (error) => {
          console.error('🔴 Error updating recruitment:', error);
          alert('Failed to update recruitment. See console for details.');
        }
      });
    }
  }

  onDeleteRecruitment(id: number): void {
    if (confirm('Are you sure you want to delete this recruitment?')) {
      this.recruitmentService.deleteRecruitment(id).subscribe({
        next: () => {
          // ✅ Xóa trực tiếp bản ghi khỏi mảng mà không cần load lại dữ liệu
          this.recruitments = this.recruitments.filter(recruitment => recruitment.id !== id);
          alert('Recruitment deleted successfully!');
        },
        error: (error) => {
          console.error('🔴 Error deleting recruitment:', error);
          alert('Failed to delete recruitment. See console for details.');
        }
      });
    }
  }


  // ✅ Improved Form Reset for Consistency
  private resetForm(): void {
    this.recruitmentForm.reset();
    this.isEditMode = false;
    this.selectedRecruitmentId = null;
  }
}
