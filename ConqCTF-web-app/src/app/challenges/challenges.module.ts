import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { ChallengesRoutingModule } from './challenges-routing.module';
import { ChallengeListComponent } from './pages/challenge-list/challenge-list.component';
import { ChallengeDetailsComponent } from './pages/challenge-details/challenge-details.component';
import { MaterialModule } from '../shared/material.module';
import { FormsModule } from '@angular/forms';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    ChallengeListComponent,
    ChallengeDetailsComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    ChallengesRoutingModule,
    FormsModule,
    MaterialModule
  ]
})
export class ChallengesModule { }
