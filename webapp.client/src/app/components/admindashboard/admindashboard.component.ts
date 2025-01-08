import { Component } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-admindashboard',
  templateUrl: './admindashboard.component.html',
  styleUrls: ['./admindashboard.component.css']
})
export class AdmindashboardComponent {
  productForm : FormGroup
  categories = [
    { id: '1', name: 'Electronics' },
    { id: '2', name: 'Books' },
    { id: '3', name: 'Clothing' },
    { id: '4', name: 'Home Appliances' },
  ];

  constructor(private productService : ProductService,
    private fb : FormBuilder
  ) {
    this.productForm = this.fb.group({
      name: ['', Validators.required],
      categorie: ['', Validators.required],
      description: ['', Validators.required],
      price: [0, [Validators.required, Validators.min(0)]],
      profileImage: [null, Validators.required],
      createdOn: [new Date, Validators.required],
      isActive: [true, Validators.required],
      isDeleted: [false, Validators.required],
    });
  }
  

  onFileChange(event: any, productForm: FormGroup) {
    const file = event.target.files[0];
    if (file) {
      productForm.patchValue({ profileImage: file }); // Update form value
    }
  }

  onSubmit() {
    if (this.productForm.valid) {
      this.productService.addProduct(this.productForm.value).subscribe({
        next: (response) => {
          alert('Product added successfully!');
        },
        error: (err) => {
          console.error(err);
          alert('Failed to add product.');
        },
      });
    }
  }
}
