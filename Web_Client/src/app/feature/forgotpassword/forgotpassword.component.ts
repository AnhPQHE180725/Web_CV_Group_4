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
  constructor(private authService: AuthService, private router: Router) {}
    onFormSubmit(): void {
    if (this.email.trim()) {
      this.authService.forgotPassword(this.email).subscribe({
        next: () => {
          alert('Email đặt lại mật khẩu đã được gửi. Vui lòng kiểm tra email của bạn.');
          this.router.navigate(['/login']);
        },
        error: (err) => {
          alert('Có lỗi xảy ra hoặc email không tồn tại.');
          console.error(err);
        }
      });
    } else {
      alert('Vui lòng nhập email.');
    }
  }
}


