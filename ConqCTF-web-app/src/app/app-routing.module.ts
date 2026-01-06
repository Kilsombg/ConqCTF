import { inject, NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'challenges',
    canActivate : [AuthGuard],
    loadChildren: () =>
      import('./challenges/challenges.module').then(m => m.ChallengesModule)
  },
  {
    path: '',
    redirectTo: 'challenges',
    pathMatch: 'full'
  },
  {
    path: '**',
    redirectTo: 'challenges'
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule {}