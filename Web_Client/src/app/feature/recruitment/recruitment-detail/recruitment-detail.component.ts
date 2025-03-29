import { Component, OnInit } from '@angular/core';
import { Recruitment } from '../../../models/Recruitment';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { CommonModule } from '@angular/common';
import { ApplyDialogComponent } from '../apply-dialog/apply-dialog.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AuthService } from '../../../services/auth.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Location } from '@angular/common';
import { JobFollowService } from '../../../services/job-follow.service';


@Component({
  selector: 'app-recruitment-detail',
  imports: [CommonModule, MatDialogModule, RouterLink],
  templateUrl: './recruitment-detail.component.html',
  styleUrl: './recruitment-detail.component.css'
})
export class RecruitmentDetailComponent implements OnInit {
  recruitment: any;
  id: number = 0;
  followedJobs: number[] = [];
  
  constructor(
    private recruitmentService: RecruitmentService, 
    private route: ActivatedRoute,
    private dialog: MatDialog, 
    private authService: AuthService,
    private location: Location, 
    private jobFollowService: JobFollowService,
  ) { }

  ngOnInit() {
    // Đầu tiên, kiểm tra xem người dùng đã đăng nhập chưa
    if (this.isLoggedIn() && this.isCandidate()) {
      // Nếu đã đăng nhập, tải danh sách công việc đã yêu thích
      this.loadFollowedJobs();
    }
    
    this.route.paramMap.subscribe(params => {
      this.id = Number(params.get('id'));

      if (!this.id) {
        console.warn('Missing postid or userid in the URL.');
        return;
      }

      console.log('id:', this.id);

      this.recruitmentService.getRecruitmentById(this.id).subscribe(
        (data) => {
          console.log('Fetched recruitment data:', data);
          this.recruitment = data; // Assign API response to recruitment
        },
        (error) => {
          console.error('Error fetching recruitment details:', error);
        }
      );
    });
  }

  onApply(recruitment: Recruitment) {
    this.dialog.open(ApplyDialogComponent, {
      width: '500px',
      data: {
        jobTitle: recruitment.title,
        recruitmentId: recruitment.id
      }
    });
  }

  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  isCandidate(): boolean {
    return this.authService.isLoggedIn() && this.authService.getRoleFromToken() === 'Candidate';
  }

  goBack() {
    this.location.back();
  }

  loadFollowedJobs() {
    this.jobFollowService.getFollowedJobs()
      .subscribe({
        next: (follows) => {
          console.log('Loaded followed jobs:', follows);
          this.followedJobs = follows.map(follow => follow.recruitmentId);
          console.log('Followed job IDs:', this.followedJobs);
        },
        error: (error) => {
          console.error('Error loading followed jobs:', error);
        }
      });
  }

  toggleJobFollow(event: Event, jobId: number) {
    event.stopPropagation();
    if (!this.authService.isLoggedIn()) {
      return;
    }
    
    console.log('Toggling follow for job ID:', jobId);
    this.jobFollowService.toggleFollow(jobId)
      .subscribe({
        next: (response) => {
          console.log('Toggle follow response:', response);
          // Cập nhật trạng thái yêu thích trực tiếp trên UI
          if (this.isJobFollowed(jobId)) {
            // Nếu đã yêu thích, xóa khỏi danh sách
            this.followedJobs = this.followedJobs.filter(id => id !== jobId);
          } else {
            // Nếu chưa yêu thích, thêm vào danh sách
            this.followedJobs.push(jobId);
          }
        },
        error: (error) => {
          console.error('Error toggling job follow:', error);
        }
      });
  }

  isJobFollowed(jobId: number): boolean {
    return this.followedJobs.includes(jobId);
  }
}