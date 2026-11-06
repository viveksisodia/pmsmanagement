import { Component, computed, effect, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { Api } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { AreaPipe, DateTimePipe } from '../../core/format';
import { errorText } from '../../core/http';
import { AuditItem, LandParcelListItem, LocationLists, PROJECT_STATUSES, PROJECT_TYPES, UserLookup } from '../../core/models';
import { DateInput } from '../../shared/ui';
import { ProjectContext } from './project-context';

interface LandRow { landParcelId: number; surveyNo: string; parcelAreaSqm: number; availableSqm: number; areaUsedSqm: number; }

@Component({
  selector: 'app-project-form',
  imports: [ReactiveFormsModule, DateInput, AreaPipe, DateTimePipe],
  template: `
    @if (!ctx) { <h1>New project</h1> }
    <form [formGroup]="form" (ngSubmit)="save()" class="card">
      <h2>Project</h2>
      <div class="grid">
        <div class="f"><label for="pj-code">Code <span class="req">*</span></label><input id="pj-code" formControlName="code" /></div>
        <div class="f"><label for="pj-name">Name <span class="req">*</span></label><input id="pj-name" formControlName="name" /></div>
        <div class="f"><label for="pj-dev">Developer entity <span class="req">*</span></label><input id="pj-dev" formControlName="developerEntity" /></div>
        <div class="f"><label for="pj-type">Project type</label><select id="pj-type" formControlName="projectType">@for (t of types; track t) { <option [value]="t">{{ t }}</option> }</select></div>
        <div class="f"><label for="pj-status">Status</label><select id="pj-status" formControlName="status">@for (s of statuses; track s) { <option [value]="s">{{ s }}</option> }</select></div>
        <label class="chk"><input type="checkbox" formControlName="usesPhases" /> Uses phases (each phase can have its own registration, dates and budget)</label>
        <div class="f"><label for="pj-start">Start date</label><app-date-input inputId="pj-start" formControlName="startDate" /></div>
        <div class="f"><label for="pj-target">Target date</label><app-date-input inputId="pj-target" formControlName="targetDate" /></div>
        <div class="f"><label for="pj-country">Country</label>
          <select id="pj-country" (change)="pickCountry(+$any($event.target).value)">
            @for (c of loc()?.countries ?? []; track c.id) { <option [value]="c.id" [selected]="c.id === countryId()">{{ c.name }}</option> }</select></div>
        <div class="f"><label for="pj-state">State / emirate</label><select id="pj-state" formControlName="stateId"><option [ngValue]="null">—</option>
          @for (s of states(); track s.id) { <option [ngValue]="s.id">{{ s.name }}</option> }</select></div>
        <div class="f"><label for="pj-city">City</label><select id="pj-city" formControlName="cityId"><option [ngValue]="null">—</option>
          @for (c of loc()?.cities ?? []; track c.id) { <option [ngValue]="c.id">{{ c.name }}</option> }</select></div>
        <div class="f"><label for="pj-addr">Address</label><input id="pj-addr" formControlName="address" /></div>
        <div class="f"><label for="pj-tz">Time zone</label><input id="pj-tz" formControlName="timeZoneId" /></div>
        <div class="f"><label for="pj-cur">Currency</label><input id="pj-cur" formControlName="currencyCode" maxlength="3" /></div>
        <div class="f"><label for="pj-au">Area unit shown</label><select id="pj-au" formControlName="areaUnit"><option value="sqm">sq m</option><option value="sqft">sq ft</option></select></div>
        <div class="f"><label for="pj-pat">Unit number pattern</label><input id="pj-pat" formControlName="unitNumberPattern" />
          <small class="hint">{{ '{P}' }} phase · {{ '{T}' }} building · {{ '{FF}' }} floor · {{ '{NN}' }} position. Example: {{ example() }}</small></div>
        <div class="f"><label for="pj-skip">Floor numbers to skip</label><input id="pj-skip" formControlName="skipFloorNumbers" placeholder="13" /></div>
      </div>

      <h2>Land</h2>
      <div class="tw"><table><thead><tr><th>Survey no.</th><th class="num">Parcel area</th><th class="num">Still free</th><th class="num">Area used by this project (sq m)</th><th></th></tr></thead>
        <tbody>@for (l of land(); track l.landParcelId) {
          <tr><td class="mono">{{ l.surveyNo }}</td><td class="num">{{ l.parcelAreaSqm | area }}</td><td class="num">{{ l.availableSqm | area }}</td>
            <td class="num"><input type="number" step="0.0001" min="0" [max]="l.availableSqm" [value]="l.areaUsedSqm" (input)="setUsed(l.landParcelId, +$any($event.target).value)" aria-label="Area used" /></td>
            <td><button type="button" class="sm" (click)="unlink(l.landParcelId)">Unlink</button></td></tr>
        } @empty { <tr><td colspan="5" class="hint">No land linked.</td></tr> }</tbody></table></div>
      @if (freeParcels().length) {
        <div class="bar left"><select #pl aria-label="Land parcel to link">@for (p of freeParcels(); track p.id) { <option [value]="p.id">{{ p.surveyNo }} · {{ p.areaSqm - p.areaUsedSqm | area }} free</option> }</select>
          <button type="button" class="sm" (click)="link(+pl.value)">Link parcel</button></div>
      }

      <h2>Team</h2>
      <p class="hint">Members can see this project. People with "See every project" see it anyway.</p>
      <div class="checks">@for (u of people(); track u.id) {
        <label class="chk"><input type="checkbox" [checked]="memberIds().includes(u.id)" (change)="toggleMember(u.id)" /> {{ u.fullName }}</label> }</div>

      @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
      @if (saved()) { <p class="ok" role="status">Project saved.</p> }
      @if (auth.can('projects.edit')) {
        <div class="acts">@if (ctx?.project()) { <button type="button" class="dan" (click)="remove()">Delete project</button> }<span class="grow"></span>
          <button type="submit" class="pri" [disabled]="busy()">Save project</button></div>
      }
    </form>

    @if (ctx?.project() && auth.can('audit.view')) {
      <details class="card" (toggle)="loadAudit()"><summary>Change history</summary>
        <div class="tw"><table><thead><tr><th>When</th><th>Who</th><th>Action</th></tr></thead><tbody>
          @for (a of audit(); track a.id) { <tr><td>{{ a.atUtc | dt }}</td><td>{{ a.userName || '—' }}</td><td>{{ a.action }}</td></tr> }
        </tbody></table></div></details>
    }`
})
export class ProjectForm {
  auth = inject(AuthService);
  ctx = inject(ProjectContext, { optional: true });
  private api = inject(Api); private fb = inject(FormBuilder); private router = inject(Router);
  readonly types = PROJECT_TYPES; readonly statuses = PROJECT_STATUSES;
  loc = signal<LocationLists | null>(null); people = signal<UserLookup[]>([]); parcels = signal<LandParcelListItem[]>([]);
  land = signal<LandRow[]>([]); memberIds = signal<number[]>([]); audit = signal<AuditItem[]>([]);
  countryId = signal(0); busy = signal(false); error = signal(''); saved = signal(false); private rowVersion: string | null = null;

  form = this.fb.group({
    code: ['', Validators.required], name: ['', Validators.required], developerEntity: ['', Validators.required],
    projectType: ['Residential'], status: ['Planning'], usesPhases: [true], startDate: [null as string | null], targetDate: [null as string | null],
    stateId: [null as number | null], cityId: [null as number | null], address: [''], timeZoneId: ['Asia/Kolkata'], currencyCode: ['INR'],
    areaUnit: ['sqm'], unitNumberPattern: ['{T}-{FF}{NN}'], skipFloorNumbers: ['']
  });
  states = computed(() => (this.loc()?.states ?? []).filter(s => s.countryId === this.countryId()));
  freeParcels = computed(() => this.parcels().filter(p => !this.land().some(l => l.landParcelId === p.id) && p.areaSqm - p.areaUsedSqm > 0));
  example = signal('A1-0502');

  constructor() {
    Promise.all([this.api.locations(), this.api.people(), this.auth.can('land.view') ? this.api.landList() : Promise.resolve([])])
      .then(([l, p, lp]) => {
        this.loc.set(l); this.people.set(p); this.parcels.set(lp);
        if (!this.countryId()) this.countryId.set(l.countries.find(c => c.name === 'India')?.id ?? l.countries[0]?.id ?? 0);
      }).catch(e => this.error.set(errorText(e)));
    this.form.controls.unitNumberPattern.valueChanges.subscribe(p => this.example.set((p ?? '').replace('{P}', 'H1').replace('{T}', 'A1').replace('{FF}', '05').replace('{NN}', '02')));
    if (!this.auth.can('projects.edit')) this.form.disable();
    effect(() => {
      const p = this.ctx?.project();
      if (!p) return;
      this.rowVersion = p.rowVersion;
      if (p.countryId) this.countryId.set(p.countryId);
      this.form.patchValue({ ...p, address: p.address ?? '', skipFloorNumbers: p.skipFloorNumbers ?? '' }, { emitEvent: true });
      this.land.set(p.land.map(l => ({ ...l })));
      this.memberIds.set(p.members.map(m => m.id));
    });
  }

  pickCountry(id: number) {
    this.countryId.set(id);
    const name = this.loc()?.countries.find(x => x.id === id)?.name ?? '';
    const uae = /emirates|uae|dubai/i.test(name);
    this.form.patchValue({ stateId: null, ...(/india/i.test(name) ? { currencyCode: 'INR', timeZoneId: 'Asia/Kolkata' } : uae ? { currencyCode: 'AED', timeZoneId: 'Asia/Dubai' } : {}) });
  }
  link(id: number) {
    const p = this.parcels().find(x => x.id === id); if (!p) return;
    const free = p.areaSqm - p.areaUsedSqm;
    this.land.update(l => [...l, { landParcelId: p.id, surveyNo: p.surveyNo, parcelAreaSqm: p.areaSqm, availableSqm: free, areaUsedSqm: free }]);
  }
  unlink(id: number) { this.land.update(l => l.filter(x => x.landParcelId !== id)); }
  setUsed(id: number, v: number) { this.land.update(l => l.map(x => x.landParcelId === id ? { ...x, areaUsedSqm: v } : x)); }
  toggleMember(id: number) { this.memberIds.update(m => m.includes(id) ? m.filter(x => x !== id) : [...m, id]); }

  async save() {
    if (this.form.invalid) { this.form.markAllAsTouched(); this.error.set('Enter the code, name and developer entity.'); return; }
    this.busy.set(true); this.error.set(''); this.saved.set(false);
    try {
      const body = { ...this.form.getRawValue(), land: this.land().map(l => ({ landParcelId: l.landParcelId, areaUsedSqm: l.areaUsedSqm })),
                     memberIds: this.memberIds(), rowVersion: this.rowVersion };
      const id = this.ctx?.project()?.id ?? null;
      const r = await this.api.saveProject(id, body);
      if (!id) { await this.router.navigate(['/projects', r.id, 'structure']); return; }
      await this.ctx!.reload(); this.saved.set(true);
    } catch (e) { this.error.set(errorText(e)); }
    finally { this.busy.set(false); }
  }

  async remove() {
    const reason = window.prompt('Reason for deleting this project'); if (!reason) return;
    try { await this.api.deleteProject(this.ctx!.project()!.id, reason); await this.router.navigateByUrl('/projects'); } catch (e) { this.error.set(errorText(e)); }
  }

  async loadAudit() { const p = this.ctx?.project(); if (p && !this.audit().length) this.audit.set(await this.api.audit('Project', p.id)); }
}
