import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from './core/guards';

const routes: Routes = [
  {
    path: '',
    pathMatch: 'full',
    redirectTo: 'sample'
  },
  {
    path: 'account',
    loadChildren: () => import('./features/identity/identity.module').then(m => m.IdentityModule)
  },
  {
    path: 'sample',
    loadChildren: () => import('./features/sample/sample.module').then(m => m.SampleModule),
    canActivate: [AuthGuard],
    canLoad: [AuthGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
