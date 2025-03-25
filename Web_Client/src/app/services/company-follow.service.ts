import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { CompanyFollowResponse } from '../../core/models/company-follow.model';


@Injectable({
    providedIn: 'root'
})
export class CompanyFollowService {
    private baseUrl = "https://localhost:7247/api";

    constructor(private http: HttpClient) { }

    toggleFollow(companyId: number): Observable<CompanyFollowResponse> {
        return this.http.post<CompanyFollowResponse>(
            `${this.baseUrl}/FollowCompany/toggle/${companyId}`,
            {}
        );
    }

    getFollowedCompanies(): Observable<any[]> {
        return this.http.get<any[]>(`${this.baseUrl}/FollowCompany/user-follows`);
    }

    checkFollowStatus(companyId: number): Observable<boolean> {
        return this.http.get<boolean>(`${this.baseUrl}/FollowCompany/check/${companyId}`);
    }
} 