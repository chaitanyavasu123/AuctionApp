import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private baseUrl = 'https://localhost:7144/api/User'; // Base URL for the UserController

  constructor(private http: HttpClient) {}

  // Method to authenticate a user
  authenticate(model: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/authenticate`, model);
  }

  // Method to get bought products for a user by their ID
  getUserWithBoughtProducts(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${userId}/bought-products`, this.getAuthHeaders());
  }

  // Method to get sold products for a user by their ID
  getUserWithSoldProducts(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${userId}/sold-products`, this.getAuthHeaders());
  }

  // Method to get not-sold products for a user by their ID
  getUnsoldProductsByUserId(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${userId}/not-sold-products`, this.getAuthHeaders());
  }

  // Private method to get Authorization headers
  private getAuthHeaders() {
    const token = localStorage.getItem('token'); // Retrieve token from local storage
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}` // Set the Authorization header with Bearer token
      })
    };
  }
}
