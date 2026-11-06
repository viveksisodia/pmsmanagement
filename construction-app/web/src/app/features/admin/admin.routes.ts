import { Routes } from '@angular/router';
import { People, Profiles } from './admin';

export const ADMIN_ROUTES: Routes = [
  { path: 'people', component: People, title: 'People' },
  { path: 'profiles', component: Profiles, title: 'Profile permissions' },
  { path: '', pathMatch: 'full', redirectTo: 'profiles' }
];
