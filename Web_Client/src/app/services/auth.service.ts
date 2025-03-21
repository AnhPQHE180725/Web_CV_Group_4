import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
    private baseUrl = "https://localhost:7247/api";

  constructor() { }
}
