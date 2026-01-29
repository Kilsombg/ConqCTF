import { Component } from '@angular/core';
import { ChallengesService } from '../../../challenges/services/challenges.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-challenge',
  templateUrl: './create-challenge.component.html',
  styleUrls: ['./create-challenge.component.css']
})
export class CreateChallengeComponent {
  
  error?: string;
  success?: string;

  constructor(
    private challengesService: ChallengesService,
    private router: Router
  ) { }

  submit(formData: FormData): void {
    this.challengesService.createChallenge(formData)
      .subscribe({
        next: id => {
          this.success = 'Challenge created successfully';
          this.router.navigate(['/challenges', id]);
        },
        error: err => {
          this.error = 'Failed to create challenge';
        }
      });
  }
}
