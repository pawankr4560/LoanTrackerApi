import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable, OnInit } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class OrderdetailsService  {
get apiUrl(): string {
    return environment.apiUrl;
  }
  get apiKey(): string {
    return environment.apiKey;
  }
  
  private headers!: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({
      'Content-Type': 'application/json; charset=utf-8',
      'api_key': this.apiKey, 
    });
  }
 

  getOrders()
  {
    return this.http.get<any>(`${this.apiUrl}/api/order/orderList`,{headers:this.headers});
  }
}
