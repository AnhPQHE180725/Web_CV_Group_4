import { Component } from '@angular/core';
import { Recruitment } from '../../../models/Recruitment';
import { RecruitmentService } from '../../../services/Recruitment.service';
import { ActivatedRoute } from '@angular/router';
import { CommonModule } from '@angular/common';
@Component({
  selector: 'app-recruitment-detail',
  imports: [CommonModule],
  templateUrl: './recruitment-detail.component.html',
  styleUrl: './recruitment-detail.component.css'
})
export class RecruitmentDetailComponent {
  recruitment: any;
  id: number = 0;
  constructor(private recruitmentService: RecruitmentService, private route: ActivatedRoute) { }

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
}
