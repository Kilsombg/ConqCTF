import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LoadingChallengeComponent } from './components/loading-challenge/loading-challenge.component';
import { MaterialModule } from './material.module';


@NgModule({
  declarations: [
      LoadingChallengeComponent
    ],
  imports: [
    CommonModule,
    MaterialModule
  ],
  exports: [
    LoadingChallengeComponent
  ]
})
export class SharedModule { }
