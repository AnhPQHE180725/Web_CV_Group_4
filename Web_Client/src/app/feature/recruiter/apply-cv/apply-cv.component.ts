import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { CvService } from '../../../services/cv.service';
import { ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-apply-cv',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './apply-cv.component.html',
  styleUrls: ['./apply-cv.component.css']
})
export class ApplyCVComponent {
  cvUrl: SafeResourceUrl | null = null;


  constructor(private cvService: CvService, private sanitizer: DomSanitizer, private route: ActivatedRoute) { }

  ngOnInit() {
    this.route.paramMap.subscribe(params => {
      const userId = Number(params.get('id'));
      const urlPath = this.route.snapshot.url.map(segment => segment.path).join('/');
      this.cvService.getCVUrl(userId).subscribe(
        (url) => {
          this.cvUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
        },
        (error) => {
          console.error('Error fetching CV URL:', error);
        }
      );
    }
    );
  }
}

