import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-confirmlogin',
  imports: [FormsModule],
  templateUrl: './confirmlogin.component.html',
  styleUrl: './confirmlogin.component.css'
})
export class ConfirmloginComponent {
  email: string = '';
  otpCode: string = '';
  isResending = false;


  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private cookieService: CookieService // Thêm CookieService vào đây
  ) {}

  ngOnInit(): void {
    // Đọc email từ cookie thay vì localStorage
    this.email = this.cookieService.get('UserEmail') || '';
  }

  verifyOtp() {
    this.authService.verifyOtp(this.email, this.otpCode).subscribe({
      next: response => {
        this.authService.setToken(response.token);
        
        // Xóa cookie sau khi xác minh thành công
        this.cookieService.delete('UserEmail', '/');
        
        alert('Xác minh thành công!');
        this.router.navigateByUrl('/home');
      },
      error: () => {
        alert('Xác minh thất bại. Vui lòng thử lại!');
      }
    });
  }

  goBack() {
    this.router.navigateByUrl('/login');
  }

resendOtp() {
  this.isResending = true; // ✅ Bật trạng thái "đang gửi"

  this.authService.resendOtp(this.email).subscribe({
    next: () => {
      alert('Mã OTP mới đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư!');
      this.isResending = false; // ✅ Kết thúc trạng thái "đang gửi"
    },
    error: () => {
      alert('Gửi lại mã OTP thất bại. Vui lòng thử lại!');
      this.isResending = false; 
    }
  });
}
}
