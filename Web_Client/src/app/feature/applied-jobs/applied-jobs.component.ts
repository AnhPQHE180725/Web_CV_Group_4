import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Recruitment } from '../../models/Recruitment';
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
  favoriteJobs: any[] = [];
  paginatedJobs: any[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;  // Số công việc mỗi trang
  totalPages: number = 1;

  appliedJobs: any[] = [];
  pageTitle: string = 'Công việc đã ứng tuyển';

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

  loadAppliedJobs() {
    this.applicationService.getAppliedJobs().subscribe(
      (data) => {
        this.appliedJobs = data;
        console.log(this.appliedJobs);
      },
      (error) => console.error('Error fetching applied jobs:', error)
    );
  }

  // Phân trang
  updatePagination() {
    this.totalPages = Math.ceil(this.favoriteJobs.length / this.recordsPerPage);
    this.paginateJobs();
  }

  // Cập nhật các công việc theo trang
  paginateJobs() {
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    const endIndex = startIndex + this.recordsPerPage;
    this.paginatedJobs = this.favoriteJobs.slice(startIndex, endIndex);
  }

  // Chuyển sang trang kế tiếp
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.paginateJobs();
    }
  }
}
