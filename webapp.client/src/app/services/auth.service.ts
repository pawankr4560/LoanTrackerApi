import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthenticatedResponse, LoginModel } from '../interfaces/login-model';
import { catchError, map, Observable, throwError } from 'rxjs';
import { SignupModel } from '../interfaces/signup-model';
import jwt_decode from 'jwt-decode';
import { JwtHelperService } from "@auth0/angular-jwt";

@Injectable({
  providedIn: 'root'
})
export class AuthService {

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

  login(credentials: LoginModel): Observable<any> {
    return this.http.post<AuthenticatedResponse>(
      `${this.apiUrl}/api/Auth/login`,
      credentials,
      { headers: this.headers }
    ).pipe(
      catchError((error: HttpErrorResponse) => {
        console.error('Login API error:', error);
        return throwError(() => new Error(error.message || 'Server error occurred.'));
      })
    );
  }

  signup(request: SignupModel): Observable<any> {
    return this.http.post<any>(`${this.apiUrl}/api/Auth/signup`, request,{headers:this.headers});
  }
  
  getAddressSuggestions(address: string): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/api/Auth/getaddress?address=${encodeURIComponent(address)}`, 
      { headers: this.headers }
    );
  }

  getRole() {
    var validToken = localStorage.getItem("jwt");
    if (validToken == null)
       {
      return false;
    }
    const decodeToken: any = jwt_decode(validToken);
    return decodeToken["Role"].toLowerCase();
  }

  get loginRequired(): boolean {
    var token = localStorage.getItem("jwt");
    if (token == null) {
      return true;
    }
    const helper = new JwtHelperService();
    const isExpired = helper.isTokenExpired(token!);
    return isExpired ? true : false;
  }

  verifyToken(idToken: string): Observable<any> {
    return this.http.get(
      `${this.apiUrl}/api/Auth/VerifyToken?idToken=${encodeURIComponent(idToken)}`,
      { headers: this.headers }
    );
  }
  getUsers(): Observable<any[]> {
    return this.http.get<any>(
      `${this.apiUrl}/api/Auth/UserList`,
      { headers: this.headers }
    );
  }


}
