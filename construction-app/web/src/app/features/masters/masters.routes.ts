import { Routes } from '@angular/router';
import { LevelTypes, Locations, UnitTypes } from './masters';

export const MASTER_ROUTES: Routes = [
  { path: 'unit-types', component: UnitTypes, title: 'Unit types' },
  { path: 'level-types', component: LevelTypes, title: 'Floor level types' },
  { path: 'locations', component: Locations, title: 'Locations' },
  { path: '', pathMatch: 'full', redirectTo: 'unit-types' }
];
