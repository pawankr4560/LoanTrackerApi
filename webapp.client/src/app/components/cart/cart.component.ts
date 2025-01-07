import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/interfaces/product';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css']
})
export class CartComponent implements OnInit{
  products: Product[] = [];
  subtotal : number= 0;
constructor(private http:HttpClient,
  private productService : ProductService
) {
 
}

ngOnInit(): void {
  const res = localStorage.getItem('Products');
  this.products = res ? JSON.parse(res) : [];
  
  this.subtotal = this.products.reduce((sum: number, product: any) => sum + (product.price * (product.quantity || 1)), 0);
  
  console.log(this.products);
  console.log('Subtotal:', this.subtotal);
}

updateCart(item:any)
{
  debugger;
  console.log(item);
}
}
