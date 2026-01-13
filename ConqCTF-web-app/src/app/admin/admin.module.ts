import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { AdminRoutingModule } from './admin-routing.module';
import { CreateChallengeComponent } from './pages/create-challenge/create-challenge.component';
import { FormsModule } from '@angular/forms';


@NgModule({
  declarations: [
    CreateChallengeComponent
  ],
  imports: [
    CommonModule,
    FormsModule,
    AdminRoutingModule
  ]
})
export class AdminModule { }
