import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from '../core/guards/auth.guard';
import { ChallengeListComponent } from './pages/challenge-list/challenge-list.component';
import { ChallengeDetailsComponent } from './pages/challenge-details/challenge-details.component';

const routes: Routes = [
    {
    path: '',
    component: ChallengeListComponent,
    canActivate: [AuthGuard]
  },
  {
    path: ':id',
    component: ChallengeDetailsComponent,
    canActivate: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class ChallengesRoutingModule { }
