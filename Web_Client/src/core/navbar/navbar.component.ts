import { Component } from '@angular/core';
import { Category } from '../../app/models/Category';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../app/services/Category.service';
import { Route, Router, RouterLink } from '@angular/router';
import { AuthService } from '../../app/services/auth.service';

@Component({
  selector: 'app-navbar',
  standalone: true,
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  categories: Category[] = [];
  columns: Category[][] = [];

  constructor(
    private categoryService: CategoryService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    this.categoryService.getAllCategories().subscribe({
      next: (data) => {
        this.categories = data;
        console.log('Categories loaded:', this.categories);
        this.generateColumns();
      },
      error: (err) => console.error('Error fetching categories:', err)
    });
  }

  generateColumns() {
    const maxRows = 5;
    this.columns = [];
    for (let i = 0; i < this.categories.length; i++) {
      const colIndex = Math.floor(i / maxRows);
      if (!this.columns[colIndex]) {
        this.columns[colIndex] = [];
      }
      this.columns[colIndex].push(this.categories[i]);
    }
    console.log('Updated Columns:', this.columns);
  }

  // Phương thức đăng xuất
  logout() {
    this.authService.logout();
    this.router.navigateByUrl('/login'); // Điều hướng về trang login sau khi đăng xuất
  }

  // Kiểm tra trạng thái đăng nhập
  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }
}
