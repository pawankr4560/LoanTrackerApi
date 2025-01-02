import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from 'src/app/components/login/login.component';
import { SignupComponent } from 'src/app/components/signup/signup.component';
import { AdmindashboardComponent } from 'src/app/components/admindashboard/admindashboard.component';
import { DashboardComponent } from 'src/app/components/dashboard/dashboard.component';
import { AuthGuard } from 'src/app/auth/auth.guard';


@NgModule({
  declarations: [],
  imports: [
    CommonModule,
    RouterModule.forChild([
      
      {path:'',component:LoginComponent},

      {path:'signup',component:SignupComponent},
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
