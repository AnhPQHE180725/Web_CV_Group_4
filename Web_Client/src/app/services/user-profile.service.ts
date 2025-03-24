import { Injectable } from '@angular/core';
import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { UserProfile } from '../models/UserProfile';
@Injectable({
  providedIn: 'root'
})
export class UserProfileService {
  private baseUrl = "https://localhost:7247/api";
  constructor(private http: HttpClient) { }
  getProfile(): Observable<UserProfile>{
    return this.http.get<UserProfile>(`${this.baseUrl}/Authentication/profile`);
  }
  updateProfile(data: UserProfile): Observable<any> {
    return this.http.put(`${this.baseUrl}/Authentication/update-profile`, data);
  }
}
