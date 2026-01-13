import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { TokenService } from './token.service';

export interface AuthUser {
  email?: string;
  roles: string[];
}

@Injectable({
  providedIn: 'root'
})
export class AuthStateService {

  private readonly userSubject = new BehaviorSubject<AuthUser | null>(null);

  user$: Observable<AuthUser | null> = this.userSubject.asObservable();

  constructor(private tokenService: TokenService) {
    this.loadUserFromToken();
  }

  private loadUserFromToken(): void {
    const payload = this.tokenService.getTokenPayload();
    if (!payload) {
      this.userSubject.next(null);
      return;
    }

    this.userSubject.next({
      email: payload.email,
      roles: this.tokenService.getUserRoles()
    });
  }

  setAuthenticated(): void {
    this.loadUserFromToken();
  }

  clear(): void {
    this.userSubject.next(null);
  }

  isAdmin(): boolean {
    const user = this.userSubject.value;
    return !!user && user.roles.includes('Administrator');
  }
}