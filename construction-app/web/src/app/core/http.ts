import { HttpErrorResponse, HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { catchError, switchMap, throwError } from 'rxjs';
import { AuthService } from './auth.service';

/** Adds the bearer token and the X-Requested-With header; on 401 tries one refresh, then sends the user to sign in. */
export const authInterceptor: HttpInterceptorFn = (req, next) => {
  const auth = inject(AuthService);
  const router = inject(Router);
  const isAuthCall = req.url.startsWith('/api/auth/sign-in') || req.url.startsWith('/api/auth/refresh');
  const withToken = (token: string | null) => req.clone({
    setHeaders: { 'X-Requested-With': 'XMLHttpRequest', ...(token && !isAuthCall ? { Authorization: `Bearer ${token}` } : {}) }
  });
  return next(withToken(auth.accessToken)).pipe(
    catchError((e: unknown) => {
      if (!(e instanceof HttpErrorResponse) || e.status !== 401 || isAuthCall) return throwError(() => e);
      return auth.refresh().pipe(
        switchMap(r => next(withToken(r.accessToken))),
        catchError(err => { auth.clear(); router.navigateByUrl('/sign-in'); return throwError(() => err); }));
    }));
};

export const signedInGuard: CanActivateFn = () => {
  const auth = inject(AuthService);
  return auth.signedIn() ? true : inject(Router).parseUrl('/sign-in');
};

export const permissionGuard = (...codes: string[]): CanActivateFn => () => {
  const auth = inject(AuthService);
  return auth.can(...codes) ? true : inject(Router).parseUrl('/projects');
};

/** The message to show for a failed call. The API sends { status, message } for business-rule errors. */
export function errorText(e: unknown): string {
  if (e instanceof HttpErrorResponse) {
    if (e.status === 0) return 'The server can’t be reached. Check your connection and try again.';
    if (e.error && typeof e.error.message === 'string') return e.error.message;
    if (e.status === 403) return 'You don’t have permission to do this.';
    if (e.status === 429) return 'Too many attempts. Wait a minute and try again.';
    return `The request failed (${e.status}).`;
  }
  return e instanceof Error ? e.message : 'Something went wrong.';
}
