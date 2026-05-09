import { Injectable } from '@angular/core';
import {
  HttpEvent,
  HttpHandler,
  HttpInterceptor,
  HttpRequest,
  HttpErrorResponse
} from '@angular/common/http';
import {
  BehaviorSubject,
  Observable,
  catchError,
  filter,
  switchMap,
  take,
  throwError
} from 'rxjs';
import { AuthService } from '../services/auth.service';
import { TokenService } from '../services/token.service';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  private isRefreshing = false;
  private refreshTokenSubject = new BehaviorSubject<string | null>(null);

  constructor(
    private authService: AuthService,
    private tokenService: TokenService
  ) {}

  intercept(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    if (this.isAuthEndpoint(request)) {
      return next.handle(request);
    }

    const accessToken = this.tokenService.getAccessToken();

    let authRequest = request;
    if (accessToken) {
      authRequest = request.clone({
        withCredentials: true,
        setHeaders: { Authorization: `Bearer ${accessToken}` }
      });
    }

    return next.handle(authRequest).pipe(
      catchError(error => {
        if (error instanceof HttpErrorResponse && error.status === 401) {
          return this.handle401(authRequest, next);
        }
        return throwError(() => error);
      })
    );
  }

  private handle401(
    request: HttpRequest<any>,
    next: HttpHandler
  ): Observable<HttpEvent<any>> {

    if (!this.isRefreshing) {
      this.isRefreshing = true;
      this.refreshTokenSubject.next(null);

      return this.authService.refreshToken().pipe(
        switchMap(response => {
          this.isRefreshing = false;
          this.refreshTokenSubject.next(response.accessToken);

          return next.handle(
            request.clone({
              setHeaders: {
                Authorization: `Bearer ${response.accessToken}`
              }
            })
          );
        }),
        catchError(err => {
          this.isRefreshing = false;
          this.tokenService.clearTokens();
          return throwError(() => err);
        })
      );
    }

    return this.refreshTokenSubject.pipe(
      filter(token => token !== null),
      take(1),
      switchMap(token =>
        next.handle(
          request.clone({
            setHeaders: { Authorization: `Bearer ${token}` }
          })
        )
      )
    );
  }

  private isAuthEndpoint(request: HttpRequest<any>): boolean {
    return request.url.includes('/auth/login')
        || request.url.includes('/auth/refresh');
  }
}
