import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { CurrencyPipe, NgFor, NgIf } from '@angular/common';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { Recruitment } from '../../../models/Recruitment';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-recruiter-homepage',
  standalone: true,
  imports: [CommonModule, NgFor, CurrencyPipe, ReactiveFormsModule, RouterLink],
  templateUrl: './recruiter-homepage.component.html',
  styleUrl: './recruiter-homepage.component.css'
})
export class RecruiterHomepageComponent implements OnInit {
  recruitmentForm: FormGroup;
  isEditMode = false;
  selectedRecruitmentId: number | null = null;
  recruitments: Recruitment[] = [];
  apiUrl: string = 'https://localhost:7247/api/Recruitment';
  constructor(
    private recruitmentService: RecruitmentService,
    private fb: FormBuilder,
    private http: HttpClient
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

  onDeleteRecruitment(id: number): void {
    if (confirm('Are you sure you want to delete this recruitment?')) {
      this.recruitmentService.deleteRecruitment(id).subscribe({
        next: () => {
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

  public resetForm(): void {
    this.recruitmentForm.reset();
    this.isEditMode = false;
    this.selectedRecruitmentId = null;
  }
}
