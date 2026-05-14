import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { catchError, Observable, of, tap } from 'rxjs';
import { LoginRequest, LoginResponse, RefreshTokenResponce } from '../../auth/auth.models';
import { TokenService } from './token.service';
import { environment } from 'src/environments/environment';
import { AuthStateService } from './auth-state.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private readonly apiUrl = `${environment.apiUrl}/api/auth`;

  constructor(
    private http: HttpClient,
    private tokenService: TokenService,
    private authStateService: AuthStateService
  ) { }

  login(request: LoginRequest): Observable<LoginResponse> {
    return this.http
      .post<LoginResponse>(`${this.apiUrl}/login`, request)
      .pipe(
        tap(response => {
          this.tokenService.saveTokens(response.accessToken, response.refreshToken);

          this.authStateService.setAuthenticated();
        })
      );
  }

  refreshToken(): Observable<RefreshTokenResponce> {
    const refreshToken = this.tokenService.getRefreshToken();

    return this.http.post<RefreshTokenResponce>(`${this.apiUrl}/refresh`, { refreshToken })
      .pipe(
        tap(response => {
          this.tokenService.saveTokens(response.accessToken, response.refreshToken);
        })
      );
  }

  register(request: LoginRequest): Observable<void> {
    return this.http.post<void>(`${this.apiUrl}/register`, request);
  }

  logout(): Observable<void> {
    const refreshToken = this.tokenService.getRefreshToken();

    if (!refreshToken) {
      this.tokenService.clearTokens();
      this.authStateService.clear();
      return of(void 0);
    }

    return this.http
      .post<void>(`${this.apiUrl}/logout`, { refreshToken })
      .pipe(
        tap(() => {
          this.tokenService.clearTokens();
          this.authStateService.clear();
        }),
        catchError(() => {
          this.tokenService.clearTokens();
          this.authStateService.clear();
          return of(void 0);
        })
      );
  }

  isAuthenticated(): boolean {
    return this.tokenService.isLoggedIn();
  }
}
