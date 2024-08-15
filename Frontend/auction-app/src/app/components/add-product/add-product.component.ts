import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ProductService } from '../../services/product.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-add-product',
  templateUrl: './add-product.component.html',
  styleUrls: ['./add-product.component.css']
})
export class AddProductComponent {
  product: any = {
    name: '',
    description: '',
    startingPrice: 0,
    // reservedPrice: 0,
    category: '',
    auctionDuration: 0,
    sellerId: null,
    isSold:false
  };
  serverErrorMessage:string="";
  constructor(private productService: ProductService, private router: Router) {}

  ngOnInit(): void {
    const token = localStorage.getItem('token');
    if (token) {
      const jwtHelper = new JwtHelperService();
      const decodedToken = jwtHelper.decodeToken(token);
      const Id = decodedToken?.UserId; 
      this.product.sellerId=Id;  // Set the seller ID
    }
  }

  onSubmit(): void {
    this.productService.addProduct(this.product).subscribe(() => {
      console.log('Product added successfully');
      this.router.navigate(['/user-home']); // Navigate to user-home after adding
    }, error => {
      if (error.status === 400 && error.error && error.error.errors) {
        const validationErrors = error.error.errors;
        this.serverErrorMessage = 'Please Enter necessary details..';
      } else {
        this.serverErrorMessage = 'Please enter necessary details..';
      }
    });
  }
}

