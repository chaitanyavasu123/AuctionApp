import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class BidService {
  private baseUrl = 'https://localhost:7144/api/Bid'; // Base URL for the BidController

  constructor(private http: HttpClient) {}

  // Method to get bids by product ID
  getBidsByProductId(productId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/ByProductId/${productId}`, this.getAuthHeaders());
  }

  // Method to get bids by user ID
  getBidsByUserId(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/ByUserId/${userId}`, this.getAuthHeaders());
  }

  // Method to add a new bid
  addBid(bidData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}/AddBid`, bidData, this.getAuthHeaders());
  }

  // Method to delete a bid by ID
  deleteBid(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, this.getAuthHeaders());
  }

  // Method to update a bid by ID
  updateBid(id: number, bidData: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, bidData, this.getAuthHeaders());
  }

  // Method to get all bids
  getAllBids(): Observable<any> {
    return this.http.get(`${this.baseUrl}`, this.getAuthHeaders());
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
