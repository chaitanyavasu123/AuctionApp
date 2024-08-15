import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { MatButtonModule } from '@angular/material/button';
import { MatInputModule } from '@angular/material/input';
import { MatCardModule } from '@angular/material/card';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatDialogModule } from '@angular/material/dialog';
import { JwtModule,JwtHelperService } from '@auth0/angular-jwt';
import { RouterModule, Routes } from '@angular/router';
import { ReactiveFormsModule } from '@angular/forms';
import { FormsModule } from '@angular/forms';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { UserHomeComponent } from './components/user-home/user-home.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { CreateAuctionComponent } from './components/create-auction/create-auction.component';
import { AdminHomeComponent } from './components/admin-home/admin-home.component';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { HttpClientModule } from '@angular/common/http';

import { UserService } from './services/user.service';
import { BidComponent } from './components/bid/bid.component';
import { ViewParticipationComponent } from './components/view-participation/view-participation.component';

// Define the token getter function

@NgModule({
  declarations: [
    AppComponent,
    HomeComponent,
    LoginComponent,
    UserHomeComponent,
    AddProductComponent,
    CreateAuctionComponent,
    AdminHomeComponent,
    BidComponent,
    ViewParticipationComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    ReactiveFormsModule,
    BrowserAnimationsModule,
    MatButtonModule,
    MatInputModule,
    MatCardModule,
    MatToolbarModule,
    MatDialogModule,
    HttpClientModule,
    FormsModule
  ],
  providers: [JwtHelperService],
  bootstrap: [AppComponent]
})
export class AppModule { }
