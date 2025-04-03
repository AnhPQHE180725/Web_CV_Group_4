import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { Recruitment } from '../../../models/Recruitment';
import { Company } from '../../../models/Company';
import { CompanyService } from '../../../services/Company.service';
import { Category } from '../../../models/Category';
import { CategoryService } from '../../../services/Category.service';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { forkJoin } from 'rxjs';
@Component({
  selector: 'app-recruiter-edit',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './recruiter-edit.component.html',
  styleUrls: ['./recruiter-edit.component.css']
})
export class RecruiterEditComponent implements OnInit {
  recruitmentForm: FormGroup;
  companyList: { id: number, name: string }[] = [];
  isEditMode = false;
  selectedRecruitmentId: number | null = null;
  recruitments: Recruitment[] = [];
  categoryList: Category[] = [];
  apiUrl: string = 'https://localhost:7247/api/Recruitment';

  constructor(
    private recruitmentService: RecruitmentService,
    private fb: FormBuilder,
    private companyService: CompanyService,
    private categoryService: CategoryService,
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
      rank: ['AA', Validators.required],
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

        // Load tất cả dữ liệu cùng lúc
        forkJoin({
          companies: this.companyService.getUserCompanies(),
          categories: this.categoryService.getAllCategories(),
          recruitment: this.recruitmentService.getRecruitmentById(this.selectedRecruitmentId)
        }).subscribe(({ companies, categories, recruitment }) => {
          this.companyList = companies;
          this.categoryList = categories;

          // Tìm công ty & danh mục theo ID
          const selectedCompany = companies.find(c => c.id === recruitment.companyId);
          const selectedCategory = categories.find(cat => cat.id === recruitment.categoryId);

          // Cập nhật form với giá trị ban đầu
          this.recruitmentForm.patchValue({
            ...recruitment,
            companyName: selectedCompany ? selectedCompany.name : '',
            categoryName: selectedCategory ? selectedCategory.name : ''
          });
        }, error => console.error('❌ Error loading data:', error));
      } else {
        // Nếu không phải chế độ chỉnh sửa thì chỉ load danh sách
        this.loadCategories();
        this.loadCompanies();
      }
    });
  }
  loadRecruitments(): void {
    this.recruitmentService.getAllRecruitments().subscribe({
      next: (data) => (this.recruitments = data),
      error: (err) => console.error('❌ Error loading recruitments:', err)
    });
  }
  loadCompanies(): void {
    this.companyService.getUserCompanies().subscribe({
      next: (data) => (this.companyList = data),
      error: (err) => console.error('❌ Error loading companies:', err)
    });
  }
  onCompanyChange(event: any) {
    const selectedCompanyName = event.target.value;
    const selectedCompany = this.companyList.find(c => c.name === selectedCompanyName);

    if (selectedCompany) {
      this.recruitmentForm.get('companyId')?.setValue(selectedCompany.id);
    }
  }
  loadCategories(): void {
    this.categoryService.getAllCategories().subscribe({
      next: (data) => (this.categoryList = data),
      error: (err) => console.error('❌ Error loading categories:', err)
    });
  }

  // 🔹 Khi chọn danh mục, tự động cập nhật ID
  onCategoryChange(event: any) {
    const selectedCategoryName = event.target.value;
    const selectedCategory = this.categoryList.find(c => c.name === selectedCategoryName);

    if (selectedCategory) {
      this.recruitmentForm.get('categoryId')?.setValue(selectedCategory.id);
    }
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

        // Tìm công ty & danh mục tương ứng để hiển thị đúng tên
        const selectedCompany = this.companyList.find(c => c.id === recruitment.companyId);
        const selectedCategory = this.categoryList.find(cat => cat.id === recruitment.categoryId);

        this.recruitmentForm.patchValue({
          companyName: selectedCompany ? selectedCompany.name : '',
          categoryName: selectedCategory ? selectedCategory.name : ''
        });
      },
      error: (err) => console.error('❌ Error loading recruitment:', err)
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
