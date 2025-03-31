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
    getAllRecruitments(): Observable<Recruitment[]> {
        return this.http.get<Recruitment[]>(`${this.baseUrl}/Recruitment/get-all-recruitments`)
    }
    getRecruitmentsByCompanyName(company: string): Observable<any> {
        return this.http.get(`${this.baseUrl}/Recruitment/get-recruitments-by-company-name/${company}`);
    }

    getRecruitmentsByTitle(title: string): Observable<any> {
        return this.http.get(`${this.baseUrl}/Recruitment/get-recruitments-by-title/${title}`);
    }

    getRecruitmentsByLocation(location: string): Observable<any> {
        return this.http.get(`${this.baseUrl}/Recruitment/get-recruitments-by-location/${location}`);
    }
    addRecruitment(recruitment: Partial<Recruitment>): Observable<any> {
        return this.http.post(`${this.baseUrl}/Recruitment/add-recruitment`, recruitment);
    }

    editRecruitment(id: number, recruitment: Partial<Recruitment>): Observable<any> {
        return this.http.put(`${this.baseUrl}/Recruitment/edit-recruitment/${id}`, recruitment);
    }

    deleteRecruitment(id: number): Observable<any> {
        return this.http.delete(`${this.baseUrl}/Recruitment/delete-recruitment/${id}`);
    }

    getRecruitmentById(id: number): Observable<any> {
        return this.http.get(`${this.baseUrl}/Recruitment/get-recruitment-by-id/${id}`)
    }

    incrementView(id: number): Observable<any> {
        return this.http.put(`${this.baseUrl}/Recruitment/increment-view/${id}`, {});
    }

    updateView(id: number): Observable<any> {
        return this.http.put(`${this.baseUrl}/Recruitment/update-view/${id}`, {});
    }

}