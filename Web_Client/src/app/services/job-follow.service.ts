import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class JobFollowService {
    private baseUrl = "https://localhost:7247/api";

    constructor(private http: HttpClient) { }

    toggleFollow(jobId: number): Observable<any> {
        return this.http.post<any>(
            `${this.baseUrl}/FollowJob/toggle/${jobId}`,
            {}
        );
    }

    getFollowedJobs(): Observable<any[]> {
        return this.http.get<any[]>(`${this.baseUrl}/FollowJob/user-follows`);
    }
} 