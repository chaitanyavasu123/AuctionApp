import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuctionService } from '../../services/auction.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  auctions: any[] = [];

  constructor(private auctionService: AuctionService, private router: Router) { }

  ngOnInit(): void {
    this.loadOngoingAuctions();
  }

  loadOngoingAuctions(): void {
    this.auctionService.getAllAuctions().subscribe(
      (data: any[]) => {
        this.auctions = data;
      },
      error => {
        console.error('Error fetching auctions', error);
      }
    );
  }

  navigateToLogin(): void {
    this.router.navigate(['/login']);
  }
}

