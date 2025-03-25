import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CompanyService } from '../../../services/Company.service';
import { CommonModule } from '@angular/common';
import { Company } from '../../../models/Company';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../../services/auth.service';
import { CompanyFollowService } from '../../../services/company-follow.service';
@Component({
  selector: 'app-company-list',
  standalone: true,
  imports: [CommonModule, RouterLink, FormsModule],
  templateUrl: './company-list.component.html',
  styleUrl: './company-list.component.css'
})
export class CompanyListComponent {
  companies: Company[] = [];
  paginatedCompanies: Company[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 15;
  totalPages: number = 1;
  pageTitle: string = 'Danh Sách Doanh Nghiệp';
  search: string = "";
  unsearch: Company[] = [];
  followedCompanies: any[] = [];
  constructor(private route: ActivatedRoute, private companyService: CompanyService,
    private authService: AuthService,
    private router: Router,
    private companyFollowService: CompanyFollowService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const filterId = Number(params.get('id'));
      const urlPath = this.route.snapshot.url.map(segment => segment.path).join('/');


      this.companies = [];
      this.paginatedCompanies = [];
      this.totalPages = 1;
      this.pageTitle = 'Danh Sách Doanh Nghiệp';

      this.companyService.getAllCompanies().subscribe(
        (data) => {
          this.companies = data;
          this.unsearch = [...data];
          if (this.companies.length > 0) {
            this.pageTitle = 'Danh Sách Tất Cả Doanh Nghiệp';
          } else {
            this.pageTitle = 'Không có doanh nghiệp nào.';
          }
          this.updatePagination();
        },
        (error) => console.error('Error fetching all recruitments:', error)
      );
    });
  }
  updatePagination() {
    this.totalPages = Math.ceil(this.companies.length / this.recordsPerPage);
    this.goToPage(1);
  }
  goToPage(page: number) {
    this.currentPage = page;
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    this.paginatedCompanies = this.companies.slice(startIndex, startIndex + this.recordsPerPage);
  }

  companySearch() {
    if (!this.search) {
      this.companies = [...this.unsearch];
      this.updatePagination();
      return;
    }
    this.companyService.getCompaniesByName(this.search).subscribe(data => {
      this.companies = data;
      this.updatePagination();
    });
  }

  loadFollowedCompanies() {
    this.followedCompanies = [];
    this.companyFollowService.getFollowedCompanies()
      .subscribe(follows => {
        // Lấy ra mảng companyId từ danh sách follows
        this.followedCompanies = follows.map(follow => follow.companyId);
        console.log('Followed companies updated:', this.followedCompanies);
      });
  }

  toggleFollow(event: Event, companyId: number) {
    event.stopPropagation();

    if (!this.authService.isAuthenticated()) {
      alert('Vui lòng đăng nhập để theo dõi công ty');
      this.router.navigate(['/login']);
      return;
    }

    this.companyFollowService.toggleFollow(companyId)
      .subscribe(response => {
        alert(response.message);
        this.loadFollowedCompanies();
      });
  }

  isFollowing(companyId: number): boolean {
    return this.followedCompanies.includes(companyId);
  }
}
