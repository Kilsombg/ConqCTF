import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChallengesService } from '../../services/challenges.service';
import { ChallengeDetailsDto } from '../../models/challenge.models';

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

  constructor(
    private route: ActivatedRoute,
    private challengesService: ChallengesService
  ) {}

  ngOnInit(): void {
    const id = Number(this.route.snapshot.paramMap.get('id'));
    this.challengesService.getChallenge(id)
      .subscribe(challenge => this.challenge = challenge);
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
}