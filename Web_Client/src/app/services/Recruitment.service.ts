import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';

import { Recruitment } from '../models/Recruitment';


@Injectable({
    providedIn: 'root'
})

export class RecruitmentService {
    private baseUrl = "https://localhost:7247/api";
    constructor(private http: HttpClient) { }

    getTopRecruitments(): Observable<Recruitment[]> {
        return this.http.get<Recruitment[]>(`${this.baseUrl}/Recruitment/get-top-recruitments`)
    }

    getRecruitmentsByCompany(id: number): Observable<Recruitment[]> {
        return this.http.get<Recruitment[]>(`${this.baseUrl}/Recruitment/get-recruitments-by-company-id/${id}`)
    }

    getRecruitmentsByCategory(id: number): Observable<Recruitment[]> {
        return this.http.get<Recruitment[]>(`${this.baseUrl}/Recruitment/get-recruitments-by-category-id/${id}`)
    }
}