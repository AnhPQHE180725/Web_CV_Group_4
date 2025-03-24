import { Component, OnInit } from '@angular/core';

import { CommonModule } from '@angular/common';
import { Category } from '../../models/Category';
import { CategoryService } from '../../services/Category.service';
import { Company } from '../../models/Company';
import { CompanyService } from '../../services/Company.service';
import { Recruitment } from '../../models/Recruitment';
import { RecruitmentService } from '../../services/Recruitment.service';
import { ActivatedRoute } from '@angular/router';
import { RouterLink } from '@angular/router';

import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';


@Component({
  selector: 'app-homepage',
  imports: [CommonModule, RouterLink],
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


  constructor(
    private categoryService: CategoryService,
    private companyService: CompanyService,
    private recruitmentService: RecruitmentService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
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
  }
  prevSlide() {
    this.currentIndex = (this.currentIndex === 0) ? this.images.length - 1 : this.currentIndex - 1;
  }

  nextSlide() {
    this.currentIndex = (this.currentIndex + 1) % this.images.length;
  }


  onApplyClick(recruitment: Recruitment) {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    // Xử lý logic apply job
  }


}
