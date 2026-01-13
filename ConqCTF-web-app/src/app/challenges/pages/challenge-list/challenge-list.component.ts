import { Component, OnInit } from '@angular/core';
import { ChallengesService } from '../../services/challenges.service';
import { ChallengeDto, PaginatedList } from '../../models/challenge.models';

@Component({
  selector: 'app-challenge-list',
  templateUrl: './challenge-list.component.html',
  styleUrls: ['./challenge-list.component.css']
})
export class ChallengeListComponent implements OnInit {

  challenges?: PaginatedList<ChallengeDto>;
  pageNumber = 1;

  constructor(private challengesService: ChallengesService) {}

  ngOnInit(): void {
    this.loadChallenges();
  }

  loadChallenges(): void {
    this.challengesService.getChallenges(this.pageNumber)
      .subscribe(result => this.challenges = result);
  }

  next(): void {
    if (this.challenges?.hasNextPage) {
      this.pageNumber++;
      this.loadChallenges();
    }
  }

  previous(): void {
    if (this.challenges?.hasPreviousPage) {
      this.pageNumber--;
      this.loadChallenges();
    }
  }
}