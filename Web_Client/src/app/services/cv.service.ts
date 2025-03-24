import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
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
    getCVUrl(userId: number): Observable<string> {
        return this.http.get<{ id: number, name: string, userId: number }>(`${this.baseUrl}/CV/user/${userId}`)
            .pipe(
                map(response => `${this.baseUrl}/CV/view/${response.id}`)
            );
    }
} 