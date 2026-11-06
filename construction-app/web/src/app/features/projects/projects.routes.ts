import { Routes } from '@angular/router';
import { ProjectList } from './project-list';
import { ProjectLayout } from './project-layout';
import { ProjectForm } from './project-form';

export const PROJECT_ROUTES: Routes = [
  { path: '', component: ProjectList, title: 'Projects' },
  { path: 'new', component: ProjectForm, title: 'New project' },
  {
    path: ':projectId', component: ProjectLayout,
    children: [
      { path: '', component: ProjectForm, title: 'Project details' },
      { path: 'structure', loadComponent: () => import('../structure/structure-page').then(m => m.StructurePage), title: 'Structure' },
      { path: 'units', loadComponent: () => import('../units/units-page').then(m => m.UnitsPage), title: 'Units' }
    ]
  }
];
