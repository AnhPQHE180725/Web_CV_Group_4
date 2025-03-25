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
    styleUrl: './favorite-companies.component.css'
})
export class FavoriteCompaniesComponent implements OnInit {
    favoriteCompanies: any[] = [];

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
                console.log(this.favoriteCompanies);

            },
            error: (error) => {
                console.error('Error loading favorite companies:', error);
            }
        });
    }

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