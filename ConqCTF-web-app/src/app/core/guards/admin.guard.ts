import { Injectable } from '@angular/core';
import {
  CanActivate,
  Router,
  ActivatedRouteSnapshot,
  RouterStateSnapshot
} from '@angular/router';
import { TokenService } from '../services/token.service';

@Injectable({
  providedIn: 'root'
})
export class AdminGuard implements CanActivate {

  constructor(
    private tokenService: TokenService,
    private router: Router
  ) {}

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): boolean {
    if (!this.tokenService.getAccessToken()) {
      this.router.navigate(['/auth/login']);
      return false;
    }

    if (!this.tokenService.hasRole('Administrator')) {
      this.router.navigate(['/forbidden']);
      return false;
    }

    return true;
  }
}