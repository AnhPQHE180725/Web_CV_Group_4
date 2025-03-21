import { Component } from '@angular/core';
import { LoginRequest } from '../../models/login-request';
import { AuthService } from '../../services/auth.service';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

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

  constructor(private authService: AuthService, private router: Router) {}

  onFormSubmit() {
    this.authService.login(this.model).subscribe({
      next: response => {
        this.authService.setToken(response.token);
        alert('Đăng nhập thành công!');
        this.router.navigateByUrl('login/confirm');
      },
      error: () => {
        alert('Đăng nhập thất bại. Vui lòng kiểm tra lại thông tin!');
      }
    });
  }
}
