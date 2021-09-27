
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { SigninPageComponent, SignupPageComponent } from './pages';
import { IdentityLayoutComponent } from '@innoshop/admin/core/layouts';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'signin'
  },
  {
    path: '',
    component: IdentityLayoutComponent,
    children: [
      {
        path: 'signin',
        component: SigninPageComponent
      },
      {
        path: 'signup',
        component: SignupPageComponent
      }]
  }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class IdentityRoutingModule { }
