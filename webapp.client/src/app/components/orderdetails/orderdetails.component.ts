import { Component, OnInit } from '@angular/core';
import { OrderdetailsService } from 'src/app/services/orderdetails.service';

@Component({
  selector: 'app-orderdetails',
  templateUrl: './orderdetails.component.html',
  styleUrls: ['./orderdetails.component.css']
})
export class OrderdetailsComponent implements OnInit{
 orders :any; 
 visible : boolean = false;

 constructor(private orderService: OrderdetailsService)
 {
 }

 ngOnInit(): void {
   this.orderService.getOrders().subscribe({
    next : (response)=>{
      this.orders = response.data;
    },
    error :(err)=>{
    }
   })
 }
  
 showDialogue()
 {
  this.visible = true;
 }
 
}
