import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { LoginComponent } from './components/login/login.component';
import { UserHomeComponent } from './components/user-home/user-home.component';
import { AddProductComponent } from './components/add-product/add-product.component';
import { CreateAuctionComponent } from './components/create-auction/create-auction.component';
import { AdminHomeComponent } from './components/admin-home/admin-home.component';
import { ViewParticipationComponent } from './components/view-participation/view-participation.component';  
import { BidComponent } from './components/bid/bid.component';
import { AuthGuard } from './guards/auth.guard';
import { adminGuard } from './guards/admin.guard';


const routes: Routes = [
  { path: 'home', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'user-home', component: UserHomeComponent, canActivate: [AuthGuard] },
  { path: 'add-product', component: AddProductComponent, canActivate: [AuthGuard] },
  { path: 'create-auction', component: CreateAuctionComponent, canActivate: [AuthGuard] },
  { path: 'admin-home', component: AdminHomeComponent, canActivate: [adminGuard] },
  { path: 'bid/:id', component: BidComponent },
  {path : 'view-participation/:id', component : ViewParticipationComponent},
  { path: '', redirectTo: '/home', pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
