import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
  private baseUrl = 'https://localhost:7144/api/Product'; // Base URL for the ProductController

  constructor(private http: HttpClient) {}

  // Method to get all products
  getAllProducts(): Observable<any> {
    return this.http.get(`${this.baseUrl}`, this.getAuthHeaders());
  }

  // Method to get a product by its ID
  getProductById(id: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/${id}`, this.getAuthHeaders());
  }

  // Method to add a new product
  addProduct(productData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddProduct`, productData, this.getAuthHeaders());
  }

  // Method to update an existing product by its ID
  updateProduct(id: number, productData: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, productData, this.getAuthHeaders());
  }

  // Method to delete a product by its ID
  deleteProduct(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, this.getAuthHeaders());
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

