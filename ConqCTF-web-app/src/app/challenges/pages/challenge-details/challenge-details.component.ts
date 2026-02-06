import { Component, Inject, OnInit, Optional } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ChallengesService } from '../../services/challenges.service';
import { ChallengeDetailsDto } from '../../models/challenge.models';
import { MAT_DIALOG_DATA, MatDialog, MatDialogRef } from '@angular/material/dialog';
import { CHALLENGE_CATEGORIES, CHALLENGE_DIFFICULTIES, getCategoryLabel, getDifficultyLabel } from '../../constants/challenge.constants';
import { AuthStateService } from 'src/app/core/services/auth-state.service';
import { ConfirmDialogComponent } from 'src/app/admin/components/confirm-dialog/confirm-dialog.component';
import { FeedbackService } from 'src/app/core/services/feedback.service';
import { ChallengeEventsService } from '../../services/challenge-events.service';

@Component({
  selector: 'app-challenge-details',
  templateUrl: './challenge-details.component.html',
  styleUrls: ['./challenge-details.component.css']
})
export class ChallengeDetailsComponent implements OnInit {

  challenge?: ChallengeDetailsDto;
  selectedHintIndex: number | null = null;
  flag = '';
  error?: string;
  success?: string;

  isAdmin = false;

  categories = CHALLENGE_CATEGORIES;
  difficulties = CHALLENGE_DIFFICULTIES;

  constructor(
    private route: ActivatedRoute,
    private challengesService: ChallengesService,
    private authStateService: AuthStateService,
    private feedback: FeedbackService,
    private challengeEvents: ChallengeEventsService,
    private router: Router,
    private dialog: MatDialog,
    @Optional() private dialogRef: MatDialogRef<ChallengeDetailsComponent>,
    @Optional() @Inject(MAT_DIALOG_DATA) public data?: { challengeId: number }
  ) {
    this.isAdmin = this.authStateService.isAdmin();
  }

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

  selectHint(index: number): void {
    this.selectedHintIndex =
      this.selectedHintIndex === index ? null : index;
  }


  submitFlag(): void {
    if (!this.challenge) return;

    this.challengesService.submitFlag(this.challenge.id, this.flag)
      .subscribe({
        next: () => {
          this.success = 'Correct flag!';
          this.error = undefined;

          this.feedback.success(this.success);
          this.challengeEvents.notifySolved(this.challenge!.id);;
        },
        error: err => {
          this.error = err.error;
          this.success = undefined;

          this.feedback.error(this.error!);
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

  edit(): void {
    if (!this.challenge) return;

    this.dialogRef?.close();
    this.router.navigate(['/admin/challenges', this.challenge?.id, 'edit']);
  }

  confirmDelete(): void {
    const ref = this.dialog.open(ConfirmDialogComponent, {
      width: '400px'
    });

    ref.afterClosed().subscribe(confirmed => {
      if (confirmed) {
        this.delete();
      }
    });
  }

  private delete(): void {
    if (!this.challenge) return;

    this.challengesService.deleteChallenge(this.challenge.id).subscribe({
      next: () => {
        this.dialogRef.close({deleted : true});
      },
      error: () => {
      }
    });
  }
}