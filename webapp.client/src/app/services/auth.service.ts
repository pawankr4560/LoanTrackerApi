import { HttpClient, HttpErrorResponse, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { AuthenticatedResponse, LoginModel } from '../interfaces/login-model';
import { catchError, Observable, throwError } from 'rxjs';
import { SignupModel } from '../interfaces/signup-model';

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

}
