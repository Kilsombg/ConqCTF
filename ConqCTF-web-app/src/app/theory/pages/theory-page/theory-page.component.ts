import { Component, HostListener } from '@angular/core';

@Component({
  selector: 'app-theory-page',
  templateUrl: './theory-page.component.html',
  styleUrls: ['./theory-page.component.css']
})
export class TheoryPageComponent {

  showScrollTop = false;

  @HostListener('window:scroll', [])
  onWindowScroll(): void {
    this.showScrollTop = window.scrollY > 300;
  }

  scrollToTop(): void {
    window.scrollTo({
      top: 0,
      behavior: 'smooth'
    });
  }
}
