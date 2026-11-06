import { Component, computed, inject } from '@angular/core';
import { RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { AuthService } from '../core/auth.service';

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  template: `
    <header class="topbar">
      <div class="title"><img src="brand/logo.png" alt="Absolute ERP" /> Construction <small class="hint">· {{ auth.user()?.tenantName }}</small></div>
      <div class="who">
        <span>{{ auth.user()?.profileName }}</span>
        <a routerLink="/account" [attr.aria-label]="'My account: ' + auth.user()?.fullName">{{ auth.user()?.fullName }}</a>
        <span class="avatar" aria-hidden="true">{{ initials() }}</span>
        <button type="button" class="sm" (click)="auth.signOut()">Sign out</button>
      </div>
    </header>
    <div class="shell">
      <nav class="side" aria-label="Main">
        <div class="grp">Projects</div>
        <a routerLink="/projects" routerLinkActive="on">Projects</a>
        @if (auth.can('land.view')) { <a routerLink="/land" routerLinkActive="on">Land parcels</a> }
        <div class="grp">Masters</div>
        <a routerLink="/masters/unit-types" routerLinkActive="on">Unit types</a>
        <a routerLink="/masters/level-types" routerLinkActive="on">Floor level types</a>
        <a routerLink="/masters/locations" routerLinkActive="on">Locations</a>
        @if (auth.can('admin.roles')) {
          <div class="grp">Administration</div>
          <a routerLink="/admin/profiles" routerLinkActive="on">Profile permissions</a>
          <a routerLink="/admin/people" routerLinkActive="on">People</a>
        }
      </nav>
      <main class="content"><router-outlet /></main>
    </div>`
})
export class Shell {
  auth = inject(AuthService);
  initials = computed(() => (this.auth.user()?.fullName ?? '?').split(/\s+/).filter(Boolean).slice(0, 2).map(w => w[0].toUpperCase()).join(''));
}
