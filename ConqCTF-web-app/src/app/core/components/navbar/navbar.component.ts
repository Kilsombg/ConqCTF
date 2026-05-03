import { Component, HostListener } from '@angular/core';
import { AuthStateService, AuthUser } from '../../services/auth-state.service';
import { AuthService } from '../../services/auth.service';
import { Observable } from 'rxjs';
import { Router } from '@angular/router';
import { ThemeService } from '../../services/theme.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  user$: Observable<AuthUser | null>;

  isHidden = false;
  private lastScrollTop = 0;

  constructor(
    private authState: AuthStateService,
    private authService: AuthService,
    private themeService: ThemeService,
    private routing: Router
  ) {
    this.user$ = this.authState.user$;
  }

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    const currentScroll = window.scrollY;

    if (currentScroll <= 0) {
      // Always show at top
      this.isHidden = false;
      return;
    }

    // Scrolling DOWN - hide
    if (currentScroll > this.lastScrollTop && currentScroll > 20) {
      this.isHidden = true;
    } else {
      // Scrolling UP - show
      this.isHidden = false;
    }

    this.lastScrollTop = currentScroll;
  }


  logout(): void {
    this.authService.logout().subscribe(() => this.routing.navigate(['/auth/login']));
  }

  isAdmin(user: AuthUser): boolean {
    return !!user && user.roles.includes('Administrator');
  }

  toggleTheme(): void {
    this.themeService.toggle();
  }

  get isDark(): boolean {
    return this.themeService.isDark();
  }
}
