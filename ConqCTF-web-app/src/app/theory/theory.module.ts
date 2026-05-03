import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TheoryPageComponent } from './pages/theory-page/theory-page.component';
import { SharedModule } from '../shared/shared.module';
import { TheoryRoutingModule } from './theory-routing.module';
import { MaterialModule } from '../shared/material.module';



@NgModule({
  declarations: [
    TheoryPageComponent
  ],
  imports: [
    CommonModule,
    TheoryRoutingModule,
    SharedModule,
    MaterialModule
  ]
})
export class TheoryModule { }
