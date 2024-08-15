import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuctionService } from '../../services/auction.service';
import { UserService } from '../../services/user.service';
import { JwtHelperService } from '@auth0/angular-jwt';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-auction',
  templateUrl: './create-auction.component.html',
  styleUrls: ['./create-auction.component.css']
})
export class CreateAuctionComponent implements OnInit {
  products: any[] = [];
  selectedProducts: any[] = [];
  auctionForm: FormGroup;
  userId?: number;
  currentDateTime?: string;

  constructor(
    private auctionService: AuctionService,
    private userService: UserService,
    private fb: FormBuilder,
    private router: Router
  ) {
    this.auctionForm = this.fb.group({
      title: ['', Validators.required],
      description: ['', Validators.required],
      startTime: ['', Validators.required],
      endTime: ['', Validators.required]
    });
  }

  ngOnInit(): void {
    
    const token = localStorage.getItem('token');
    if (token) {
      const jwtHelper = new JwtHelperService();
      const decodedToken = jwtHelper.decodeToken(token);
      const Id= decodedToken?.UserId;
      console.log("User Id in Create - Auction ", this.userId);
      this.userId = Id;
    }
    this.setCurrentDateTime();
    this.loadProducts();
  }

  loadProducts(): void {
    if (this.userId) {
      this.userService.getUnsoldProductsByUserId(this.userId).subscribe((products: any[]) => {
        this.products = products; // Assign unsold products to the component's products array
      }, error => {
        console.error('Error loading products:', error);
      });
    }
  }
  
  

  addToAuction(product: any): void {
    if (!this.selectedProducts.includes(product)) {
      this.selectedProducts.push(product);
    }
  }

  removeFromAuction(product: any): void {
    this.selectedProducts = this.selectedProducts.filter(p => p.id !== product.id);
  }

  createAuction(): void {
    if (this.auctionForm.valid && this.selectedProducts.length > 0) {
        const auctionData = {
            ...this.auctionForm.value,
            userId: this.userId,
            productIds: this.selectedProducts.map(p => p.id) // Pass only the IDs of the products
        };

        this.auctionService.createAuction(auctionData).subscribe(() => {
            alert('Auction created successfully');
            this.router.navigate(['/user-home']);
        }, (error) => {
            console.error('Error creating auction:', error);
            alert('There was an error creating the auction. Please try again.');
        });
    } else {
        alert('Please fill out all fields and select at least one product.');
    }
}
setCurrentDateTime() {
  const now = new Date();
  this.currentDateTime = now.toISOString().slice(0, 16); // Format to "YYYY-MM-DDTHH:MM"
}

}

