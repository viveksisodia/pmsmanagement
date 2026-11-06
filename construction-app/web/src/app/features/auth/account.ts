import { Component, inject } from '@angular/core';
import { AuthService } from '../../core/auth.service';

@Component({
  selector: 'app-account',
  template: `
    <h1>My account</h1>
    <div class="card narrow">
      <dl class="ro">
        <dt>Name</dt><dd>{{ auth.user()?.fullName }}</dd>
        <dt>Username</dt><dd class="mono">{{ auth.user()?.userName }}</dd>
        <dt>Email</dt><dd>{{ auth.user()?.email || '—' }}</dd>
        <dt>Company</dt><dd>{{ auth.user()?.tenantName }}</dd>
        <dt>Profile</dt><dd>{{ auth.user()?.profileName || '—' }}</dd>
      </dl>
      <p class="hint">You sign in with your ERP username and password. Change your password or profile in the ERP.</p>
    </div>`
})
export class Account { auth = inject(AuthService); }
