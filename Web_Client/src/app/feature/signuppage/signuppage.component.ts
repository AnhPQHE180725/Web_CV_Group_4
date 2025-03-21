import { Component } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AuthService } from '../../services/auth.service';
import { RegisterRequest } from '../../models/RegisterRequest';
import { Router } from '@angular/router';

@Component({
  selector: 'app-signuppage',
  imports: [FormsModule],
  templateUrl: './signuppage.component.html',
  styleUrl: './signuppage.component.css'
})
export class SignuppageComponent {
  email: string = '';
  fullName: string = '';
  password: string = '';
  confirmPassword: string = '';
  role: string = '';

  constructor(private authService: AuthService, private route : Router) {}

  onFormSubmit() {
    if (this.password !== this.confirmPassword) {
      alert('Passwords do not match!');
      return;
    }

    if(this.role == ''){
      alert('Role must not empty')
      return;
    }

    const request: RegisterRequest = {
      email: this.email,
      fullName: this.fullName,
      password: this.password,
      confirmPassword: this.confirmPassword,
      roleName: this.role
    };

    this.authService.register(request).subscribe({
      next: (response) => {
        alert('Đăng ký thành công!');
        this.route.navigateByUrl('login');
        console.log(response);
      },
      error: (error) => {
        alert('Đăng ký thất bại!');
        console.error('Lỗi đăng ký:', error);
      }
    });
  }
}
