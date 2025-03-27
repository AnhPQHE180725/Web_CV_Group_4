import { Component } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-confirmlogin',
  imports: [FormsModule],
  templateUrl: './confirmlogin.component.html',
  styleUrl: './confirmlogin.component.css'
})
export class ConfirmloginComponent {

  email: string = '';
  otpCode: string = '';

  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.email = this.route.snapshot.queryParams['email'];
  }

ngOnInit(): void {
    this.email = localStorage.getItem('email') || '';
}

verifyOtp() {
    this.authService.verifyOtp(this.email, this.otpCode).subscribe({
      next: response => {
        this.authService.setToken(response.token);
        localStorage.removeItem('email'); // Xóa email sau khi xác minh thành công
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
    this.authService.resendOtp(this.email).subscribe({
      next: () => {
        alert('Mã OTP mới đã được gửi đến email của bạn. Vui lòng kiểm tra hộp thư!');
      },
      error: () => {
        alert('Gửi lại mã OTP thất bại. Vui lòng thử lại!');
      }
    });
}

}
