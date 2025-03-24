import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class CvService {
    private baseUrl = "https://localhost:7247/api";

    constructor(private http: HttpClient) { }

    uploadCV(file: File): Observable<any> {
        const formData = new FormData();
        formData.append('file', file);

        return this.http.post(`${this.baseUrl}/CV/upload`, formData);
    }
} 