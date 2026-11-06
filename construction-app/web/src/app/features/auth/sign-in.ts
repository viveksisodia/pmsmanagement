import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from '../../core/auth.service';
import { errorText } from '../../core/http';

/** Looks like the Absolute ERP login page (login.aspx): same background, logo, box and button colours. */
@Component({
  selector: 'app-sign-in',
  imports: [ReactiveFormsModule],
  template: `
    <div class="erp-login">
      <img class="erp-logo" src="brand/logo.png" alt="Absolute ERP" width="250" />
      <form [formGroup]="form" (ngSubmit)="submit()" class="erp-box" novalidate>
        <p class="erp-sub">Construction · sign in with your ERP username and password</p>
        <div class="erp-field">
          <span class="erp-addon" aria-hidden="true">
            <svg viewBox="0 0 24 24" width="14" height="14"><path fill="currentColor" d="M12 12a5 5 0 1 0 0-10 5 5 0 0 0 0 10Zm0 2c-4.4 0-8 2.2-8 5v3h16v-3c0-2.8-3.6-5-8-5Z"/></svg>
          </span>
          <input id="si-user" formControlName="userName" placeholder="username" aria-label="Username" autocomplete="username" />
        </div>
        <div class="erp-field">
          <span class="erp-addon" aria-hidden="true">
            <svg viewBox="0 0 24 24" width="14" height="14"><path fill="currentColor" d="M17 9V7A5 5 0 0 0 7 7v2H5v13h14V9h-2Zm-8-2a3 3 0 0 1 6 0v2H9V7Z"/></svg>
          </span>
          <input id="si-pw" type="password" formControlName="password" placeholder="password" aria-label="Password" autocomplete="current-password" />
        </div>
        @if (error()) { <p class="erp-err" role="alert">{{ error() }}</p> }
        <button type="submit" class="erp-btn" [disabled]="busy()">{{ busy() ? 'SIGNING IN…' : 'LOG IN' }}</button>
        <p class="erp-note">Forgot your password? Reset it from the ERP login page.</p>
      </form>
      <p class="erp-copy">Copyright © Absolute ERP | All Rights Reserved.</p>
    </div>`
})
export class SignIn {
  private auth = inject(AuthService);
  private router = inject(Router);
  form = inject(FormBuilder).nonNullable.group({ userName: ['', Validators.required], password: ['', Validators.required] });
  busy = signal(false);
  error = signal('');

  constructor() { if (this.auth.signedIn()) this.router.navigateByUrl('/projects'); }

  async submit() {
    if (this.form.invalid) { this.error.set('Enter your username and password.'); return; }
    this.busy.set(true); this.error.set('');
    try {
      await this.auth.signIn(this.form.getRawValue().userName, this.form.getRawValue().password);
      await this.router.navigateByUrl('/projects');
    } catch (e) { this.error.set(errorText(e)); }
    finally { this.busy.set(false); }
  }
}
