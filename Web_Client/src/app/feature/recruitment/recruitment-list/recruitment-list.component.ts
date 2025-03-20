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
    this.route.url.subscribe(urlSegments => {
      const path = urlSegments.map(segment => segment.path).join('/');
      const filterId = Number(this.route.snapshot.paramMap.get('id'));

      if (path.startsWith('recruitment/category/')) {
        this.recruitmentService.getRecruitmentsByCategory(filterId).subscribe(
          (data) => {

            this.recruitments = data;
            this.pageTitle = `Danh Sách Tuyển Dụng - ${this.recruitments[0].categoryName}`;
            this.totalPages = Math.ceil(this.recruitments.length / this.recordsPerPage);
            this.updatePaginatedList();
          },
          (error) => console.error('Error fetching recruitments by category:', error)
        );
      } else if (path.startsWith('recruitment/company/')) {
        this.recruitmentService.getRecruitmentsByCompany(filterId).subscribe(
          (data) => {
            this.recruitments = data;
            this.pageTitle = `Danh Sách Tuyển Dụng - ${this.recruitments[0].companyName}`;
            this.totalPages = Math.ceil(this.recruitments.length / this.recordsPerPage);
            this.updatePaginatedList();
          },
          (error) => console.error('Error fetching recruitments by company:', error)
        );
      }
    });
  }

  updatePaginatedList() {
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    this.paginatedRecruitments = this.recruitments.slice(startIndex, startIndex + this.recordsPerPage);
  }

  goToPage(page: number) {
    this.currentPage = page;
    this.updatePaginatedList();
  }
}
