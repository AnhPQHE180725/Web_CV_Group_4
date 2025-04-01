import { Component } from '@angular/core';
import { LoginRequest } from '../../models/login-request';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';

@Component({
  selector: 'app-loginpage',
  imports: [FormsModule],
  templateUrl: './loginpage.component.html',
  styleUrl: './loginpage.component.css'
})
export class LoginpageComponent {
  model: LoginRequest = {
    email: '',
    password: ''
  };

  constructor(private authService: AuthService, private router: Router, private cookieService: CookieService) {}

onFormSubmit() {
  this.authService.login(this.model).subscribe({
    next: response => {
      if (response.token) {
        // Nếu có token => user có role "Candidate" => đăng nhập ngay
        this.authService.setToken(response.token);
        alert('Đăng nhập thành công!');
        this.router.navigate(['/home']);
      } else {
        // Nếu không có token => cần xác minh OTP
        this.cookieService.set('UserEmail', this.model.email, 5, '/', undefined, true, 'Strict');
        alert('Vui lòng kiểm tra email để nhập mã xác minh.');
        this.router.navigate(['/login/confirm']);
      }
    },
    error: () => {
      alert('Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin!');
    }
  });
}
}
