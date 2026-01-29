import { Component, Inject, OnInit, Optional } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChallengesService } from '../../services/challenges.service';
import { ChallengeDetailsDto } from '../../models/challenge.models';
import { MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CHALLENGE_CATEGORIES, CHALLENGE_DIFFICULTIES, getCategoryLabel, getDifficultyLabel } from '../../constants/challenge.constants';

@Component({
  selector: 'app-challenge-details',
  templateUrl: './challenge-details.component.html',
  styleUrls: ['./challenge-details.component.css']
})
export class ChallengeDetailsComponent implements OnInit {

  challenge?: ChallengeDetailsDto;
  flag = '';
  error?: string;
  success?: string;

  categories = CHALLENGE_CATEGORIES;
  difficulties = CHALLENGE_DIFFICULTIES;

  constructor(
    private route: ActivatedRoute,
    private challengesService: ChallengesService,
    @Optional() @Inject(MAT_DIALOG_DATA) public data?: { challengeId: number }
  ) { }

  ngOnInit(): void {
    let id: number | null = null;

    if (this.data?.challengeId) {
      id = this.data.challengeId;
    }

    if (!id) {
      id = Number(this.route.snapshot.paramMap.get('id'));
    }

    if (!id) {
      this.error = 'Invalid challenge';
      return;
    }

    this.challengesService.getChallenge(id).subscribe(challenge => this.challenge = challenge);
  }


  submitFlag(): void {
    if (!this.challenge) return;

    this.challengesService.submitFlag(this.challenge.id, this.flag)
      .subscribe({
        next: () => {
          this.success = 'Correct flag!';
          this.error = undefined;
        },
        error: err => {
          this.error = 'Incorrect flag';
          this.success = undefined;
        }
      });
  }

  download(fileName: string): void {
    if (!this.challenge) return;

    this.challengesService.downloadFile(this.challenge.id, fileName)
      .subscribe(blob => {
        const url = window.URL.createObjectURL(blob);
        const a = document.createElement('a');
        a.href = url;
        a.download = fileName;
        a.click();
        window.URL.revokeObjectURL(url);
      });
  }

  categoryLabel(value: number): string {
    return getCategoryLabel(value);
  }

  difficultyLabel(value: number): string {
    return getDifficultyLabel(value);
  }
}