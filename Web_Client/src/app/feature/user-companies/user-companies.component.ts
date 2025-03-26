import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Company } from '../../models/Company';
import { CompanyService } from '../../services/Company.service';
import { AuthService } from '../../services/auth.service';

@Component({
    selector: 'app-user-companies',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink],
    templateUrl: './user-companies.component.html',
    styleUrl: './user-companies.component.css'
})
export class UserCompaniesComponent implements OnInit {
    companies: Company[] = [];
    paginatedCompanies: Company[] = [];
    currentPage: number = 1;
    recordsPerPage: number = 10;
    totalPages: number = 1;
    pageTitle: string = 'Công ty của tôi';
    search: string = '';
    filteredCompanies: Company[] = [];

    // For creating/editing company
    isEditing: boolean = false;
    showForm: boolean = false;
    currentCompany: Company = {
        id: 0,
        name: '',
        description: '',
        address: '',
        email: '',
        phoneNumber: '',
        logo: '',
        status: 0,
        recruitments: []
    };

    constructor(
        private companyService: CompanyService,
        private authService: AuthService
    ) { }

    ngOnInit() {
        this.loadUserCompanies();
    }

    loadUserCompanies() {
        this.companyService.getUserCompanies().subscribe(
            (data) => {
                this.companies = data;
                this.filteredCompanies = [...data];
                this.updatePagination();
            },
            (error) => {
                console.error('Error fetching user companies:', error);
                if (error.status === 401) {
                    alert('Please log in to view your companies');
                }
            }
        );
    }

    updatePagination() {
        this.totalPages = Math.ceil(this.filteredCompanies.length / this.recordsPerPage);
        this.goToPage(1);
    }

    goToPage(page: number) {
        this.currentPage = page;
        const startIndex = (this.currentPage - 1) * this.recordsPerPage;
        this.paginatedCompanies = this.filteredCompanies.slice(startIndex, startIndex + this.recordsPerPage);
    }

    searchCompanies() {
        if (!this.search) {
            this.filteredCompanies = [...this.companies];
        } else {
            this.filteredCompanies = this.companies.filter(company =>
                company.name.toLowerCase().includes(this.search.toLowerCase())
            );
        }
        this.updatePagination();
    }

    // Create new company form
    showCreateForm() {
        this.isEditing = false;
        this.currentCompany = {
            id: 0,
            name: '',
            description: '',
            address: '',
            email: '',
            phoneNumber: '',
            logo: '',
            status: 0,
            recruitments: []
        };
        this.showForm = true;
    }

    // Edit existing company
    editCompany(company: Company) {
        this.isEditing = true;
        this.currentCompany = { ...company };
        // If the existing company doesn't have the new fields, initialize them
        if (!this.currentCompany.description) this.currentCompany.description = '';
        if (!this.currentCompany.address) this.currentCompany.address = '';
        if (!this.currentCompany.email) this.currentCompany.email = '';
        if (!this.currentCompany.phoneNumber) this.currentCompany.phoneNumber = '';
        if (this.currentCompany.status === undefined) this.currentCompany.status = 0;

        this.showForm = true;
    }

    // Save company (create or update)
    saveCompany() {
        if (this.isEditing) {
            this.companyService.updateCompany(this.currentCompany).subscribe(
                (response) => {
                    alert('Công ty đã được cập nhật thành công!');
                    this.showForm = false;
                    this.loadUserCompanies();
                },
                (error) => {
                    console.error('Error updating company:', error);
                    alert('Đã xảy ra lỗi khi cập nhật công ty');
                }
            );
        } else {
            this.companyService.createCompany(this.currentCompany).subscribe(
                (response) => {
                    alert('Công ty đã được tạo thành công!');
                    this.showForm = false;
                    this.loadUserCompanies();
                },
                (error) => {
                    console.error('Error creating company:', error);
                    alert('Đã xảy ra lỗi khi tạo công ty');
                }
            );
        }
    }

    // Delete company
    deleteCompany(id: number) {
        if (confirm('Bạn có chắc chắn muốn xóa công ty này?')) {
            this.companyService.deleteCompany(id).subscribe(
                (response) => {
                    alert('Công ty đã được xóa thành công!');
                    this.loadUserCompanies();
                },
                (error) => {
                    console.error('Error deleting company:', error);
                    alert('Đã xảy ra lỗi khi xóa công ty');
                }
            );
        }
    }

    // Cancel form
    cancelForm() {
        this.showForm = false;
    }
} 