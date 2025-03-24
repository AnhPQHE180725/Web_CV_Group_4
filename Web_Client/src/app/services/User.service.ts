import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { User } from '../models/User';


@Injectable({
    providedIn: 'root'
})

export class UserService {
    private baseUrl = "https://localhost:7247/api";
    constructor(private http: HttpClient) { }

    getUserByPostId(id: number): Observable<User[]> {
        return this.http.get<User[]>(`${this.baseUrl}/User/get-user-by-post-id/${id}`)
    }
}