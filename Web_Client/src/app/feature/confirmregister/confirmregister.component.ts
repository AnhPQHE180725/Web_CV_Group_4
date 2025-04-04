import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-confirmregister',
  imports: [FormsModule],
  templateUrl: './confirmregister.component.html',
  styleUrl: './confirmregister.component.css'
})
export class ConfirmregisterComponent {
  email: string = '';
  otpCode: string = '';
  isResending = false;


  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router,
    private cookieService: CookieService
  ) {}

  ngOnInit(): void {
    // Đọc email từ cookie thay vì localStorage
    this.email = this.cookieService.get('UserEmail') || '';
  }

  verifyOtp() {
    this.authService.verifySignup({ email: this.email, otp: this.otpCode }).subscribe({
      next: () => {
        // Xóa cookie sau khi xác minh thành công
        this.cookieService.delete('UserEmail', '/');
        
        alert('Xác minh thành công! Tài khoản của bạn đã được kích hoạt.');
        this.router.navigateByUrl('/login');
      },
      error: () => {
        alert('Xác minh thất bại. Vui lòng thử lại!');
      }
    });
  }

  goBack() {
    this.router.navigateByUrl('/register');
  }

resendOtp() {
  this.isResending = true; // Hiển thị trạng thái đang gửi

  this.authService.resendOtpRegister(this.email).subscribe({
    next: (response) => {
      alert(response.message);
      this.isResending = false; // Bật lại nút sau khi gửi xong
    },
    error: () => {
      alert('Gửi lại mã OTP thất bại. Vui lòng thử lại!');
      this.isResending = false; 
    }
  });
}

}
