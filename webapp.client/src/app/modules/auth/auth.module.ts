import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from 'src/app/components/login/login.component';
import { SignupComponent } from 'src/app/components/signup/signup.component';
import { AdmindashboardComponent } from 'src/app/components/admindashboard/admindashboard.component';
import { DashboardComponent } from 'src/app/components/dashboard/dashboard.component';
import { AuthGuard } from 'src/app/auth/auth.guard';
import { UserComponent } from 'src/app/components/user/user.component';
import { ProductComponent } from 'src/app/components/product/product.component';
import { CartComponent } from 'src/app/components/cart/cart.component';
import { OrderdetailsComponent } from 'src/app/components/orderdetails/orderdetails.component';
import { SidebarComponent } from 'src/app/components/sidebar/sidebar.component';
import { AboutComponent } from 'src/app/components/about/about.component';
import { ContactComponent } from 'src/app/components/contact/contact.component';
@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild([
      
      {path:'',component:LoginComponent},
      {path:'users',component:UserComponent},
      {path:'product',component:ProductComponent},
      {path:'cart',component:CartComponent  },
      {path:"orders", component:OrderdetailsComponent},
      {path:"sidebar", component:SidebarComponent},
      {path:'signup',component:SignupComponent},
      {path:'home',component:ProductComponent},
      {path:'about',component:AboutComponent},
      {path:'contact',component:ContactComponent},
      { 
        path: 'admin', 
        component: AdmindashboardComponent, 
        canActivate: [AuthGuard], 
        data: { role: 'Admin' } 
      },

      { 
        path: 'dashboard', 
        component: DashboardComponent, 
        canActivate: [AuthGuard], 
        data: { role: 'User' } 
      }

    ])
  ]
})
export class AuthModule { }
