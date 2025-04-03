import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-forgotpassword',
  imports: [FormsModule],
  templateUrl: './forgotpassword.component.html',
  styleUrl: './forgotpassword.component.css'
})
export class ForgotpasswordComponent {
  email: string ='';
  isWorking = false; // Trạng thái xử lý

  constructor(private authService: AuthService, private router: Router) {}
  onFormSubmit(): void {
    if (this.isWorking) return; // Nếu đang xử lý, không làm gì cả

    if (!this.email.trim()) {
      alert('Vui lòng nhập email.');
      return;
    }

    this.isWorking = true; // Bắt đầu xử lý

    this.authService.forgotPassword(this.email).subscribe({
      next: () => {
        alert('Email đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email của bạn.');
        this.router.navigate(['/login']);
      },
      error: (err) => {
        alert('Có lỗi xảy ra hoặc email không tồn tại.');
        console.error(err);
      },
      complete: () => {
        this.isWorking = false; // Hoàn tất xử lý
      }
    });
  }
}


  