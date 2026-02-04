import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LoadingChallengeComponent } from './loading-challenge.component';

describe('LoadingChallengeComponent', () => {
  let component: LoadingChallengeComponent;
  let fixture: ComponentFixture<LoadingChallengeComponent>;

  beforeEach(() => {
    TestBed.configureTestingModule({
      declarations: [LoadingChallengeComponent]
    });
    fixture = TestBed.createComponent(LoadingChallengeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
