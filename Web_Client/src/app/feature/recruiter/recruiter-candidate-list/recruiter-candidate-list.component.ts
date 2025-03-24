import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { User } from '../../../models/User';
import { UserService } from '../../../services/User.service';
@Component({
  selector: 'app-recruiter-candidate-list',
  imports: [CommonModule],
  templateUrl: './recruiter-candidate-list.component.html',
  styleUrl: './recruiter-candidate-list.component.css'
})
export class RecruiterCandidateListComponent {
  candidates: User[] = [];
  paginatedCandidates: User[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 10;
  totalPages: number = 1;
  pageTitle: string = 'Danh Sách Ứng Viên';
  constructor(private userService: UserService, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const filterId = Number(params.get('id'));
      const urlPath = this.route.snapshot.url.map(segment => segment.path).join('/');


      this.candidates = [];
      this.paginatedCandidates = [];
      this.totalPages = 1;
      this.pageTitle = 'Danh Sách Ứng Viên';

      this.userService.getUserByPostId(filterId).subscribe(
        (data) => {
          this.candidates = data;
          if (this.candidates.length > 0) {
            this.pageTitle = `Danh Sách Ứng Viên - ${this.candidates[0].postName}`;
          } else {
            this.pageTitle = 'Không có ứng viên nào trong bài đăng này.';
          }
          this.updatePagination();
        },
        (error) => console.error('Error fetching:', error)
      );

    }
    );



  }
  updatePagination() {
    this.totalPages = Math.ceil(this.candidates.length / this.recordsPerPage);
    this.goToPage(1);
  }
  goToPage(page: number) {
    this.currentPage = page;
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    this.paginatedCandidates = this.candidates.slice(startIndex, startIndex + this.recordsPerPage);
  }

}
