import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { LoginRequest } from '../models/login-request';
import { LoginResponse } from '../models/login-response';
import { Observable } from 'rxjs';


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
}
