import { Component } from '@angular/core';
import { Recruitment } from '../../../models/Recruitment';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { CommonModule } from '@angular/common';
import { ApplyDialogComponent } from '../apply-dialog/apply-dialog.component';
import { MatDialog, MatDialogModule } from '@angular/material/dialog';
import { AuthService } from '../../../services/auth.service';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
@Component({
  selector: 'app-recruitment-detail',
  imports: [CommonModule, MatDialogModule, RouterLink],
  templateUrl: './recruitment-detail.component.html',
  styleUrl: './recruitment-detail.component.css'
})
export class RecruitmentDetailComponent {
  recruitment: any;
  id: number = 0;
  constructor(private recruitmentService: RecruitmentService, private route: ActivatedRoute,
    private dialog: MatDialog, private authService: AuthService
  ) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {

      this.id = Number(params.get('id'));

      if (!this.id) {
        console.warn('Missing postid or userid in the URL.');
        return;
      }

      console.log('id:', this.id);

      this.recruitmentService.getRecruitmentById(this.id).subscribe(
        (data) => {
          console.log('Fetched recruitment data:', data);
          this.recruitment = data; // Assign API response to recruitment
        },
        (error) => {
          console.error('Error fetching recruitment details:', error);
        }
      );
    });
  }

  onApply(recruitment: Recruitment) {
    this.dialog.open(ApplyDialogComponent, {
      width: '500px',
      data: {
        jobTitle: recruitment.title,
        recruitmentId: recruitment.id
      }
    });
  }

  isLoggedIn(): boolean {
    return this.authService.isAuthenticated();
  }

  isCandidate(): boolean {
    return this.authService.isLoggedIn() && this.authService.getRoleFromToken() === 'Candidate';
  }
}
