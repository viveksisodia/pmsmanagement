import { Component, computed, inject, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Api } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { AreaPipe, DatePipe } from '../../core/format';
import { errorText } from '../../core/http';
import { ENCUMBRANCES, LAND_UNITS, LandParcelListItem, LocationLists, TENURES, UserLookup } from '../../core/models';
import { DateInput, Modal } from '../../shared/ui';

@Component({
  selector: 'app-land-list',
  imports: [ReactiveFormsModule, Modal, DateInput, AreaPipe, DatePipe],
  template: `
    <div class="bar">
      <div class="grow"><h1>Land parcels</h1><p class="lead">Each parcel is recorded once and can be shared by several projects. Areas are stored in square metres.</p></div>
      <input type="search" placeholder="Search survey no., locality, city" aria-label="Search land parcels" [value]="search()" (input)="search.set($any($event.target).value); load()" />
      @if (auth.can('land.edit')) { <button type="button" class="pri" (click)="open(null)">Add parcel</button> }
    </div>
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="tw"><table>
      <thead><tr><th>Survey no.</th><th>Location</th><th class="num">Area</th><th class="num">Used by projects</th><th>Tenure</th><th>Registration</th><th>Encumbrance</th><th>Projects</th><th></th></tr></thead>
      <tbody>
        @for (p of rows(); track p.id) {
          <tr>
            <td class="mono">{{ p.surveyNo }}</td><td>{{ p.locality }}{{ p.city ? ', ' + p.city : '' }}<br /><small class="hint">{{ p.state }}</small></td>
            <td class="num">{{ p.areaSqm | area }}<br /><small class="hint">entered {{ p.areaEntered }} {{ p.areaUnitEntered }}</small></td>
            <td class="num">{{ p.areaUsedSqm | area }}</td><td>{{ p.tenure }}</td>
            <td>{{ p.registrationNo || '—' }}<br /><small class="hint">{{ p.registrationDate | d }}</small></td>
            <td>{{ p.encumbrance }}</td><td>{{ p.projects || '—' }}</td>
            <td class="acts-cell">@if (auth.can('land.edit')) { <button type="button" class="sm" (click)="open(p.id)">Edit</button> }</td>
          </tr>
        } @empty { <tr><td colspan="9" class="hint">{{ loading() ? 'Loading…' : 'No land parcels yet.' }}</td></tr> }
      </tbody>
    </table></div>

    <app-modal [heading]="editId() ? 'Edit land parcel' : 'Add land parcel'" [open]="dialog()" [wide]="true" (closed)="dialog.set(false)">
      <form [formGroup]="form" id="land-form" (ngSubmit)="save()">
        <div class="grid">
          <div class="f"><label for="lp-survey">Survey / plot no. <span class="req">*</span></label><input id="lp-survey" formControlName="surveyNo" /></div>
          <div class="f"><label for="lp-loc">Locality / sector</label><input id="lp-loc" formControlName="locality" /></div>
          <div class="f"><label for="lp-country">Country</label>
            <select id="lp-country" (change)="countryId.set(+$any($event.target).value); form.controls.stateId.setValue(null)">
              @for (c of loc()?.countries ?? []; track c.id) { <option [value]="c.id" [selected]="c.id === countryId()">{{ c.name }}</option> }
            </select></div>
          <div class="f"><label for="lp-state">State / emirate <span class="req">*</span></label>
            <select id="lp-state" formControlName="stateId">
              <option [ngValue]="null">Choose…</option>
              @for (s of states(); track s.id) { <option [ngValue]="s.id">{{ s.name }}</option> }
            </select></div>
          <div class="f"><label for="lp-city">City</label>
            <select id="lp-city" formControlName="cityId"><option [ngValue]="null">—</option>
              @for (c of loc()?.cities ?? []; track c.id) { <option [ngValue]="c.id">{{ c.name }}</option> }
            </select></div>
          <div class="f"><label for="lp-area">Area <span class="req">*</span></label><input id="lp-area" type="number" step="0.0001" min="0" formControlName="areaEntered" /></div>
          <div class="f"><label for="lp-unit">Area unit</label><select id="lp-unit" formControlName="areaUnitEntered">
            @for (u of units; track u.v) { <option [value]="u.v">{{ u.t }}</option> }</select></div>
          <div class="f"><label for="lp-tenure">Ownership / tenure</label><select id="lp-tenure" formControlName="tenure">
            @for (t of tenures; track t) { <option [value]="t">{{ t }}</option> }</select></div>
          <div class="f"><label for="lp-reg">Registration no.</label><input id="lp-reg" formControlName="registrationNo" /></div>
          <div class="f"><label for="lp-regon">Registration date</label><app-date-input inputId="lp-regon" formControlName="registrationDate" /></div>
          <div class="f"><label for="lp-office">Registry office</label><input id="lp-office" formControlName="registryOffice" /></div>
          <div class="f"><label for="lp-use">Land use / zoning</label><input id="lp-use" formControlName="landUse" /></div>
          <div class="f"><label for="lp-enc">Encumbrance</label><select id="lp-enc" formControlName="encumbrance">
            @for (e of encumbrances; track e) { <option [value]="e">{{ e }}</option> }</select></div>
          <div class="f"><label for="lp-lat">Latitude</label><input id="lp-lat" type="number" step="0.000001" formControlName="latitude" /></div>
          <div class="f"><label for="lp-lng">Longitude</label><input id="lp-lng" type="number" step="0.000001" formControlName="longitude" /></div>
          <div class="f"><label for="lp-resp">Responsible person</label><select id="lp-resp" formControlName="responsibleUserId">
            <option [ngValue]="null">—</option>@for (u of people(); track u.id) { <option [ngValue]="u.id">{{ u.fullName }}</option> }</select></div>
        </div>
        <h3>Owners</h3>
        <div class="tw" formArrayName="owners"><table>
          <thead><tr><th>Owner name</th><th class="num">Share %</th><th></th></tr></thead>
          <tbody>
            @for (o of owners.controls; track $index; let i = $index) {
              <tr [formGroupName]="i"><td><input formControlName="ownerName" aria-label="Owner name" /></td>
                <td class="num"><input type="number" step="0.01" min="0" max="100" formControlName="sharePct" aria-label="Share percent" /></td>
                <td><button type="button" class="sm" (click)="owners.removeAt(i)">Remove</button></td></tr>
            }
          </tbody>
        </table></div>
        <div class="bar left"><button type="button" class="sm" (click)="addOwner('', 0)">Add owner</button>
          <small class="hint">Shares must total 100 percent (now {{ shareTotal() }} %).</small></div>
        <div class="f"><label for="lp-rem">Remarks</label><textarea id="lp-rem" formControlName="remarks"></textarea></div>
        @if (formError()) { <p class="err" role="alert">{{ formError() }}</p> }
      </form>
      <ng-container actions>
        @if (editId()) { <button type="button" class="dan" (click)="remove()">Delete</button> }
        <span class="grow"></span>
        <button type="button" (click)="dialog.set(false)">Cancel</button>
        <button type="submit" form="land-form" class="pri" [disabled]="busy()">Save parcel</button>
      </ng-container>
    </app-modal>`
})
export class LandList {
  auth = inject(AuthService);
  private api = inject(Api);
  private fb = inject(FormBuilder);
  readonly units = LAND_UNITS; readonly tenures = TENURES; readonly encumbrances = ENCUMBRANCES;
  rows = signal<LandParcelListItem[]>([]);
  loading = signal(true); error = signal(''); search = signal('');
  loc = signal<LocationLists | null>(null); people = signal<UserLookup[]>([]);
  dialog = signal(false); editId = signal<number | null>(null); busy = signal(false); formError = signal('');
  countryId = signal(0); private rowVersion: string | null = null;

