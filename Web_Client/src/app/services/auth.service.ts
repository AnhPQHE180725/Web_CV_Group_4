import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { BehaviorSubject, Observable } from 'rxjs';
import { jwtDecode } from 'jwt-decode';
import { RegisterRequest } from '../models/RegisterRequest';


@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private baseUrl = "https://localhost:7247/api";

  constructor(private http: HttpClient, private cookieService: CookieService) {

  }
  // Phương thức gọi API đăng nhập
  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http.post<LoginResponse>(`${this.baseUrl}/Authentication/login`, request);
  }
  // Lưu token vào cookie
  setToken(token: string): void {
    this.cookieService.set('Authentication', token, undefined, '/', undefined, true, 'Strict');
  }

  // Xóa token khỏi cookie (logout)
  logout(): void {
    this.cookieService.delete('Authentication', '/');
  }

  // Kiểm tra xem token có tồn tại không
  isAuthenticated(): boolean {
    return this.cookieService.check('Authentication');
  }

  // Lấy token từ cookie
  getToken(): string {
    return this.cookieService.get('Authentication');
  }
// lấy email từ token
  getEmailFromToken(): string | undefined {
    const token = this.getToken();
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        console.log('Full decoded token:', decodedToken); // Giữ log để kiểm tra
        return decodedToken.email; 
      } catch (error) {
        console.error('Token không hợp lệ:', error);
        return undefined;
      }
    }
    return undefined;
  }
// lấy role từ token
  getRoleFromToken(): string | undefined {
    const token = this.getToken();
    if (token) {
      try {
        const decodedToken: any = jwtDecode(token);
        console.log('Decoded Token:', decodedToken); // Giữ log để kiểm tra
        return decodedToken.role; // Đã đúng với "role" từ backend, không cần sửa
      } catch (error) {
        console.error('Token không hợp lệ:', error);
        return undefined;
      }
    }
    return undefined;
  }

  // đăng kí tài khoản
  register(request: RegisterRequest): Observable<any> {
    return this.http.post(`${this.baseUrl}/Authentication/register`, request);
  }
  // kiểm tra đăng nhập
  isLoggedIn(): boolean {
    return this.isAuthenticated();
  }

  // Gửi email lấy lại mật khẩu
  forgotPassword(email: string): Observable<any> {
    return this.http.post(`${this.baseUrl}/Authentication/forgot-password`, { email });
  }
  //Reset mật khẩu
resetPassword(token: string, newPassword: string): Observable<any> {
  const formData = new FormData();
  formData.append('token', token);
  formData.append('newPassword', newPassword);

return this.http.post(`${this.baseUrl}/Authentication/reset-password`, formData);
}
}
