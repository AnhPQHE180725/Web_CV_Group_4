import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { NgFor, NgIf } from '@angular/common';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { Recruitment } from '../../../models/Recruitment';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { HttpClient } from '@angular/common/http';
import { CompanyService } from '../../../services/Company.service';
import { Company } from '../../../models/Company';
@Component({
  selector: 'app-recruiter-homepage',
  standalone: true,
  imports: [CommonModule, NgFor, ReactiveFormsModule, RouterLink],
  templateUrl: './recruiter-homepage.component.html',
  styleUrl: './recruiter-homepage.component.css'
})
export class RecruiterHomepageComponent implements OnInit {
  recruitmentForm: FormGroup;
  isEditMode = false;
  selectedRecruitmentId: number | null = null;
  recruitments: Recruitment[] = [];
  companyName: string = 'YourCompanyName';
  companyId: number | null = null;
  selectedCompanyName: string = '';
  apiUrl: string = 'https://localhost:7247/api/Recruitment';
  constructor(
    private recruitmentService: RecruitmentService,
    private companyService: CompanyService,
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
      categoryName: ['', Validators.required],
      logo: ['', Validators.required],
    });
  }

  ngOnInit(): void {
    this.companyService.getUserCompanies().subscribe({
      next: (companies) => {
        if (companies && companies.length > 0) {
          // Lặp qua tất cả công ty của user và tải dữ liệu tuyển dụng
          companies.forEach(company => {
            this.loadRecruitments(company.name);
          });
        } else {
          console.error('No companies found for the logged-in user');
        }
      },
      error: (err) => console.error('Error fetching user companies:', err)
    });
  }

  loadRecruitments(companyName: string): void {
    this.recruitmentService.getRecruitmentsByCompanyName(companyName).subscribe({
      next: (data) => {
        if (data && data.length > 0) {
          // Thêm vào mảng recruitments thay vì ghi đè
          this.recruitments = [...this.recruitments, ...data];
        }
        console.log(`Loaded recruitments for ${companyName}:`, data);
      },
      error: (err) => console.error(`Error loading recruitments for ${companyName}:`, err)
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
