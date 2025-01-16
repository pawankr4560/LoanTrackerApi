import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { map } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class CardService {
  token:string = localStorage.getItem("jwt")?? '';
get apiUrl(): string {
    return environment.apiUrl;
  }
  get apiKey(): string {
    return environment.apiKey;
  }
  
  private headers!: HttpHeaders;

  constructor(private router: Router, private http: HttpClient) { 
    this.headers = new HttpHeaders({'Content-Type': 'application/json; charset=utf-8'});
    this.headers = new HttpHeaders()
    .set('Authorization', `Bearer ${this.token}`)
    .set('Api_Key',`${this.apiKey}`);
}

getCards() {
  return this.http.get<any>(`${this.apiUrl}/api/stripe/listCard`, { headers: this.headers }).pipe(
    map((response: any) => {
      if (!response || !response.data) {
        return [];
      }
      return response.data.map((card: any) => ({
        expMonth: card.expMonth,
        expYear: card.expYear,
        isDefault: card.isDefault,
        id: card.id,
        customerId: card.customerId,
        last4: card.last4,
      }));
    })
  );
}

  defaultCard(cardId : string){
    return this.http.get<any>(`${this.apiUrl}/api/stripe/SetDefaultCard?cardId=${cardId}`,{headers:this.headers});
  }

  getdefaultCard(){
    return this.http.get<any>(`${this.apiUrl}/api/stripe/SetDefaultCard`,{headers:this.headers});
  }
}
