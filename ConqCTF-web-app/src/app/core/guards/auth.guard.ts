import { Injectable } from '@angular/core';
import {
  CanActivate,
  ActivatedRouteSnapshot,
  RouterStateSnapshot,
  Router
} from '@angular/router';
import { AuthService } from '../services/auth.service';
import { TokenService } from '../services/token.service';
import { catchError, map, Observable, of } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {

  constructor(
    private authService: AuthService,
    private tokenService: TokenService,
    private router: Router
  ) {}

  canActivate(
    route: ActivatedRouteSnapshot,
    state: RouterStateSnapshot
  ): Observable<boolean> {

    const accessToken = this.tokenService.getAccessToken();

    if(!accessToken){
      this.redirectToLogin();
      return of(false);
    }

    if(!this.tokenService.isAccessTokenExpired()) {
      return of(true);
    }

    return this.authService.refreshToken()
      .pipe(
        map(responce => {
          this.tokenService.saveTokens(
            responce.accessToken,
            this.tokenService.getRefreshToken()!
          );
          return true;
        }),
        catchError(() => {
          this.authService.logout().subscribe();
          this.redirectToLogin();
          return of(false);
        })
      );
  }

  private redirectToLogin() : void {
    this.router.navigate(['/auth/login']);
  }
}
