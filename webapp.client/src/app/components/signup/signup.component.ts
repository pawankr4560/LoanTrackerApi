import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { AuthService } from 'src/app/services/auth.service';
import { MessageService } from 'primeng/api';
import { Router } from '@angular/router';
@Component({
  selector: 'app-signup',
  templateUrl: './signup.component.html',
  styleUrls: ['./signup.component.css'],
  providers:[MessageService]
})
export class SignupComponent implements OnInit{
  signupForm: FormGroup;
  suggestions : any;
  addressData : any;
  constructor(private fb: FormBuilder,
     private authService: AuthService,
     private messageService : MessageService,
     private router : Router
  ) {
    this.signupForm = this.fb.group(
      {
        email: ['', [Validators.required, Validators.email]],
        password: ['', [Validators.required, Validators.minLength(8)]],
        confirmPassword: ['', Validators.required],
        firstName: ['', Validators.required],
        lastName: ['', Validators.required],
        gender: ['', Validators.required],
        phone: ['', [Validators.required, Validators.pattern(/^\d{10}$/)]],
        address: ['', Validators.required],
      },
      { validators: this.passwordMatchValidator }
    );
  }

  passwordMatchValidator(form: FormGroup) {
    const password = form.get('password')?.value;
    const confirmPassword = form.get('confirmPassword')?.value;
    return password === confirmPassword ? null : { passwordMismatch: true };
  }

  onSubmit() {
    if (this.signupForm.valid) {
      this.authService.signup(this.signupForm.value).subscribe({
        next: async (response) => {
          this.messageService.add({ severity: 'success', summary: 'Success', detail: 'User registered successfully.'});
            //Delay navigation by 1 second (1000ms)
            await new Promise<void>(resolve => setTimeout(resolve, 2000));
           this.router.navigate(['']);

        },
        error: (err) => {
          this.messageService.add({ severity: 'error', summary: 'Success', detail: 'err.message' });
        }
      });
    }

    else {

    }
  }

  getAddress(address: any)
  {
   this.authService.getAddressSuggestions(address.value).subscribe({
    next: async (response) => {
   
      if (response && response.data && Array.isArray(response.data.predictions)) {
        this.addressData = response.data; // Assign data to addressData
        this.suggestions = this.addressData.predictions.map((prediction: { description: any; }) => prediction.description); // Extract descriptions only
    
      } else {
        console.error('Predictions data not found in the response');
        this.suggestions = []; // Clear suggestions if no predictions are found
      }
    },
    error: (err) => {
      this.messageService.add({ severity: 'error', summary: 'Success', detail: 'err.message' });
    }
   })
  
  }

  ngOnInit(): void {
  
  }
  
  selectAddress(address: string): void {
    // Set the selected address to the address form field
    this.signupForm.controls['address'].setValue(address);
    this.suggestions = [];  // Clear suggestions after selection
  }
}
