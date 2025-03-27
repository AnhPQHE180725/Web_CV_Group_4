import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/User.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CompanyFollowService } from '../../services/company-follow.service';
import { RouterLink } from '@angular/router';

@Component({
  selector: 'app-favorite-companies',
  templateUrl: './favorite-companies.component.html',
  imports: [CommonModule, RouterLink],
  standalone: true,
  styleUrls: ['./favorite-companies.component.css']
})
export class FavoriteCompaniesComponent implements OnInit {
  favoriteCompanies: any[] = [];
  paginatedCompanies: any[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;  // Số lượng công ty hiển thị mỗi trang
  totalPages: number = 1;

  constructor(
    private userService: UserService,
    private authService: AuthService,
    private router: Router,
    private companyFollowService: CompanyFollowService
  ) { }

  ngOnInit(): void {
    if (!this.authService.isLoggedIn()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadFavoriteCompanies();
  }

  private loadFavoriteCompanies(): void {
    this.userService.getFavoriteCompanies().subscribe({
      next: (companies) => {
        this.favoriteCompanies = companies;
        this.updatePagination();
      },
      error: (error) => {
        console.error('Error loading favorite companies:', error);
      }
    });
  }

  // Cập nhật phân trang
  updatePagination() {
    this.totalPages = Math.ceil(this.favoriteCompanies.length / this.recordsPerPage);
    this.paginateCompanies();
  }

  // Phân trang công ty
  paginateCompanies() {
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    const endIndex = startIndex + this.recordsPerPage;
    this.paginatedCompanies = this.favoriteCompanies.slice(startIndex, endIndex);
  }

  // Chuyển trang
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.paginateCompanies();
    }
  }

  // Hàm toggle theo dõi công ty
  toggleFollow(event: Event, companyId: number) {
    event.stopPropagation();
    this.companyFollowService.toggleFollow(companyId)
      .subscribe(response => {
        this.loadFavoriteCompanies(); // Reload the list after toggling
      });
  }

  isFollowing(companyId: number): boolean {
    return true; // Always true since this is the favorites list
  }
}
