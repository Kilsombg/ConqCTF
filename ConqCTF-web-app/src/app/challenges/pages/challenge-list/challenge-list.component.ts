import { Component, OnInit } from '@angular/core';
import { ChallengesService } from '../../services/challenges.service';
import { ChallengeDto, PaginatedList } from '../../models/challenge.models';
import { MatDialog } from '@angular/material/dialog';
import { ChallengeDetailsComponent } from '../challenge-details/challenge-details.component';
import { CHALLENGE_CATEGORIES, CHALLENGE_DIFFICULTIES, getCategoryLabel, getDifficultyLabel } from '../../constants/challenge.constants';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {

  challenges?: PaginatedList<ChallengeDto>;
  pageNumber = 1;

  categories = CHALLENGE_CATEGORIES;
  difficulties = CHALLENGE_DIFFICULTIES;

  constructor(
    private challengesService: ChallengesService,
    private dialog: MatDialog
  ) { }

  ngOnInit(): void {
    this.loadChallenges();
  }

  loadChallenges(): void {
    this.challengesService.getChallenges(this.pageNumber)
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
    this.dialog.open(ChallengeDetailsComponent, {
      width: '600px',
      data: { challengeId: id }
    });
  }
}