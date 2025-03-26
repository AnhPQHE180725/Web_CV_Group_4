import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Recruitment } from '../../models/Recruitment';
import { ApplicationService } from '../../services/application.service';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-applied-jobs',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './applied-jobs.component.html',
  styleUrl: './applied-jobs.component.css'
})
export class AppliedJobsComponent {
  appliedJobs: any[] = [];
  pageTitle: string = 'Danh Sách Công Việc Đã Ứng Tuyển';

  constructor(
    private applicationService: ApplicationService,
    private authService: AuthService,
    private router: Router
  ) { }

  ngOnInit() {
    if (!this.authService.isAuthenticated()) {
      this.router.navigate(['/login']);
      return;
    }
    this.loadAppliedJobs();
  }

  loadAppliedJobs() {
    this.applicationService.getAppliedJobs().subscribe(
      (data) => {
        this.appliedJobs = data;
        console.log(this.appliedJobs);
      },
      (error) => console.error('Error fetching applied jobs:', error)
    );
  }
}
