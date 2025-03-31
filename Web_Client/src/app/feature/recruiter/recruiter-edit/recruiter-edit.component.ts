import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { Recruitment } from '../../../models/Recruitment';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
@Component({
  selector: 'app-recruiter-homepage',
  standalone: true,
  imports: [CommonModule, NgFor, CurrencyPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './recruiter-edit.component.html',
  styleUrl: './recruiter-edit.component.css'
})
export class RecruiterEditComponent implements OnInit {
  recruitmentForm: FormGroup;
  isEditMode = false;
  selectedRecruitmentId: number | null = null;
  recruitments: Recruitment[] = [];
  apiUrl: string = 'https://localhost:7247/api/Recruitment';
  constructor(
    private recruitmentService: RecruitmentService,
    private fb: FormBuilder,
    private http: HttpClient,
    private router: Router
  ) {
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
      categoryId: [null, Validators.required],
      companyName: ['', Validators.required],
      categoryName: ['', Validators.required]
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

  onAddRecruitment() {
    const { companyName, categoryName, ...recruitmentData } = this.recruitmentForm.value;
    recruitmentData.salary = parseFloat(recruitmentData.salary);
    recruitmentData.id = undefined;

    this.http.post(`${this.apiUrl}/add-recruitment`, recruitmentData, { responseType: 'text' })
      .subscribe({
        next: (response) => {
          console.log('✅ Server response:', response);
          alert('Recruitment added successfully!');
          this.router.navigate(['/recruiter']);
        },
        error: (error) => {
          console.error('❌ Error adding recruitment:', error);
          alert('Failed to add recruitment. Please try again!');
        }
      });
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
      const formData = {
        ...this.recruitmentForm.value,
        deadline: new Date(this.recruitmentForm.value.deadline).toISOString()
      };

      console.log('🟡 Dữ liệu gửi lên:', formData);
      this.recruitmentService.editRecruitment(this.selectedRecruitmentId, formData).subscribe({
        next: () => {
          alert('Recruitment updated successfully!');
          this.loadRecruitments();
          this.resetForm();
        },
      });
    }
  }
  public resetForm(): void {
    this.recruitmentForm.reset();
    this.isEditMode = false;
    this.selectedRecruitmentId = null;
  }
  goBack() {
    this.router.navigate(['/recruiter/homepage']);
  }
  onSubmit(): void {
    if (this.isEditMode) {
      this.onUpdateRecruitment();
    } else {
      this.onAddRecruitment();
    }
  }
}
