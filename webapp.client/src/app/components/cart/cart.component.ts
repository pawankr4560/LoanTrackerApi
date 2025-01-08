import { Component, OnInit } from '@angular/core';
import { Product } from 'src/app/interfaces/product';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-cart',
  templateUrl: './cart.component.html',
  styleUrls: ['./cart.component.css'],
})
export class CartComponent implements OnInit {
  products: Product[] = [];
  subtotal: number = 0;
  deliveryFee: number = 50; // Example static fee
  serviceFee: number = 20; // Example static fee
  totalFee: number = 0;

  constructor(private productService: ProductService,
    private orderService : OrderService
  ) {}

  ngOnInit(): void {
    this.loadCartItems();
  }

  loadCartItems(): void {
    const storedItems = localStorage.getItem('cart');
    this.products = storedItems ? JSON.parse(storedItems) : [];
    this.calculateFees();
  }

  incrementQuantity(product: Product): void {
    if (!product.price || !product.quantity) return;
    product.quantity++;
    product.totalPrice = product.price * product.quantity;
    this.updateCart();
  }


  decrementQuantity(product: Product): void {
    if (!product.price || !product.quantity || product.quantity <= 1) return;
    product.quantity--;
    product.totalPrice = product.price * product.quantity;
    this.updateCart();
  }


  calculateFees(): void {
    this.subtotal = this.products.reduce((sum, item) => sum + (item.totalPrice || 0), 0);
    this.totalFee = this.subtotal + this.deliveryFee + this.serviceFee;
  }


  updateCart(): void {
    localStorage.setItem('cart', JSON.stringify(this.products));
    this.calculateFees();
  }
}
