import { Pipe, PipeTransform } from '@angular/core';

/** All dates show as dd/mm/yyyy and date-times as dd/mm/yyyy HH:mm (local time of the browser). */
export function formatDate(iso: string | null | undefined): string {
  if (!iso) return '—';
  const m = /^(\d{4})-(\d{2})-(\d{2})/.exec(iso);
  return m ? `${m[3]}/${m[2]}/${m[1]}` : iso;
}

export function formatDateTime(isoUtc: string | null | undefined): string {
  if (!isoUtc) return '—';
  const d = new Date(/Z|[+-]\d\d:\d\d$/.test(isoUtc) ? isoUtc : isoUtc + 'Z');
  if (isNaN(d.getTime())) return isoUtc;
  const p = (n: number) => String(n).padStart(2, '0');
  return `${p(d.getDate())}/${p(d.getMonth() + 1)}/${d.getFullYear()} ${p(d.getHours())}:${p(d.getMinutes())}`;
}

/** Parses dd/mm/yyyy into yyyy-mm-dd. Returns null for empty, undefined for invalid. */
export function parseDisplayDate(text: string): string | null | undefined {
  const t = text.trim();
  if (!t) return null;
  const m = /^(\d{1,2})[/.-](\d{1,2})[/.-](\d{4})$/.exec(t);
  if (!m) return undefined;
  const [d, mo, y] = [+m[1], +m[2], +m[3]];
  const dt = new Date(Date.UTC(y, mo - 1, d));
  if (dt.getUTCFullYear() !== y || dt.getUTCMonth() !== mo - 1 || dt.getUTCDate() !== d) return undefined;
  return `${y}-${String(mo).padStart(2, '0')}-${String(d).padStart(2, '0')}`;
}

export const SQM_PER_SQFT = 0.09290304;
export const toSqm = (v: number | null | undefined, unit: string) => v == null || isNaN(+v) ? null : Math.round((unit === 'sqft' ? +v * SQM_PER_SQFT : +v) * 10000) / 10000;
export const fromSqm = (v: number | null | undefined, unit: string) => v == null ? null : Math.round((unit === 'sqft' ? v / SQM_PER_SQFT : v) * 100) / 100;
export const unitLabel = (unit: string) => unit === 'sqft' ? 'sq ft' : unit === 'sqm' ? 'sq m' : unit;

export function formatArea(sqm: number | null | undefined, unit = 'sqm'): string {
  const v = fromSqm(sqm, unit);
  return v == null ? '—' : `${v.toLocaleString('en-IN', { maximumFractionDigits: 2 })} ${unitLabel(unit)}`;
}

@Pipe({ name: 'd' })
export class DatePipe implements PipeTransform { transform(v: string | null | undefined) { return formatDate(v); } }

@Pipe({ name: 'dt' })
export class DateTimePipe implements PipeTransform { transform(v: string | null | undefined) { return formatDateTime(v); } }

@Pipe({ name: 'area' })
export class AreaPipe implements PipeTransform { transform(v: number | null | undefined, unit = 'sqm') { return formatArea(v, unit); } }
