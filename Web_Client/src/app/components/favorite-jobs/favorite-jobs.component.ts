import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/User.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApplyDialogComponent } from '../../feature/recruitment/apply-dialog/apply-dialog.component';
import { JobFollowService } from '../../services/job-follow.service';

@Component({
  selector: 'app-favorite-jobs',
  templateUrl: './favorite-jobs.component.html',
  imports: [CommonModule, MatDialogModule],
  standalone: true,
  styleUrls: ['./favorite-jobs.component.css']
})
export class FavoriteJobsComponent implements OnInit {
  favoriteJobs: any[] = [];
  paginatedJobs: any[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;  // Số công việc mỗi trang
  totalPages: number = 1;

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

  private loadFavoriteJobs(): void {
    this.userService.getFavoriteJobs().subscribe({
      next: (jobs) => {
        this.favoriteJobs = jobs;
        this.updatePagination();
      },
      error: (error) => {
        console.error('Error loading favorite jobs:', error);
      }
    });
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

  toggleJobFollow(event: Event, jobId: number) {
    event.stopPropagation();
    this.jobFollowService.toggleFollow(jobId)
      .subscribe(response => {
        this.loadFavoriteJobs(); // Reload the list after toggling
      });
  }

  isJobFollowed(jobId: number): boolean {
    return true; // Always true since this is the favorites list
  }
}
