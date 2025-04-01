import { Component, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { Category } from '../../models/Category';
import { CategoryService } from '../../services/Category.service';
import { Company } from '../../models/Company';
import { CompanyService } from '../../services/Company.service';
import { Recruitment } from '../../models/Recruitment';
import { RecruitmentService } from '../../services/Recruitment.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../services/auth.service';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { ApplyDialogComponent } from '../recruitment/apply-dialog/apply-dialog.component';
import { JobFollowService } from '../../services/job-follow.service';
import { CompanyFollowService } from '../../services/company-follow.service';

@Component({
  selector: 'app-homepage',
  standalone: true,
  imports: [CommonModule, RouterLink, MatDialogModule],
  templateUrl: './homepage.component.html',
  styleUrl: './homepage.component.css'
})
export class HomepageComponent {
  categories: Category[] = [];
  companies: Company[] = [];
  recruitments: Recruitment[] = [];
  images: string[] = [
    'assets/images/slide1.jpg',
    'assets/images/slide2.jpg',
    'assets/images/slide3.jpg',
    'assets/images/slide4.jpg',
    'assets/images/slide5.jpg'
  ];
  currentIndex: number = 0;
  followedJobs: number[] = [];
  followedCompanies: number[] = [];

  constructor(
    private categoryService: CategoryService,
    private companyService: CompanyService,
    private recruitmentService: RecruitmentService,
    private route: ActivatedRoute,
    private router: Router,
    private dialog: MatDialog,
    private authService: AuthService,
    private jobFollowService: JobFollowService,
    private companyFollowService: CompanyFollowService
  ) { }

  ngOnInit(): void {
    this.categoryService.getTopCategories().subscribe({
      next: (data) => {
        this.categories = data;
        console.log('Categories loaded:', this.categories);
      },
      error: (err) => console.error('Error fetching categories:', err)
    });

    this.companyService.getTopCompanies().subscribe({
      next: (data) => {
        this.companies = data;
        console.log('Companies loaded:', this.companies);
      },
      error: (err) => console.error('Error fetching companies:', err)
    });

    this.recruitmentService.getTopRecruitments().subscribe({
      next: (data) => {
        this.recruitments = data;
        console.log('Recruitments loaded:', this.recruitments);
      },
      error: (err) => console.error('Error fetching recruitments:', err)
    });

    setInterval(() => {
      this.currentIndex = (this.currentIndex + 1) % this.images.length;
    }, 15000);

    if (this.authService.isLoggedIn()) {
      this.loadFollowedJobs();
      this.loadFollowedCompanies();
    }
  }

  prevSlide() {
    this.currentIndex = (this.currentIndex === 0) ? this.images.length - 1 : this.currentIndex - 1;
  }

  nextSlide() {
    this.currentIndex = (this.currentIndex + 1) % this.images.length;
  }

  isLoggedIn(): boolean {
    return this.authService.isLoggedIn();
  }

  // Kiểm tra nếu người dùng đã đăng nhập và có role là Candidate
  isCandidate(): boolean {
    return this.authService.isLoggedIn() && this.authService.getRoleFromToken() === 'Candidate';
  }

  onApply(recruitment: Recruitment) {
    if (!this.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }

    this.dialog.open(ApplyDialogComponent, {
      width: '500px',
      data: {
        jobTitle: recruitment.title,
        recruitmentId: recruitment.id
      }
    });
  }


  loadFollowedJobs() {
    this.jobFollowService.getFollowedJobs()
      .subscribe(follows => {
        this.followedJobs = follows.map(follow => follow.recruitmentId);
      });
  }

  toggleJobFollow(event: Event, jobId: number) {
    event.stopPropagation();
    if (!this.authService.isLoggedIn()) {
      return;
    }
    this.jobFollowService.toggleFollow(jobId)
      .subscribe(response => {
        this.loadFollowedJobs();
      });
  }

  isJobFollowed(jobId: number): boolean {
    return this.followedJobs.includes(jobId);
  }

  loadFollowedCompanies() {
    if (this.isLoggedIn()) {
      this.companyFollowService.getFollowedCompanies()
        .subscribe(follows => {
          this.followedCompanies = follows.map(follow => follow.companyId);
        });
    }
  }

  toggleCompanyFollow(event: Event, companyId: number) {
    event.stopPropagation();

    if (!this.isLoggedIn()) {
      alert('Vui lòng đăng nhập để theo dõi công ty');
      this.router.navigate(['/login']);
      return;
    }

    this.companyFollowService.toggleFollow(companyId)
      .subscribe(response => {
        this.loadFollowedCompanies();
      });
  }

  isCompanyFollowed(companyId: number): boolean {
    return this.followedCompanies.includes(companyId);
  }

  viewDetail(recruitment: Recruitment) {
    this.router.navigate(['recruitment/detail', recruitment.id]);
  }
}

