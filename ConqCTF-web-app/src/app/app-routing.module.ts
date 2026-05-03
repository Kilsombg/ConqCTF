import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards/auth.guard';
import { AdminGuard } from './core/guards/admin.guard';

const routes: Routes = [
  {
    path: 'auth',
    loadChildren: () =>
      import('./auth/auth.module').then(m => m.AuthModule)
  },
  {
    path: 'challenges',
    canActivate: [AuthGuard],
    loadChildren: () =>
      import('./challenges/challenges.module').then(m => m.ChallengesModule)
  },
  {
    path: 'admin',
    canActivate: [AuthGuard, AdminGuard],
    loadChildren: () =>
      import('./admin/admin.module').then(m => m.AdminModule),
  },
  {
    path: 'theory',
    loadChildren: () =>
      import('./theory/theory.module').then(m => m.TheoryModule)
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
  imports: [RouterModule.forRoot(routes, {
    anchorScrolling: 'enabled',
    scrollPositionRestoration: 'enabled'
  })],
  exports: [RouterModule]
})
export class AppRoutingModule { }