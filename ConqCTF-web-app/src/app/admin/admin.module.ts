import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { CreateChallengeComponent } from './pages/create-challenge/create-challenge.component';
import { FormsModule } from '@angular/forms';
import { MaterialModule } from '../shared/material.module';
import { ChallengeFormComponent } from './components/challenge-form/challenge-form.component';
import { EditChallengeComponent } from './pages/edit-challenge/edit-challenge.component';
import { ConfirmDialogComponent } from './components/confirm-dialog/confirm-dialog.component';
import { SharedModule } from '../shared/shared.module';


@NgModule({
  declarations: [
    CreateChallengeComponent,
    ChallengeFormComponent,
    EditChallengeComponent,
    ConfirmDialogComponent
  ],
  imports: [
    CommonModule,
    SharedModule,
    FormsModule,
    AdminRoutingModule,
    MaterialModule
  ]
})
export class AdminModule { }