  form = this.fb.group({
    surveyNo: ['', Validators.required], locality: [''], stateId: [null as number | null, Validators.required], cityId: [null as number | null],
    areaEntered: [null as number | null, [Validators.required, Validators.min(0.0001)]], areaUnitEntered: ['sqm'], tenure: ['Owned'],
    registrationNo: [''], registrationDate: [null as string | null], registryOffice: [''], landUse: [''], encumbrance: ['Under check'],
    latitude: [null as number | null], longitude: [null as number | null], responsibleUserId: [null as number | null], remarks: [''],
    owners: this.fb.array<ReturnType<LandList['ownerGroup']>>([])
  });
  get owners() { return this.form.controls.owners as FormArray; }
  states = computed(() => (this.loc()?.states ?? []).filter(s => s.countryId === this.countryId()));
  shareTotal = signal(0);

  constructor() {
    this.load();
    this.api.locations().then(l => { this.loc.set(l); this.countryId.set(l.countries.find(c => c.name === 'India')?.id ?? l.countries[0]?.id ?? 0); });
    this.api.people().then(p => this.people.set(p));
    this.form.controls.owners.valueChanges.subscribe(v => this.shareTotal.set(Math.round((v as { sharePct: number }[]).reduce((a, o) => a + (+o.sharePct || 0), 0) * 100) / 100));
  }

  async load() {
    this.loading.set(true);
    try { this.rows.set(await this.api.landList(this.search())); this.error.set(''); } catch (e) { this.error.set(errorText(e)); }
    finally { this.loading.set(false); }
  }

  private ownerGroup(name: string, share: number) { return this.fb.group({ ownerName: [name], sharePct: [share] }); }
  addOwner(name: string, share: number) { this.owners.push(this.ownerGroup(name, share)); }

  async open(id: number | null) {
    this.formError.set(''); this.editId.set(id); this.owners.clear();
    if (id) {
      const p = await this.api.land(id);
      this.countryId.set(p.countryId || this.countryId()); this.rowVersion = p.rowVersion;
      this.form.patchValue({ ...p, locality: p.locality ?? '', registrationNo: p.registrationNo ?? '', registryOffice: p.registryOffice ?? '', landUse: p.landUse ?? '', remarks: p.remarks ?? '' });
      p.owners.forEach(o => this.addOwner(o.ownerName, o.sharePct));
    } else {
      this.rowVersion = null;
      this.form.reset({ areaUnitEntered: 'sqm', tenure: 'Owned', encumbrance: 'Under check' });
      this.addOwner('', 100);
    }
    this.dialog.set(true);
  }

  async save() {
    if (this.form.invalid) { this.form.markAllAsTouched(); this.formError.set('Fill in the survey number, state and area.'); return; }
    this.busy.set(true); this.formError.set('');
    try {
      const v = this.form.getRawValue();
      await this.api.saveLand(this.editId(), { ...v, rowVersion: this.rowVersion, owners: (v.owners as { ownerName: string; sharePct: number }[]).filter(o => o.ownerName?.trim()) });
      this.dialog.set(false); await this.load();
    } catch (e) { this.formError.set(errorText(e)); }
    finally { this.busy.set(false); }
  }

  async remove() {
    const reason = window.prompt('Reason for deleting this parcel');
    if (!reason) return;
    try { await this.api.deleteLand(this.editId()!, reason); this.dialog.set(false); await this.load(); } catch (e) { this.formError.set(errorText(e)); }
  }
}
