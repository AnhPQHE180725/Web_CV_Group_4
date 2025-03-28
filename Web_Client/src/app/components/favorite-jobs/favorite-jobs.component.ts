import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/User.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApplyDialogComponent } from '../../feature/recruitment/apply-dialog/apply-dialog.component';
import { JobFollowService } from '../../services/job-follow.service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-favorite-jobs',
  templateUrl: './favorite-jobs.component.html',
  imports: [CommonModule, MatDialogModule, FormsModule],
  standalone: true,
  styleUrls: ['./favorite-jobs.component.css']
})
export class FavoriteJobsComponent implements OnInit {
  favoriteJobs: any[] = [];
  filteredJobs: any[] = []; // Danh sách sau khi tìm kiếm
  paginatedJobs: any[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;
  totalPages: number = 1;

  // Biến tìm kiếm
  searchQuery: string = '';
  salaryQuery: string = '';

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private dialog: MatDialog,
    private jobFollowService: JobFollowService
  ) { }

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadFavoriteJobs();
  }

  toggleJobFollow(event: Event, jobId: number) {
    event.stopPropagation();
    this.jobFollowService.toggleFollow(jobId)
      .subscribe(response => {
        this.loadFavoriteJobs(); // Reload the list after toggling
      });
  }

  // Kiểm tra trạng thái đăng nhập
  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  onApply(recruitment: any) {
    this.dialog.open(ApplyDialogComponent, {
      width: '500px',
      data: {
        jobTitle: recruitment.title,
        recruitmentId: recruitment.id
      }
    });
  }


  private loadFavoriteJobs(): void {
    this.userService.getFavoriteJobs().subscribe({
      next: (jobs) => {
        this.favoriteJobs = jobs;
        this.filteredJobs = [...jobs]; // Sao chép danh sách gốc
        this.updatePagination();
      },
      error: (error) => {
        console.error('Error loading favorite jobs:', error);
      }
    });
  }

  // Hàm tìm kiếm theo tên hoặc lương
  searchJobs(): void {
    const searchSalary = this.salaryQuery ? parseFloat(this.salaryQuery) : null;
    const searchText = this.searchQuery.toLowerCase().trim();

    this.filteredJobs = this.favoriteJobs.filter(job => {
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
