import { CommonModule } from '@angular/common';
import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-resetpassword',
  imports: [FormsModule,CommonModule],
  templateUrl: './resetpassword.component.html',
  styleUrl: './resetpassword.component.css'
})
export class ResetpasswordComponent {
  showNewPassword: boolean = false;
  showConfirmPassword: boolean = false;
  token: string = '';
  newPassword: string = '';
  confirmPassword: string = '';

  constructor(
    private route: ActivatedRoute,
    private authService: AuthService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.token = this.route.snapshot.paramMap.get('token') || '';
  }

  togglePassword(field: string) {
    if (field === 'new') {
      this.showNewPassword = !this.showNewPassword;
    } else if (field === 'confirm') {
      this.showConfirmPassword = !this.showConfirmPassword;
    }
  }

  onSubmit(): void {
    if (!this.newPassword || !this.confirmPassword) {
      alert('Vui lòng nhập đầy đủ mật khẩu.');
      return;
    }

    if (this.newPassword !== this.confirmPassword) {
      alert('Mật khẩu xác nhận không khớp.');
      return;
    }

    this.authService.resetPassword(this.token, this.newPassword).subscribe({
      next: () => {
        alert('Mật khẩu đã được đặt lại thành công.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        alert('Token không hợp lệ hoặc đã hết hạn.');
        console.error(err);
      }
    });
  }

}
