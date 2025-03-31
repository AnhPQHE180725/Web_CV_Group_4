import { Component, OnInit } from '@angular/core';
import { UserService } from '../../services/User.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { CommonModule } from '@angular/common';
import { CompanyFollowService } from '../../services/company-follow.service';
import { RouterLink } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-favorite-companies',
  templateUrl: './favorite-companies.component.html',
  imports: [CommonModule, RouterLink, FormsModule],
  standalone: true,
  styleUrls: ['./favorite-companies.component.css']
})
export class FavoriteCompaniesComponent implements OnInit {
  favoriteCompanies: any[] = [];
  filteredCompanies: any[] = []; // Danh sách sau khi tìm kiếm
  paginatedCompanies: any[] = [];
  currentPage: number = 1;
  recordsPerPage: number = 5;
  totalPages: number = 1;
  searchQuery: string = ''; // Biến lưu từ khóa tìm kiếm

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
        this.filteredCompanies = [...companies]; // Sao chép danh sách gốc
        this.updatePagination();
      },
      error: (error) => {
        console.error('Lỗi khi tải danh sách công ty đã theo dõi:', error);
      }
    });
  }

  // Hàm tìm kiếm công ty theo tên
  searchCompanies(): void {
    const searchText = this.searchQuery.toLowerCase().trim();

    this.filteredCompanies = this.favoriteCompanies.filter(company => {
      const name = company.company.name?.toLowerCase() || '';
      const email = company.company.email?.toLowerCase() || '';
      const phone = company.company.phoneNumber || '';

      return (
        name.includes(searchText) ||
        email.includes(searchText) ||
        phone.includes(searchText)
      );
    });

    this.updatePagination();
  }


  // Cập nhật phân trang
  updatePagination() {
    this.totalPages = Math.ceil(this.filteredCompanies.length / this.recordsPerPage);
    this.paginateCompanies();
  }

  // Phân trang danh sách
  paginateCompanies() {
    const startIndex = (this.currentPage - 1) * this.recordsPerPage;
    const endIndex = startIndex + this.recordsPerPage;
    this.paginatedCompanies = this.filteredCompanies.slice(startIndex, endIndex);
  }

  // Chuyển trang
  goToPage(page: number) {
    if (page >= 1 && page <= this.totalPages) {
      this.currentPage = page;
      this.paginateCompanies();
    }
  }

  // Toggle theo dõi công ty
  toggleFollow(event: Event, companyId: number) {
    event.stopPropagation();
    this.companyFollowService.toggleFollow(companyId)
      .subscribe(response => {
        this.loadFavoriteCompanies(); // Reload danh sách sau khi bỏ theo dõi
      });
  }
}
