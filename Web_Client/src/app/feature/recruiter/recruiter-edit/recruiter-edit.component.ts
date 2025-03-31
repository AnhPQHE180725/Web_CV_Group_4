import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { Recruitment } from '../../../models/Recruitment';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-recruiter-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './recruiter-edit.component.html',
  styleUrls: ['./recruiter-edit.component.css']
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
    private router: Router,
    private route: ActivatedRoute
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
    this.route.paramMap.subscribe(params => {
      const id = params.get('id');
      if (id) {
        this.isEditMode = true;
        this.selectedRecruitmentId = +id;
        this.loadRecruitmentById(this.selectedRecruitmentId);
      }
    });
  }

  loadRecruitments(): void {
    this.recruitmentService.getAllRecruitments().subscribe({
      next: (data) => (this.recruitments = data),
      error: (err) => console.error('❌ Error loading recruitments:', err)
    });
  }

  onAddRecruitment() {
    if (this.recruitmentForm.invalid) return;

    const { companyName, categoryName, ...recruitmentData } = this.recruitmentForm.value;
    recruitmentData.deadline = new Date(recruitmentData.deadline).toISOString();
    recruitmentData.id = undefined;

    this.http.post(`${this.apiUrl}/add-recruitment`, recruitmentData, { responseType: 'text' })
      .subscribe({
        next: () => {
          alert('✅ Recruitment added successfully!');
          this.loadRecruitments();
          this.router.navigate(['/recruiter']);
        },
        error: (error) => {
          console.error('❌ Error adding recruitment:', error);
          alert('Failed to add recruitment. Please try again!');
        }
      });
  }
  loadRecruitmentById(id: number): void {
    this.recruitmentService.getRecruitmentById(id).subscribe({
      next: (recruitment) => {
        this.recruitmentForm.patchValue(recruitment);
      },
      error: (err) => console.error('Error loading recruitment:', err)
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

  onUpdateRecruitment() {
    if (this.recruitmentForm.invalid || !this.selectedRecruitmentId) return;

    const { companyName, categoryName, ...recruitmentData } = this.recruitmentForm.value;
    recruitmentData.deadline = new Date(recruitmentData.deadline).toISOString();

    this.http.put(`${this.apiUrl}/edit-recruitment/${this.selectedRecruitmentId}`, recruitmentData, { responseType: 'text' })
      .subscribe({
        next: () => {
          alert('✅ Recruitment updated successfully!');
          this.loadRecruitments();
          this.router.navigate(['/recruiter']);
        },
        error: (error) => {
          console.error('❌ Error updating recruitment:', error);
          alert('Failed to update recruitment. Please try again!');
        }
      });
  }


  resetForm(): void {
    this.recruitmentForm.reset();
    this.isEditMode = false;
    this.selectedRecruitmentId = null;
  }

  goBack() {
    this.router.navigate(['/recruiter']);
  }

  onSubmit(): void {
    if (this.isEditMode) {
      this.onUpdateRecruitment();
    } else {
      this.onAddRecruitment();
    }
  }
}
