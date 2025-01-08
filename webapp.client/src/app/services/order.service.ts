import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Createorder } from '../interfaces/createorder';

@Injectable({
  providedIn: 'root'
})
export class OrderService {

get apiUrl(): string {
    return environment.apiUrl;
  }
  get apiKey(): string {
    return environment.apiKey;
  }
  
  private headers!: HttpHeaders;

  constructor(private http: HttpClient) {
    this.headers = new HttpHeaders({ 'Content-Type': 'application/json; charset=utf-8' });
  }


  createOrder(order : any)
  {
    return this.http.post<any>(`${this.apiUrl}/api/order/createOrder`, order,{headers:this.headers});
  }

}
