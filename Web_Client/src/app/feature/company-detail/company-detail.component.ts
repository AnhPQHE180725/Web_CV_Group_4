import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { CompanyService } from '../../services/Company.service';
import { Company } from '../../models/Company';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-company-detail',
    standalone: true,
    imports: [CommonModule, RouterLink],
    templateUrl: './company-detail.component.html',
    styleUrl: './company-detail.component.css'
})
export class CompanyDetailComponent implements OnInit {
    company: Company | null = null;
    isLoading = true;
    isOwner = false;
    errorMessage = '';

    constructor(
        private route: ActivatedRoute,
        private router: Router,
        private companyService: CompanyService,
        private authService: AuthService
    ) { }

    ngOnInit(): void {
        this.route.paramMap.subscribe(params => {
            const id = Number(params.get('id'));
            if (id) {
                this.loadCompany(id);
            } else {
                this.errorMessage = 'Không tìm thấy thông tin công ty.';
                this.isLoading = false;
            }
        });
    }

    loadCompany(id: number): void {
        this.isLoading = true;
        this.companyService.getCompanyById(id).subscribe(
            (company) => {
                this.company = company;
                this.isLoading = false;
                // Check if user is the owner of this company
                this.checkOwnership();
            },
            (error) => {
                console.error('Error loading company:', error);
                this.errorMessage = 'Không thể tải thông tin công ty. Vui lòng thử lại sau.';
                this.isLoading = false;
            }
        );
    }

    checkOwnership(): void {
        if (!this.authService.isAuthenticated()) {
            this.isOwner = false;
            return;
        }

        this.companyService.getUserCompanies().subscribe(
            (companies) => {
                this.isOwner = companies.some(c => c.id === this.company?.id);
            },
            (error) => {
                console.error('Error checking ownership:', error);
                this.isOwner = false;
            }
        );
    }

    // Navigate to edit page
    editCompany(): void {
        if (this.company) {
            this.router.navigate(['/user-companies']);
        }
    }

    // Handle back navigation
    goBack(): void {
        window.history.back();
    }
} 