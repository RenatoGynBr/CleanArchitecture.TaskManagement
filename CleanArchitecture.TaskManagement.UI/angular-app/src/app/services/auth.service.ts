import { Injectable, signal, computed } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Router } from '@angular/router';
import { tap } from 'rxjs/operators';
import { environment } from '../../environments/environment';

import { 
  LoginRequest, 
  RegisterRequest, 
  AuthResponse 
} from '../models/auth.model';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  // Angular 20 uses Signals for state management
  private _currentUser = signal<AuthResponse | null>(this.getUserFromStorage());
  
  // Computed signal - automatically updates when _currentUser changes
  isLoggedIn = computed(() => this._currentUser() !== null);
  currentUser = this._currentUser.asReadonly();
  private apiUrl = `${environment.apiUrl}/Auth`;
  constructor(
    private http: HttpClient,
    private router: Router
  ) {}
  register(data: RegisterRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/register`, data)
      .pipe(
        tap(response => this.setSession(response))
      );
  }
  login(data: LoginRequest) {
    return this.http.post<AuthResponse>(`${this.apiUrl}/login`, data)
      .pipe(
        tap(response => this.setSession(response))
      );
  }
  logout() {
    localStorage.removeItem('auth_token');
    localStorage.removeItem('auth_user');
    this._currentUser.set(null);
    this.router.navigate(['/login']);
  }
  getToken(): string | null {
    return localStorage.getItem('auth_token');
  }
  private setSession(response: AuthResponse) {
    localStorage.setItem('auth_token', response.token);
    localStorage.setItem('auth_user', JSON.stringify(response));
    this._currentUser.set(response);
  }
  private getUserFromStorage(): AuthResponse | null {
    const user = localStorage.getItem('auth_user');
    return user ? JSON.parse(user) : null;
  }
}
