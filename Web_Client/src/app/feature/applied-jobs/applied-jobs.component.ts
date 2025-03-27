import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ApplicationService } from '../../services/application.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-applied-jobs',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './applied-jobs.component.html',
  styleUrl: './applied-jobs.component.css'
})
export class AppliedJobsComponent implements OnInit {
  appliedJobs: any[] = [];
  filteredJobs: any[] = []; // Danh sách sau khi tìm kiếm
  paginatedJobs: any[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;
  totalPages: number = 1;

  // Biến tìm kiếm
  searchQuery: string = '';
  salaryQuery: string = '';

  constructor(
    private applicationService: ApplicationService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadAppliedJobs();
  }

  private loadAppliedJobs() {
    this.applicationService.getAppliedJobs().subscribe(
      (data) => {
        this.appliedJobs = data;
        this.filteredJobs = [...data]; // Sao chép danh sách gốc
        this.updatePagination();
      },
      (error) => console.error('Lỗi khi tải danh sách công việc đã ứng tuyển:', error)
    );
  }

  // Hàm tìm kiếm theo tên hoặc lương
  searchJobs(): void {
    const searchSalary = this.salaryQuery ? parseFloat(this.salaryQuery) : null;
    const searchText = this.searchQuery.toLowerCase().trim();

    this.filteredJobs = this.appliedJobs.filter(job => {
      const matchesTitle = searchText ? job.recruitment.title.toLowerCase().includes(searchText) : true;
      const matchesSalary = searchSalary !== null ? parseFloat(job.recruitment.salary) === searchSalary : true;
      return matchesTitle && matchesSalary;
    });

    this.updatePagination();
  }

  // Cập nhật phân trang
  updatePagination(): void {
    this.totalPages = Math.ceil(this.filteredJobs.length / this.recordsPerPage);
    this.paginateJobs();
  }

  // Cập nhật danh sách theo trang
  paginateJobs(): void {
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    const endIndex = startIndex + this.recordsPerPage;
    this.paginatedJobs = this.filteredJobs.slice(startIndex, endIndex);
  }

  // Chuyển trang
  goToPage(page: number): void {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.paginateJobs();
    }
  }
}
