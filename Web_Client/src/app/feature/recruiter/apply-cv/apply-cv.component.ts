import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
@Component({
  selector: 'app-apply-cv',
  imports: [CommonModule],
  templateUrl: './apply-cv.component.html',
  styleUrl: './apply-cv.component.css'
})
export class ApplyCVComponent {
  pdfUrl!: SafeResourceUrl;

  constructor(private sanitizer: DomSanitizer) { }
  ngOnInit(): void {
    this.loadLocalPDF();
  }

  loadLocalPDF() {
    const pdfPath = '/assets/CVs/GuideToInstallAngular.pdf';
    this.pdfUrl = this.sanitizer.bypassSecurityTrustResourceUrl(pdfPath);
  }
}
