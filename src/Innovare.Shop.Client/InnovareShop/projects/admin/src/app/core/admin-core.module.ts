import { RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityLayoutComponent, PanelLayoutComponent } from './layouts';



@NgModule({
  declarations: [
    IdentityLayoutComponent,
    PanelLayoutComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ]
})
export class AdminCoreModule { }
