import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CvService } from '../../../services/cv.service';
import { ActivatedRoute } from '@angular/router';
import { Router } from '@angular/router';
import { UserService } from '../../../services/User.service';
import { Location } from '@angular/common';
@Component({
  selector: 'app-apply-cv',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './apply-cv.component.html',
  styleUrls: ['./apply-cv.component.css']
})
export class ApplyCVComponent {
  cvUrl: SafeResourceUrl | null = null;
  userId: number = 0;
  applyPostId: number = 0;
  constructor(
    private cvService: CvService,
    private sanitizer: DomSanitizer,
    private router: Router,
    private route: ActivatedRoute,
    private userService: UserService,
    private location: Location,

  ) { }



  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      this.applyPostId = Number(params.get('postid'));  // Get post ID
      this.userId = Number(params.get('userid'));

      if (!this.applyPostId || !this.userId) {
        console.warn('Missing postid or userid in the URL.');
        return;
      }

      console.log('Post ID:', this.applyPostId, 'User ID:', this.userId); // Debugging

      this.cvService.getCVUrl(this.userId).subscribe(
        (url) => {
          this.cvUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        },
        (error) => {
          console.error('Error fetching CV URL:', error);
        }
      );
    });
  }


  confirmAction(status: number) {
    const action = status === 2 ? 'Chấp thuận' : 'Từ chối';
    if (confirm(`Bạn có muốn ${action} CV này?`)) {
      const request = status === 2
        ? this.userService.applyCV(this.applyPostId)
        : this.userService.rejectCV(this.applyPostId);

      request.subscribe(
        () => {
          alert(`${action} thành công!`);
          this.location.back();

        },
        (error) => {
          console.error(`Error updating CV status:`, error);
        }
      );
    }
  }
}

