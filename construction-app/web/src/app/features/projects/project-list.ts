import { Component, inject, signal } from '@angular/core';
import { RouterLink } from '@angular/router';
import { Api } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { AreaPipe, DatePipe } from '../../core/format';
import { errorText } from '../../core/http';
import { ProjectListItem } from '../../core/models';

@Component({
  selector: 'app-project-list',
  imports: [RouterLink, AreaPipe, DatePipe],
  template: `
    <div class="bar"><div class="grow"><h1>Projects</h1>
      <p class="lead">Each project sits on one or more land parcels and is split into phases, towers or blocks, floors and units.</p></div>
      @if (auth.can('projects.edit')) { <a class="btn pri" routerLink="/projects/new">New project</a> }</div>
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="tw"><table>
      <thead><tr><th>Code</th><th>Project</th><th>Developer</th><th>Type</th><th>Status</th><th>City</th><th>Start</th><th>Target</th><th class="num">Land</th><th class="num">Buildings</th><th class="num">Units</th></tr></thead>
      <tbody>@for (p of rows(); track p.id) {
        <tr><td class="mono">{{ p.code }}</td><td><a [routerLink]="['/projects', p.id]">{{ p.name }}</a></td><td>{{ p.developerEntity }}</td><td>{{ p.projectType }}</td>
          <td>{{ p.status }}</td><td>{{ p.city || '—' }}</td><td>{{ p.startDate | d }}</td><td>{{ p.targetDate | d }}</td><td class="num">{{ p.landSqm | area }}</td>
          <td class="num"><a [routerLink]="['/projects', p.id, 'structure']">{{ p.buildings }}</a></td><td class="num"><a [routerLink]="['/projects', p.id, 'units']">{{ p.units }}</a></td></tr>
      } @empty { <tr><td colspan="11" class="hint">{{ loading() ? 'Loading…' : 'No projects you can see yet.' }}</td></tr> }</tbody>
    </table></div>`
})
export class ProjectList {
  auth = inject(AuthService); private api = inject(Api);
  rows = signal<ProjectListItem[]>([]); loading = signal(true); error = signal('');
  constructor() { this.api.projects().then(r => this.rows.set(r)).catch(e => this.error.set(errorText(e))).finally(() => this.loading.set(false)); }
}
