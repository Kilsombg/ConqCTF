import { Component } from '@angular/core';
import { ChallengesService } from '../../../challenges/services/challenges.service';
import { Router } from '@angular/router';
import { FeedbackService } from 'src/app/core/services/feedback.service';

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
    private feedback: FeedbackService,
    private router: Router
  ) { }

  submit(formData: FormData): void {
    this.challengesService.createChallenge(formData)
      .subscribe({
        next: id => {
          this.success = 'Challenge created successfully';
          this.router.navigate(['/challenges', id]);
          
          this.feedback.success(this.success!);
        },
        error: err => {
          this.error = 'Failed to create challenge';
          
          this.feedback.error(this.error!);
        }
      });
  }
}
