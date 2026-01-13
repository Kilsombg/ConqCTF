import { NgModule, Optional, SkipSelf } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';

import { JwtInterceptor } from './interceptors/jwt.interceptor';
import { NavbarComponent } from './components/navbar/navbar.component';
import { RouterModule } from '@angular/router';
import { CommonModule } from '@angular/common';



@NgModule({
 providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: JwtInterceptor,
      multi: true
    }
  ],
 declarations: [
   NavbarComponent
 ],
 imports: [
  CommonModule,
  RouterModule
 ],
 exports: [
  NavbarComponent
 ]
})
export class CoreModule { 
   constructor(@Optional() @SkipSelf() parentModule: CoreModule) {
    if (parentModule) {
      throw new Error('CoreModule should only be imported in AppModule');
    }
  }
}
