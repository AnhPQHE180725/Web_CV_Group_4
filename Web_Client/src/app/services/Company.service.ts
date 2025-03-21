import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Company } from '../models/Company';


@Injectable({
    providedIn: 'root'
})

export class CompanyService {
    private baseUrl = "https://localhost:7247/api";
    constructor(private http: HttpClient) { }

    getTopCompanies(): Observable<Company[]> {
        return this.http.get<Company[]>(`${this.baseUrl}/Company/get-top-companies`)
    }
    getAllCompanies(): Observable<Company[]> {
        return this.http.get<Company[]>(`${this.baseUrl}/Company/get-all-companies`)
    }
    getCompaniesByName(name : string): Observable<Company[]>{
        return this.http.get<Company[]>(`${this.baseUrl}/Company/get-companies-by-name/${name}`)
    }
}