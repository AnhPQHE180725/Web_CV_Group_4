import { Component } from '@angular/core';
import { Recruitment } from '../../../models/Recruitment';
import { CommonModule } from '@angular/common';
import { ActivatedRoute } from '@angular/router';
import { RecruitmentService } from '../../../services/Recruitment.service';
@Component({
  selector: 'app-recruitment-list',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './recruitment-list.component.html',
  styleUrl: './recruitment-list.component.css'
})
export class RecruitmentListComponent {
  recruitments: Recruitment[] = [];
  paginatedRecruitments: Recruitment[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;
  totalPages: number = 1;
  pageTitle: string = 'Danh Sách Tuyển Dụng';
  constructor(private route: ActivatedRoute, private recruitmentService: RecruitmentService) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const filterId = Number(params.get('id'));
      const urlPath = this.route.snapshot.url.map(segment => segment.path).join('/');

      // Clear previous data
      this.recruitments = [];
      this.paginatedRecruitments = [];
      this.totalPages = 1;
      this.pageTitle = 'Danh Sách Tuyển Dụng';

      if (urlPath.startsWith('recruitment/category/')) {
        this.fetchRecruitmentsByCategory(filterId);
      } else if (urlPath.startsWith('recruitment/company/')) {
        this.fetchRecruitmentsByCompany(filterId);
      }
    });
  }

  fetchRecruitmentsByCategory(categoryId: number) {
    this.recruitmentService.getRecruitmentsByCategory(categoryId).subscribe(
      (data) => {
        this.recruitments = data;
        if (this.recruitments.length > 0) {
          this.pageTitle = `Danh Sách Tuyển Dụng - ${this.recruitments[0].categoryName}`;
        } else {
          this.pageTitle = 'Không có tuyển dụng nào trong danh mục này.';
        }
        this.updatePagination();
      },
      (error) => console.error('Error fetching recruitments by category:', error)
    );
  }

  fetchRecruitmentsByCompany(companyId: number) {
    this.recruitmentService.getRecruitmentsByCompany(companyId).subscribe(
      (data) => {
        this.recruitments = data;
        if (this.recruitments.length > 0) {
          this.pageTitle = `Danh Sách Tuyển Dụng - ${this.recruitments[0].companyName}`;
        } else {
          this.pageTitle = 'Không có tuyển dụng nào trong công ty này.';
        }
        this.updatePagination();
      },
      (error) => console.error('Error fetching recruitments by company:', error)
    );
  }

  updatePagination() {
    this.totalPages = Math.ceil(this.recruitments.length / this.recordsPerPage);
    this.goToPage(1);
  }
  goToPage(page: number) {
    this.currentPage = page;
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    this.paginatedRecruitments = this.recruitments.slice(startIndex, startIndex + this.recordsPerPage);
  }
}
