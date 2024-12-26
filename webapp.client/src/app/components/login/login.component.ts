import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { MessageService } from 'primeng/api';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css'],
  providers:[MessageService]
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  errorMessage: string | null = null;
  errorMsg  = ""
  constructor(private fb: FormBuilder,
     private router: Router,
     private authService : AuthService,
     private messageService: MessageService) {
     this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(6)]],
    });
  }

  onSubmit() {
    if (this.loginForm.valid) {
      
      this.authService.login(this.loginForm.value).subscribe({

        next: async (response) => {
          console.log(response.data);
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Login Successfull' });
        },

        error: (err) => {
          debugger
          this.errorMsg = err.error.errorMessage;
          console.log('Login failed:', err);
          this.messageService.add({ severity: 'error', summary: 'Success', detail: 'this.errorMsg' });
        }

      });
    } else {
      console.log('Form is invalid:', this.loginForm);
    }
    }

  onSignUp() {
    // Navigate to the registration page
    this.router.navigate(['/signup']);
  }
  

  ngOnInit(): void {
    
  }
}
