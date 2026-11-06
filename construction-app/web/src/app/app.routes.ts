import { Routes } from '@angular/router';
import { permissionGuard, signedInGuard } from './core/http';
import { Shell } from './shell/shell';
import { SignIn } from './features/auth/sign-in';

export const routes: Routes = [
  { path: 'sign-in', component: SignIn, title: 'Sign in' },
  {
    path: '', component: Shell, canActivate: [signedInGuard],
    children: [
      { path: '', pathMatch: 'full', redirectTo: 'projects' },
      { path: 'projects', loadChildren: () => import('./features/projects/projects.routes').then(m => m.PROJECT_ROUTES) },
      { path: 'land', canActivate: [permissionGuard('land.view')], loadChildren: () => import('./features/land/land.routes').then(m => m.LAND_ROUTES) },
      { path: 'masters', loadChildren: () => import('./features/masters/masters.routes').then(m => m.MASTER_ROUTES) },
      { path: 'admin', canActivate: [permissionGuard('admin.roles')], loadChildren: () => import('./features/admin/admin.routes').then(m => m.ADMIN_ROUTES) },
      { path: 'account', loadComponent: () => import('./features/auth/account').then(m => m.Account), title: 'My account' }
    ]
  },
  { path: '**', redirectTo: '' }
];
