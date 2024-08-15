import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuctionService {
  private baseUrl = 'https://localhost:7144/api/Auction'; // Base URL for the AuctionController

  constructor(private http: HttpClient) {}

  // Method to create an auction
  createAuction(auctionData: any): Observable<any> {
    return this.http.post(`${this.baseUrl}`, auctionData, this.getAuthHeaders());
  }

  // Method to delete an auction by ID
  deleteAuction(id: number): Observable<any> {
    return this.http.delete(`${this.baseUrl}/${id}`, this.getAuthHeaders());
  }

  // Method to update an auction by ID
  updateAuction(id: number, auctionData: any): Observable<any> {
    return this.http.put(`${this.baseUrl}/${id}`, auctionData, this.getAuthHeaders());
  }

  // Method to get all auctions
  getAllAuctions(): Observable<any> {
    return this.http.get(`${this.baseUrl}`, this.getAuthHeaders());
  }

  // Method to get all auctions by user ID
  getAllAuctionsByUserId(userId: number): Observable<any> {
    return this.http.get(`${this.baseUrl}/ByUserId/${userId}`, this.getAuthHeaders());
  }

  // Private method to get HTTP options including headers
  private getAuthHeaders() {
    const token = localStorage.getItem('token');
    return {
      headers: new HttpHeaders({
        Authorization: `Bearer ${token}`
      })
    };
  }
}
