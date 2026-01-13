import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChallengesRoutingModule } from './challenges-routing.module';
import { ChallengeListComponent } from './pages/challenge-list/challenge-list.component';
import { ChallengeDetailsComponent } from './pages/challenge-details/challenge-details.component';


@NgModule({
  declarations: [
    ChallengeListComponent,
    ChallengeDetailsComponent
  ],
  imports: [
    CommonModule,
    ChallengesRoutingModule
  ]
})
export class ChallengesModule { }
