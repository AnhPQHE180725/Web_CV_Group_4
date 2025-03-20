import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { Category } from '../models/Category';


@Injectable({
    providedIn: 'root'
})

export class CategoryService {
    private baseUrl = "https://localhost:7247/api";
    constructor(private http: HttpClient) { }

    getTopCategories(): Observable<Category[]> {
        return this.http.get<Category[]>(`${this.baseUrl}/Category/get-top-categories`)
    }

    getAllCategories(): Observable<Category[]> {
        return this.http.get<Category[]>(`${this.baseUrl}/Category/get-all-categories`)
    }
}