import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/RegisterRequest';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-signuppage',
  imports: [FormsModule],
  templateUrl: './signuppage.component.html',
  styleUrl: './signuppage.component.css'
})
export class SignuppageComponent {
   model: RegisterRequest = {
    email: '',
    fullName: '',
    password: '',
    confirmPassword: '',
    roleName: ''
  };
    isWorking = false; // Trạng thái xử lý


  constructor(
    private authService: AuthService,
    private router: Router,
    private cookieService: CookieService
  ) {}

  onFormSubmit() {
        if (this.isWorking) return; // Nếu đang xử lý, không làm gì cả

    const emailPattern = /^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$/;

    if (!emailPattern.test(this.model.email)) {
      alert('Email không đúng định dạng!');
      return;
    }

    if (this.model.password !== this.model.confirmPassword) {
      alert('Mật khẩu không khớp!');
      return;
    }

    if (!this.model.roleName) {
      alert('Vui lòng chọn vai trò!');
      return;
    }
        this.isWorking = true; // Bắt đầu xử lý

    this.authService.register(this.model).subscribe({
      next: (response) => {
        // Lưu email vào cookie với thời gian sống 5 phút
        this.cookieService.set('UserEmail', this.model.email, 5, '/', undefined, true, 'Strict');

        alert('Vui lòng kiểm tra email để nhập mã xác minh.');
        this.router.navigate(['/register/confirm']);
      },
      error: (error) => {
        alert('Đăng ký thất bại! Vui lòng thử lại.');
        console.error('Lỗi đăng ký:', error);
      },
      complete: () => {
        this.isWorking = false; // Hoàn tất xử lý
      }
    });
  }
}
