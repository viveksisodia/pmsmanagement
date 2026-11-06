import { Injectable, inject, signal } from '@angular/core';
import { Api } from '../../core/api.service';
import { ProjectDetail } from '../../core/models';

/** The open project, shared by the Details, Structure and Units tabs. */
@Injectable()
export class ProjectContext {
  private api = inject(Api);
  readonly project = signal<ProjectDetail | null>(null);
  async reload(): Promise<void> {
    const p = this.project();
    if (p) this.project.set(await this.api.project(p.id));
  }
}
