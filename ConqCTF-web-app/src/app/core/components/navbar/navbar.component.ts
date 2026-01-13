import { Component } from '@angular/core';
import { AuthStateService, AuthUser } from '../../services/auth-state.service';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
user$: Observable<AuthUser | null>;

  constructor(
    private authState: AuthStateService,
    private authService: AuthService
  ) {
    this.user$ = this.authState.user$;
  }

  logout(): void {
    this.authService.logout().subscribe();
  }

  isAdmin(user: AuthUser): boolean {
    return !!user && user.roles.includes('Administrator');
  }
}
