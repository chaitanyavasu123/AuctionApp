import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { BidService } from '../../services/bid.service';
import { ProductService } from '../../services/product.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-bid',
  templateUrl: './bid.component.html',
  styleUrls: ['./bid.component.css']
})
export class BidComponent implements OnInit {
  product: any={};
  bidAmount: number=0;
  userId: number=0;

  constructor(
    private route: ActivatedRoute,
    private bidService: BidService,
    private productService: ProductService,
    private router: Router
  ) {
  }

  ngOnInit(): void {
    const productId: string | null =this.route.snapshot.paramMap.get('id');
    const token = localStorage.getItem('token');
    if(token){
    const jwtHelper = new JwtHelperService();
    const decodedToken = jwtHelper.decodeToken(token);
    const Id = decodedToken?.UserId;
    this.userId=Id;
    }
    console.log('User ID:', this.userId);
    // Check if productId is not null before converting it to a number
    if (productId !== null) {
        const productIdAsNumber = parseInt(productId, 10);
        this.productService.getProductById(productIdAsNumber).subscribe(product => {
          this.product = product;
        });
    } else {
        console.error("Product ID is null.");
    }
  
  }

  submitBid(): void {
    console.log('User ID:', this.userId);
    console.log('Current Product Starting Price:', this.product.startingPrice);
    console.log('Bid Amount:', this.bidAmount);
    if ((this.bidAmount > this.product.startingPrice) && this.userId) {
      const bidData = {
        productId: this.product.id,
        amount: this.bidAmount,
        userId: this.userId
      };
      console.log('Bid Data:', bidData);
      this.bidService.addBid(bidData).subscribe(() => {
        console.log('Bid submitted successfully');
        this.router.navigate(['/user-home']);
      });
    } else {
      alert('Your bid must be higher than the current highest bid.');
    }
  }
}

