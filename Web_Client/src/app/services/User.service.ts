import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { User } from '../models/User';


@Injectable({
    providedIn: 'root'
})

export class UserService {
    private baseUrl = "https://localhost:7247/api";
    constructor(private http: HttpClient) { }

    getUserByPostId(id: number): Observable<User[]> {
        return this.http.get<User[]>(`${this.baseUrl}/User/get-user-by-post-id/${id}`)
    }

    applyCV(userId: number): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/User/apply-cv/${userId}`, {});
    }

    rejectCV(userId: number): Observable<void> {
        return this.http.put<void>(`${this.baseUrl}/User/reject-cv/${userId}`, {});
    }

    getFavoriteJobs(): Observable<any[]> {
        return this.http.get<any[]>(`${this.baseUrl}/FollowJob/user-follows`);
    }

    getFavoriteCompanies(): Observable<any[]> {
        return this.http.get<any[]>(`${this.baseUrl}/FollowCompany/user-follows`);
    }
}