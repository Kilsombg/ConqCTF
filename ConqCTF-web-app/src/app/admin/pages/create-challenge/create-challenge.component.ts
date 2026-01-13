import { Component } from '@angular/core';
import { ChallengesService } from '../../../challenges/services/challenges.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-challenge',
  templateUrl: './create-challenge.component.html',
  styleUrls: ['./create-challenge.component.css']
})
export class CreateChallengeComponent {

  title = '';
  description = '';
  category = 0;
  difficulty = 0;
  points = 0;
  flag = '';

  files: File[] = [];

  error?: string;
  success?: string;

  constructor(
    private challengesService: ChallengesService,
    private router: Router
  ) {}

  onFilesSelected(event: Event): void {
    const input = event.target as HTMLInputElement;
    if (!input.files) return;

    this.files = Array.from(input.files);
  }

  submit(): void {
    const formData = new FormData();

    formData.append('Title', this.title);
    formData.append('Description', this.description);
    formData.append('Category', this.category.toString());
    formData.append('Difficulty', this.difficulty.toString());
    formData.append('Points', this.points.toString());
    formData.append('Flag', this.flag);

    this.files.forEach(file =>
      formData.append('Files', file, file.name)
    );

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
