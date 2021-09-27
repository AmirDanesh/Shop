import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { IdentityRoutingModule } from './identity-routing.module';
import { SigninComponent, SignupComponent } from './components';
import { SigninPageComponent, SignupPageComponent } from './pages';
import { AdminCoreModule } from '@innoshop/admin/core/admin-core.module';


@NgModule({
  declarations: [
    SigninComponent,
    SignupComponent,
    SignupPageComponent,
    SigninPageComponent
  ],
  imports: [
    CommonModule,
    AdminCoreModule,
    IdentityRoutingModule
  ]
})
export class IdentityModule { }
