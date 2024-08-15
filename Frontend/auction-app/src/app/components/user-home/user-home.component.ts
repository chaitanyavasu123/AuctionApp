import { Component, OnInit, OnDestroy } from '@angular/core';
import { AuctionService } from '../../services/auction.service';
import { BidService } from '../../services/bid.service';
import { UserService } from 'src/app/services/user.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Component({
  selector: 'app-user-home',
  templateUrl: './user-home.component.html',
  styleUrls: ['./user-home.component.css']
})
export class UserHomeComponent implements OnInit, OnDestroy {
  auctions: any[] = [];
  searchQuery: string = '';
  auctionsToParticipate:any[]=[];
  filteredAuctions: any[] = [];
  userId?: number;
  auctionTimers: any = {}; // Object to store remaining time for each auction
  private intervalId!: any;
  userBids:any[]=[];
  userAuctions:any=[];
  boughtProducts:any=[];

  constructor(private auctionService: AuctionService, private bidService: BidService, private userService: UserService,private router: Router) {}

  ngOnInit(): void {
    
    const token = localStorage.getItem('token');
    if (token) {
      const jwtHelper = new JwtHelperService();
      const decodedToken = jwtHelper.decodeToken(token);
      console.log('Decoded Token:', decodedToken); // Log the entire decoded token to inspect it
      const Role=decodedToken?.role;
      const Id = decodedToken?.UserId; // Check if 'UserId' exists in the token
      this.userId = Id;
      console.log("User Id", this.userId);
      console.log("User role : ", Role);
    }
    

    this.intervalId = setInterval(() => {
      this.updateRemainingTime();
    }, 1000);//updated every second
    this.loadAuctions();
    this.getUserBids(); 
    this.getUserAuction();
    this.getUserBoughtProducts();
  }

  ngOnDestroy(): void {
    if (this.intervalId) {
      clearInterval(this.intervalId);
    }
  }

  loadAuctions(): void {
    this.auctionService.getAllAuctions().subscribe((data: any[]) => {
      // Assuming `this.userId` is set to the current user's ID
      this.auctions = data;
      
      this.auctionsToParticipate = this.auctions.filter(auction => {
        // Check if the auction is active
        const isAuctionActive = new Date(auction.endTime) > new Date();
        
        // Check if the auction is not created by the current user
        const isNotCreatedByUser = auction.userId != this.userId;
        
        // Check if there are unsold products in the auction
        const hasUnsoldProducts = auction.products.some((product: { isSold: boolean; }) => !product.isSold);
        
        return isAuctionActive && isNotCreatedByUser && hasUnsoldProducts;
      });
      
      this.updateRemainingTime(); // Initial update of remaining time
    });
  }
  

  updateRemainingTime(): void {
    this.auctionsToParticipate.forEach(auction => {
      const remainingTime = this.calculateTimeRemaining(auction.endTime);
      this.auctionTimers[auction.id] = remainingTime;
    });
  }

  calculateTimeRemaining(endTime: string): string {
    const end = new Date(endTime).getTime();
    const now = new Date().getTime();
    const timeLeft = end - now;

    if (timeLeft <= 0) return 'Auction ended';

    const days = Math.floor(timeLeft / (1000 * 60 * 60 * 24));
    const hours = Math.floor((timeLeft % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((timeLeft % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((timeLeft % (1000 * 60)) / 1000);

    return `${days}d ${hours}h ${minutes}m ${seconds}s remaining`;
  }

  searchAuctions(): void {
    this.filteredAuctions = this.auctionsToParticipate.filter(auction =>
      auction.title.toLowerCase().includes(this.searchQuery.toLowerCase()) ||
      auction.products.some((product: { name: string; }) => product.name.toLowerCase().includes(this.searchQuery.toLowerCase()))
    );
  }

  filterAuctionsByPrice(order: string): void {
    if (order === 'asc') {
      this.filteredAuctions.sort((a, b) => this.getHighestBid(a) - this.getHighestBid(b));
    } else if (order === 'desc') {
      this.filteredAuctions.sort((a, b) => this.getHighestBid(b) - this.getHighestBid(a));
    }
  }

  filterAuctionsByTime(order: string): void {
    if (order === 'asc') {
      this.filteredAuctions.sort((a, b) => new Date(a.endTime).getTime() - new Date(b.endTime).getTime());
    } else if (order === 'desc') {
      this.filteredAuctions.sort((a, b) => new Date(b.endTime).getTime() - new Date(a.endTime).getTime());
    }
  }

  getHighestBid(auction: any): number {
    return Math.max(0, ...auction.products.map((p: { highestBid: any; }) => p.highestBid || 0));
  }

  bid(auction: any, product: any): void {
    const input = prompt(`Enter your bid amount for ${product.name} (Current highest bid: ${product.startingPrice})`);
  
    if (input !== null) {
      const bidAmount = parseFloat(input);
      if (bidAmount > product.startingPrice) {
        if (this.userId) {
          const bidData = { productId: product.id, bidAmount: bidAmount, userId: this.userId };
          this.bidService.addBid(bidData).subscribe(() => {
            // After a successful bid, reload auctions to show updated bids
            this.loadAuctions();
          }, (error) => {
            console.error("Error submitting bid:", error);
            alert("There was an issue submitting your bid. Please try again.");
          });
        }
      } else {
        alert('Your bid must be higher than the current highest bid.');
      }
    } else {
      // Handle the case where the user canceled the prompt
      alert("Bid canceled.");
    }
  }
  logout() {
    localStorage.removeItem('token');
    this.router.navigate(['']);
  }
  getUserBids(): void {
    if (this.userId) {
      this.bidService.getBidsByUserId(this.userId).subscribe((bids: any[]) => {
        this.userBids = bids; // Assign unsold products to the component's products array
      }, error => {
        console.error('Error loading products:', error);
      });
    }
  }
  getUserAuction(): void {
    if (this.userId) {
      this.auctionService.getAllAuctionsByUserId(this.userId).subscribe((auctions: any[]) => {
        this.userAuctions = auctions; // Assign unsold products to the component's products array
      }, error => {
        console.error('Error loading products:', error);
      });
    }
  }
  getUserBoughtProducts():void{
    if(this.userId){
      this.userService.getUserWithBoughtProducts(this.userId).subscribe((products:any[])=>{
        this.boughtProducts=products;
      }, error=>{
        console.error("Error in loading the User Bought Products:", error);
      }
      );
    }
  }
}


