import { Component, effect, inject, input, signal } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Api } from '../../core/api.service';
import { errorText } from '../../core/http';
import { ProjectDetail } from '../../core/models';
import { ProjectContext } from './project-context';

@Component({
  selector: 'app-project-layout',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  providers: [ProjectContext],
  template: `
    <div class="crumb"><a routerLink="/projects">Projects</a> › {{ ctx.project()?.name ?? '…' }}</div>
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <nav class="tabs" aria-label="Project sections">
      <a [routerLink]="['/projects', projectId()]" routerLinkActive="on" [routerLinkActiveOptions]="{ exact: true }">Details</a>
      <a [routerLink]="['/projects', projectId(), 'structure']" routerLinkActive="on">Structure</a>
      <a [routerLink]="['/projects', projectId(), 'units']" routerLinkActive="on">Units</a>
    </nav>
    <router-outlet />`
})
export class ProjectLayout {
  projectId = input.required<string>();
  ctx = inject(ProjectContext);
  private api = inject(Api);
  error = signal('');
  constructor() {
    effect(() => {
      const id = +this.projectId();
      this.api.project(id).then(p => { this.ctx.project.set(p); this.error.set(''); }).catch(e => this.error.set(errorText(e)));
    });
  }
}
