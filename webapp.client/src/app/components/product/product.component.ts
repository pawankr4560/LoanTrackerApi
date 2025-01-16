import { Component, inject, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MessageService } from 'primeng/api';
import { Createorder } from 'src/app/interfaces/createorder';
import { Product } from 'src/app/interfaces/product';
import { OrderService } from 'src/app/services/order.service';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
  providers: [MessageService]
})
export class ProductComponent implements OnInit {

  products: any;
  data: Product[] = [];
  cartItems: any;
  visible: boolean = false;
  subtotal: number = 0;
  deliveryFee: number = 50; // Example static fee
  serviceFee: number = 20; // Example static fee
  totalFee: number = 0;
  orders: Createorder[] = [];

  constructor(private productService: ProductService,
    private messageService: MessageService,
    private router: Router,
    private orderService : OrderService
  ) 
  {
  }

  ngOnInit(): void {
    this.productService.getProducts().subscribe({
      next: (response) => {
        this.products = response.data;
      },
      error: (err) => {
      }
    })

  }

  addItem(product: any): void {
    const item = {
      id: product.id,
      name: product.name,
      price: product.price,
      quantity: 1,
      image: product.image,
      totalPrice: product.price
    };


    var result = localStorage.getItem('cart');
    if (result != null) {
      this.data = JSON.parse(result);
      const existingItem = this.data.find((item: any) => item.id === product.id);

      if (existingItem) {
        this.messageService.add({ severity: 'error', summary: 'Error', detail: 'This item is already present in the cart.' });
      }

      else {
        this.addItemInCart(item);
      }
    }
    else {
      this.addItemInCart(item);
    }
    this.calculateFees();
  }

  showDialog() {
    this.visible = true;
  }

  addItemInCart(item: any) {
    this.data.push(item);
    localStorage.setItem('cart', JSON.stringify(this.data));
    this.showDialog();
  }


  goToCart() {
    this.router.navigate(['cart']);
  }

  incrementQuantity(cart: Product): void {
    if (!cart.price || !cart.quantity) return;
    cart.quantity++;
    cart.totalPrice = cart.price * cart.quantity;
    this.updateCart();
  }


  decrementQuantity(cart: Product): void {
    if (!cart.price || !cart.quantity || cart.quantity <= 1) return;
    cart.quantity--;
    cart.totalPrice = cart.price * cart.quantity;
    this.updateCart();
  }

  updateCart(): void {
    localStorage.setItem('cart', JSON.stringify(this.data));
    this.calculateFees();
  }

  calculateFees(): void {
    this.subtotal = this.data.reduce((sum, item) => sum + (item.totalPrice || 0), 0);
   
  }

  removeItem(item: Product): void {
    const index = this.data.findIndex(x => x['id'] === item['id']);
    if (index !== -1) {
      this.data.splice(index, 1); 
      this.updateLocalStorage(); 
    }
    this.subtotal = this.data.reduce((sum, item) => sum + (item.totalPrice || 0), 0);
  }

  updateLocalStorage(): void {
    localStorage.setItem('cart', JSON.stringify(this.data));
  }


  createOrder()
  {
    const storedOrders = localStorage.getItem('cart');
    this.orders = storedOrders ? JSON.parse(storedOrders) : []; 
    this.orderService.createOrder(this.orders).subscribe({
      next : async (response)=>{
       localStorage.clear();
       this.messageService.add({ severity: 'success', summary: 'Success', detail: 'Order created successfully.' });
       this.closeDialogue()
       await new Promise<void>(resolve => setTimeout(resolve, 1000));
       this.router.navigate(['orders']);
      },
      error : (err)=>{
      }
    })
  }

  closeDialogue()
  {
    this.visible = false;
    //this.router.navigate(['orderdetails']);
  }
}
