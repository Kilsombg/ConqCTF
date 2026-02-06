import { Component, OnInit, ViewChild } from '@angular/core';
import { ChallengesService } from '../../services/challenges.service';
import { ChallengeDto, PaginatedList } from '../../models/challenge.models';
import { MatDialog } from '@angular/material/dialog';
import { ChallengeDetailsComponent } from '../challenge-details/challenge-details.component';
import { CHALLENGE_CATEGORIES, CHALLENGE_DIFFICULTIES, getCategoryLabel, getDifficultyLabel } from '../../constants/challenge.constants';
import { AuthStateService } from 'src/app/core/services/auth-state.service';
import { Router } from '@angular/router';
import { MatSelectionList } from '@angular/material/list';
import { ChallengeEventsService } from '../../services/challenge-events.service';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {

  @ViewChild('categoryList') categoryList!: MatSelectionList;
  @ViewChild('difficultyList') difficultyList!: MatSelectionList;
  @ViewChild('statusList') statusList!: MatSelectionList;

  challenges?: PaginatedList<ChallengeDto>;
  pageNumber = 1;
  pageSize = 10;

  isEditMode = false;
  isAdmin = false;

  categories = CHALLENGE_CATEGORIES;
  difficulties = CHALLENGE_DIFFICULTIES;

  selectedCategory?: number;
  selectedDifficulty?: number;
  selectedStatus?: 'solved' | 'unsolved';

  constructor(
    private challengesService: ChallengesService,
    private authStateService: AuthStateService,
    private challengeEvents: ChallengeEventsService,
    private dialog: MatDialog,
    private router: Router
  ) {
    this.isAdmin = this.authStateService.isAdmin();
  }

  ngOnInit(): void {
    this.loadChallenges();

    this.challengeEvents.challengeSolved$
      .subscribe(challengeId => {
        const challenge = this.challenges?.items
          .find(c => c.id == challengeId);

        if (challenge) challenge.isSolved = true;
      })
  }

  loadChallenges(): void {
    this.challengesService.getChallenges(
      this.pageNumber,
      this.pageSize,
      {
        category: this.selectedCategory,
        difficulty: this.selectedDifficulty,
        status: this.selectedStatus
      }
    )
      .subscribe(result => this.challenges = result);
  }

  onPageChange(event: any): void {
    this.pageNumber = event.pageIndex + 1;
    this.loadChallenges();
  }

  categoryLabel(value: number): string {
    return getCategoryLabel(value);
  }

  difficultyLabel(value: number): string {
    return getDifficultyLabel(value);
  }

  openChallenge(id: number): void {
    if (this.isAdmin && this.isEditMode) {
      this.router.navigate(['/admin/challenges', id, 'edit']);
    } else {
      const dialogRef = this.dialog.open(ChallengeDetailsComponent, {
        width: '600px',
        data: { challengeId: id }
      });

      dialogRef.afterClosed().subscribe(result => {
        if (result?.deleted) {
          this.loadChallenges();
        }
      });
    }
  }

  toggleMode(): void {
    this.isEditMode = !this.isEditMode;
  }

  onCategoryChange(event: any): void {
    const option = event.options[0];

    this.selectedCategory = option.selected
      ? option.value
      : undefined;

    this.pageNumber = 1;
    this.loadChallenges();
  }

  onDifficultyChange(event: any): void {
    const option = event.options[0];

    this.selectedDifficulty = option.selected
      ? option.value
      : undefined;

    this.pageNumber = 1;
    this.loadChallenges();
  }

  onStatusChange(event: any): void {
    const option = event.options[0];

    this.selectedStatus = option.selected
      ? option.value
      : undefined;

    this.pageNumber = 1;
    this.loadChallenges();
  }

  clearFilters(): void {
    this.selectedCategory = undefined;
    this.selectedDifficulty = undefined;
    this.selectedStatus = undefined;

    this.categoryList?.deselectAll();
    this.difficultyList?.deselectAll();
    this.statusList?.deselectAll();

    this.pageNumber = 1;
    this.loadChallenges();
  }

  hasActiveFilters(): boolean {
    return !!(
      this.selectedCategory ||
      this.selectedDifficulty ||
      this.selectedStatus
    );
  }
}