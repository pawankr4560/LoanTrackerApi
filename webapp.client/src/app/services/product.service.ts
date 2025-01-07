import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CreateProductRequestModel, UpdateProductRequestModel } from '../interfaces/create-product-request-model';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class ProductService {
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
  
  addProduct(product: CreateProductRequestModel): Observable<any> {
    const formData = new FormData();
    if (product.profileImage) {
      formData.append('ProfileImage', product.profileImage, product.profileImage.name);
    }

    formData.append('Name', product.name);
    formData.append('Description', product.description);
    formData.append('Categorie', product.categorie);
    formData.append('CreatedOn', product.createdOn.toISOString());
    formData.append('IsActive', product.isActive.toString());
    formData.append('IsDeleted', product.isDeleted.toString());
    formData.append('Price', product.price.toString());

    return this.http.post<any>('https://localhost:7176/api/Product/AddProduct', formData, {
      reportProgress: true,
      observe: 'events',
    });
  }
  
  updateProduct(product: UpdateProductRequestModel): Observable<any> {
    const formData = new FormData();
    if (product.profileImage) {
      formData.append('ProfileImage', product.profileImage, product.profileImage.name);
    }

    formData.append('Id', product.id);
    formData.append('Name', product.name);
    formData.append('Description', product.description);
    formData.append('Categorie', product.categorie);
    formData.append('CreatedOn', product.createdOn.toISOString());
    formData.append('IsActive', product.isActive.toString());
    formData.append('IsDeleted', product.isDeleted.toString());
    formData.append('Price', product.price.toString());

    return this.http.put<any>('https://localhost:7176/api/Product/UpdateProduct', formData, {
      reportProgress: true,
      observe: 'events',
    });
  }

  getProducts():Observable<any>
  {
    return this.http.get<any>('https://localhost:7176/api/Product/ProductList', {headers:this.headers});
  }
}
