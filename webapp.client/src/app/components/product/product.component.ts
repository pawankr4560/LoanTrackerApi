import { Component, OnInit } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
 products : any;
 constructor(private productService : ProductService) {
  
 }

 ngOnInit(): void {
   this.productService.getProducts().subscribe({
    next : (response)=>{
     this.products = response.data;
     console.log(this.products);
    },
    error : (err)=>{
      console.log(err);
    }
   })
 }
}
