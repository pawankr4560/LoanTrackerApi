import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/interfaces/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css']
})
export class ProductComponent implements OnInit {
 products : any;
 data: Product[] = [];
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

 addItem(product: any): void {
  debugger;
  const item = {
    name: product.name,
    price: product.price,
    quantity: 1,
    image: product.image,
  };


  var result = localStorage.getItem('Products');
  if(result != null)
  {
    this.data = JSON.parse(result);
    this.data.push(item);
  }
  else{
    this.data.push(item);
  }
  localStorage.setItem('Products', JSON.stringify(this.data));
}

}
