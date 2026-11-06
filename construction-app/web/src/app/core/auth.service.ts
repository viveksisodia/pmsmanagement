import { HttpClient } from '@angular/common/http';
import { Injectable, computed, inject, signal } from '@angular/core';
import { Router } from '@angular/router';
import { Observable, firstValueFrom, shareReplay, tap, finalize } from 'rxjs';
import { SessionUser, SignInResult } from './models';

/** Keeps the access token in memory only. The refresh token is an httpOnly cookie the browser sends to /api/auth. */
@Injectable({ providedIn: 'root' })
export class AuthService {
  private http = inject(HttpClient);
  private router = inject(Router);
  private token = signal<string | null>(null);
  private refreshing: Observable<SignInResult> | null = null;

  readonly user = signal<SessionUser | null>(null);
  readonly signedIn = computed(() => this.user() !== null);

  get accessToken(): string | null { return this.token(); }

  can(...codes: string[]): boolean {
    const p = this.user()?.permissions ?? [];
    return codes.some(c => p.includes(c));
  }

  signIn(userName: string, password: string): Promise<SignInResult> {
    return firstValueFrom(this.http.post<SignInResult>('/api/auth/sign-in', { userName, password }).pipe(tap(r => this.apply(r))));
  }

  /** One refresh at a time; parallel 401s share it. */
  refresh(): Observable<SignInResult> {
    this.refreshing ??= this.http.post<SignInResult>('/api/auth/refresh', null).pipe(
      tap(r => this.apply(r)),
      finalize(() => (this.refreshing = null)),
      shareReplay(1));
    return this.refreshing;
  }

  /** On app start: restore the session from the refresh cookie if there is one. */
  async restore(): Promise<void> {
    try { await firstValueFrom(this.refresh()); } catch { this.clear(); }
  }

  async signOut(): Promise<void> {
    try { await firstValueFrom(this.http.post('/api/auth/sign-out', null)); } catch { /* signed out locally anyway */ }
    this.clear();
    await this.router.navigateByUrl('/sign-in');
  }

  clear(): void { this.token.set(null); this.user.set(null); }

  private apply(r: SignInResult): void { this.token.set(r.accessToken); this.user.set(r.user); }
}
