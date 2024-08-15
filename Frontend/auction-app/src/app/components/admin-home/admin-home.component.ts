import { Component, OnInit } from '@angular/core';
import { AuctionService } from '../../services/auction.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-admin-home',
  templateUrl: './admin-home.component.html',
  styleUrls: ['./admin-home.component.css']
})
export class AdminHomeComponent implements OnInit {
  ongoingAuctions: any[] = [];
  completedAuctions: any[] = [];

  constructor(private auctionService: AuctionService, private router: Router) {}

  ngOnInit(): void {
    this.loadAuctions();
  }

  loadAuctions(): void {
    this.auctionService.getAllAuctions().subscribe(auctions => {
      const currentTime = new Date().getTime();
      this.ongoingAuctions = auctions.filter((auction: { endTime: string | number | Date; }) => new Date(auction.endTime).getTime() > currentTime);
      this.completedAuctions = auctions.filter((auction: { endTime: string | number | Date; }) => new Date(auction.endTime).getTime() <= currentTime);
    });
  }

  deleteAuction(auctionId: number): void {
    if (confirm('Are you sure you want to delete this auction?')) {
      this.auctionService.deleteAuction(auctionId).subscribe(() => {
        alert('Auction deleted successfully');
        this.loadAuctions();
      }, error => {
        console.error('Error deleting auction:', error);
        alert('There was an error deleting the auction. Please try again.');
      });
    }
  }

  // suspendAuction(auctionId: number): void {
  //   if (confirm('Are you sure you want to suspend this auction?')) {
  //     this.auctionService.suspendAuction(auctionId).subscribe(() => {
  //       alert('Auction suspended successfully');
  //       this.loadAuctions();
  //     }, error => {
  //       console.error('Error suspending auction:', error);
  //       alert('There was an error suspending the auction. Please try again.');
  //     });
  //   }
  // }

  // banAuction(auctionId: number): void {
  //   if (confirm('Are you sure you want to ban this auction?')) {
  //     this.auctionService.banAuction(auctionId).subscribe(() => {
  //       alert('Auction banned successfully');
  //       this.loadAuctions();
  //     }, error => {
  //       console.error('Error banning auction:', error);
  //       alert('There was an error banning the auction. Please try again.');
  //     });
  //   }
  // }

  viewUserParticipation(auctionId: number): void {
    this.router.navigate(['/view-participation', auctionId]);
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['']);
  }
}

