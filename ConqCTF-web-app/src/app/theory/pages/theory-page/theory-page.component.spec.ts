import { ComponentFixture, TestBed } from '@angular/core/testing';

import { TheoryPageComponent } from './theory-page.component';

describe('TheoryPageComponent', () => {
  let component: TheoryPageComponent;
  let fixture: ComponentFixture<TheoryPageComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [TheoryPageComponent]
    });
    fixture = TestBed.createComponent(TheoryPageComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
