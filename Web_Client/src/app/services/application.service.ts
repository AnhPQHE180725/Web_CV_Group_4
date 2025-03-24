import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
    providedIn: 'root'
})
export class ApplicationService {
    private baseUrl = "https://localhost:7247/api";

    constructor(private http: HttpClient) { }

    // Ứng tuyển với CV đã có
    applyWithExistingCV(recruitmentId: number, introduction?: string): Observable<any> {
        return this.http.post(`${this.baseUrl}/ApplyPost/apply-with-existing-cv`, {
            postId: recruitmentId,
            text: introduction
        });
    }

    // Ứng tuyển với CV mới
    applyWithNewCV(recruitmentId: number, cvFile: File, introduction?: string): Observable<any> {
        const formData = new FormData();
        formData.append('CVFile', cvFile);
        formData.append('PostId', recruitmentId.toString());
        if (introduction) {
            formData.append('Text', introduction);
        }

        return this.http.post(`${this.baseUrl}/ApplyPost/apply-with-new-cv`, formData);
    }
} 