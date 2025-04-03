import { CommonModule } from '@angular/common';
import { Component, OnInit, ViewChild, ElementRef } from '@angular/core';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Company } from '../../models/Company';
import { CompanyService } from '../../services/Company.service';
import { AuthService } from '../../services/auth.service';
import { EditorModule } from '@tinymce/tinymce-angular';

@Component({
    selector: 'app-user-companies',
    standalone: true,
    imports: [CommonModule, FormsModule, ReactiveFormsModule, RouterLink, EditorModule],
    templateUrl: './user-companies.component.html',
    styleUrl: './user-companies.component.css'
})
export class UserCompaniesComponent implements OnInit {
    // New properties for logo preview Xem truoc hinh anh logo cty
    logoPreviewUrl: string | ArrayBuffer | null = null;
    @ViewChild('logoFileInput') logoFileInput!: ElementRef<HTMLInputElement>;

    companies: Company[] = [];
    paginatedCompanies: Company[] = [];
    currentPage: number = 1;
    recordsPerPage: number = 10;
    totalPages: number = 1;
    pageTitle: string = 'Công ty của tôi';
    search: string = '';
    filteredCompanies: Company[] = [];

    // Biến lưu giá trị tìm kiếm, đã tắt tìm kiếm đc sđt
    searchName: string = '';
    searchAddress: string = '';
    searchPhone: string = '';

    // For creating/editing company
    isEditing: boolean = false;
    showForm: boolean = false;
    selectedLogoFile: File | null = null;
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
    // Logo preview methods
    onLogoSelected(event: any) {
        const file = event.target.files[0];
        if (file) {
            // Validate file type
            if (!this.isValidImageFile(file)) {
                alert('Chỉ chấp nhận file ảnh (JPEG, PNG, GIF, JPG)');
                this.resetLogoPreview();
                return;
            }

            // Create file reader for preview
            const reader = new FileReader();
            reader.onload = (e: any) => {
                // Set preview URL
                this.logoPreviewUrl = e.target.result;
            };

            // Read the file as data URL
            reader.readAsDataURL(file);

            // Store the file for upload
            this.selectedLogoFile = file;
        }
    }

    // Reset logo preview and file input
    resetLogoPreview() {
        // Reset preview
        this.logoPreviewUrl = null;
        this.selectedLogoFile = null;

        // Reset file input using ViewChild
        if (this.logoFileInput) {
            this.logoFileInput.nativeElement.value = '';
        }
    }

    // Validate image file
    isValidImageFile(file: File): boolean {
        const validTypes = ['image/jpeg', 'image/png', 'image/gif', 'image/jpg'];
        return validTypes.includes(file.type);
    }

    // Create new company form
    showCreateForm() {
        this.isEditing = false;
        this.selectedLogoFile = null;
        this.logoPreviewUrl = ''; // reset preview nếu có
        this.currentCompany = {
            id: 0,
            name: '',
            description: '',
            address: '',
            email: '',
            phoneNumber: '',
            logo: '',
            status: 1, // ✅ mặc định là Hoạt động
            recruitments: []
        };
        this.showForm = true;
    }

    // Edit existing company
    editCompany(company: Company) {
        this.isEditing = true;
        this.resetLogoPreview();
        this.currentCompany = { ...company };

        // If the existing company doesn't have the new fields, initialize them
        if (!this.currentCompany.description) this.currentCompany.description = '';
        if (!this.currentCompany.address) this.currentCompany.address = '';
        if (!this.currentCompany.email) this.currentCompany.email = '';
        if (!this.currentCompany.phoneNumber) this.currentCompany.phoneNumber = '';
        if (this.currentCompany.status === undefined) this.currentCompany.status = 0;

        this.showForm = true;
    }

    // Cancel form
    cancelForm() {
        this.showForm = false;
        this.resetLogoPreview();
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

    searchTerm: string = '';

    searchCompanies() {
        if (!this.searchTerm.trim()) {
            this.filteredCompanies = [...this.companies];
        } else {
            const searchLower = this.searchTerm.toLowerCase();

            this.filteredCompanies = this.companies.filter(company =>
                company.name.toLowerCase().includes(searchLower) ||
                (company.address && company.address.toLowerCase().includes(searchLower)) ||
                (company.phoneNumber && company.phoneNumber.includes(searchLower))
            );
        }

        this.updatePagination();
    }

    // Validation methods
    isValidEmail(email: string): boolean {
        const emailRegex = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
        return emailRegex.test(email);
    }

    isValidPhoneNumber(phone: string): boolean {
        const phoneRegex = /^\d{10}$/;
        return phoneRegex.test(phone);
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

    // Kiểm tra đường dẫn có hợp lệ không (chỉ nhận .jpg, .png)
    isValidImageUrl(url: string): boolean {
        return /\.(jpg|jpeg|png)$/i.test(url.trim());
    }

    saveCompany() {
        // Validate tên công ty
        if (!this.currentCompany.name || this.currentCompany.name.trim().length === 0) {
            alert('Vui lòng nhập tên công ty!');
            return;
        }

        // Validate email
        if (!this.currentCompany.email || !this.isValidEmail(this.currentCompany.email)) {
            alert('Vui lòng nhập địa chỉ email hợp lệ!');
            return;
        }

        // Validate số điện thoại
        if (!this.currentCompany.phoneNumber || !this.isValidPhoneNumber(this.currentCompany.phoneNumber)) {
            alert('Vui lòng nhập số điện thoại hợp lệ (10 chữ số)!');
            return;
        }

        if (!this.currentCompany.logo || !this.isValidImageUrl(this.currentCompany.logo)) {
            alert('Vui lòng nhập đường dẫn hợp lệ cho logo (.jpg, .png)!');
            return;
        }

        if (this.isEditing) {
            this.companyService.updateCompany(this.currentCompany).subscribe(() => {
                alert('Công ty đã được cập nhật!');
                this.showForm = false;
                this.loadUserCompanies();
            });
        } else {
            this.companyService.createCompany(this.currentCompany).subscribe(() => {
                alert('Công ty đã được tạo!');
                this.showForm = false;
                this.loadUserCompanies();
            });
        }
    }


} 