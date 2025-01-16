import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { MessageService } from 'primeng/api';
import jwt_decode from 'jwt-decode';
import { SocialAuthService } from '@abacritt/angularx-social-login';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers:[MessageService]
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string | null = null;
  errorMsg  = "";
  role : string='';
  loading :boolean=false;

  constructor(private fb: FormBuilder,
     private router: Router,
     private authService : AuthService,
     private messageService: MessageService,
     private googleService : SocialAuthService
     ) {
     this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      this.authService.login(this.loginForm.value).subscribe({
        next: async (response) => {
          const token = response.data;
          localStorage.setItem("jwt", token);
          const decodedToken: any = jwt_decode(token);
          
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Login Successful' });
          this.role = this.authService.getRole();

          if (this.role.toLowerCase() === 'admin')
          {
            await new Promise<void>(resolve => setTimeout(resolve, 1000));
            this.router.navigate(['admin']);
          } 

          else if (this.role.toLocaleLowerCase() === 'user')
          {
            await new Promise<void>(resolve => setTimeout(resolve, 1000));
            this.router.navigate(['dashboard']);
          }

          else
          {
            console.error('Unknown role:', decodedToken.role);
            this.messageService.add({ severity: 'error', summary: 'Error', detail: 'Unknown role detected.' });
          }
          
        },

        error: (err) => {
          this.errorMsg = err.error.errorMessage;
          this.messageService.add({ severity: 'error', summary: 'Success', detail: this.errorMsg });
        }

      });
    } else {
    }
    }

  onSignUp() {
    this.router.navigate(['/signup']);
  }

  ngOnInit(): void {
    
      this.googleService.authState.subscribe({
        next:(result) =>{
          this.loading = true;
          const idToken = result.idToken;
         this.authService.verifyToken(idToken).subscribe({
          next:async (result)=>{
            localStorage.setItem("jwt", result.data);
                      const decodedToken: any = jwt_decode(result.data);
                      
                      this.role = this.authService.getRole();
            
                      if (this.role.toLowerCase() === 'admin')
                      {
                        await new Promise<void>(resolve => setTimeout(resolve, 1000));
                        this.loading = false;
                        this.router.navigate(['admin']);
                      } 
            
                      else if (this.role.toLocaleLowerCase() === 'user')
                      {
                        await new Promise<void>(resolve => setTimeout(resolve, 1000));
                        this.loading =false;
                        this.router.navigate(['dashboard']);
                      }
            
                      else
                      {
                        console.error('Unknown role:', decodedToken.role);
                      }
                      
          },
          error:(err)=>{
          }
         })
        },
        error:(err)=>{
        }
      })
  }
}
