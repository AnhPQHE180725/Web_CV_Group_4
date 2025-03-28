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
    getCompaniesByName(name: string): Observable<Company[]> {
        return this.http.get<Company[]>(`${this.baseUrl}/Company/get-companies-by-name/${name}`)
    }

    // Get companies owned by the currently logged-in user
    getUserCompanies(): Observable<Company[]> {
        return this.http.get<Company[]>(`${this.baseUrl}/Company/my-companies`);
    }

    // Get a single company by ID
    getCompanyById(id: number): Observable<Company> {
        return this.http.get<Company>(`${this.baseUrl}/Company/get-company/${id}`);
    }

    // Create a new company
    createCompany(company: Company, logoFile: File): Observable<any> {
        const formData = new FormData();
        formData.append('Name', company.name || '');
        formData.append('Description', company.description || '');
        formData.append('Address', company.address || '');
        formData.append('Email', company.email || '');
        formData.append('PhoneNumber', company.phoneNumber || '');
        formData.append('Status', (company.status ?? 0).toString());
        formData.append('Logo', logoFile);
        return this.http.post<any>(`${this.baseUrl}/Company/create-company`, formData);
    }

    // Update an existing company
    updateCompany(company: Company, logoFile?: File): Observable<any> {
        const formData = new FormData();
        formData.append('Id', (company.id ?? 0).toString());
        formData.append('Name', company.name || '');
        formData.append('Description', company.description || '');
        formData.append('Address', company.address || '');
        formData.append('Email', company.email || '');
        formData.append('PhoneNumber', company.phoneNumber || '');
        formData.append('Status', (company.status ?? 0).toString());
        if (logoFile) {
            formData.append('Logo', logoFile);
        }
        return this.http.put<any>(`${this.baseUrl}/Company/update-company`, formData);
    }

    // Delete a company
    deleteCompany(id: number): Observable<any> {
        return this.http.delete<any>(`${this.baseUrl}/Company/delete-company/${id}`);
    }
}