import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { LoginComponent } from 'src/app/components/login/login.component';
import { SignupComponent } from 'src/app/components/signup/signup.component';



@NgModule({
  declarations: [],
  imports: [
    CommonModule,

    RouterModule.forChild([
      
      {path:'',component:LoginComponent},

      {path:'signup',component:SignupComponent},
     
    ])
  ]
})
export class AuthModule { }
