import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { CreateChallengeComponent } from './pages/create-challenge/create-challenge.component';
import { AuthGuard } from '../core/guards/auth.guard';
import { AdminGuard } from '../core/guards/admin.guard';

const routes: Routes = [
   {
    path: 'challenges/create',
    component: CreateChallengeComponent,
    canActivate: [AuthGuard, AdminGuard]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class AdminRoutingModule { }
