import { Component } from '@angular/core';
import { Category } from '../../app/models/Category';
import { CommonModule } from '@angular/common';
import { CategoryService } from '../../app/services/Category.service';
import { RouterLink } from '@angular/router';
@Component({
  selector: 'app-navbar',
  imports: [CommonModule, RouterLink],
  templateUrl: './navbar.component.html',
  styleUrl: './navbar.component.css'
})
export class NavbarComponent {
  categories: Category[] = [];
  columns: Category[][] = [];
  constructor(private categoryService: CategoryService) {

  }

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
    const maxRows = 5; // Adjust based on desired rows
    this.columns = [];

    for (let i = 0; i < this.categories.length; i++) {
      const colIndex = Math.floor(i / maxRows);

      if (!this.columns[colIndex]) {
        this.columns[colIndex] = []; // Initialize column if it doesn't exist
      }

      this.columns[colIndex].push(this.categories[i]);
    }

    console.log('Updated Columns:', this.columns); // ✅ Debugging
  }


}
