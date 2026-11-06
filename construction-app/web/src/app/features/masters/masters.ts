import { Component, computed, inject, signal } from '@angular/core';
import { FormBuilder, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { Api } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { AreaPipe } from '../../core/format';
import { errorText } from '../../core/http';
import { LevelTypeItem, LocationLists, UNIT_CATEGORIES, UnitTypeItem } from '../../core/models';
import { Modal } from '../../shared/ui';

@Component({
  selector: 'app-unit-types',
  imports: [ReactiveFormsModule, Modal, AreaPipe],
  template: `
    <div class="bar"><div class="grow"><h1>Unit types</h1>
      <p class="lead">1 BHK, 2 BHK, shops, offices, villas, plots and more. Default areas fill new units; changing them doesn't change existing units.</p></div>
      <label class="chk"><input type="checkbox" [checked]="showInactive()" (change)="showInactive.set($any($event.target).checked); load()" /> Show inactive</label>
      @if (auth.can('masters.edit')) { <button type="button" class="pri" (click)="open(null)">Add unit type</button> }</div>
    @if (error()) { <p class="err">{{ error() }}</p> }
    <div class="tw"><table>
      <thead><tr><th>Code</th><th>Name</th><th>Category</th><th class="num">Bed / bath</th><th class="num">Carpet</th><th class="num">Built-up</th><th class="num">Super</th><th class="num">Plot</th><th>Saleable</th><th class="num">Units</th><th>Active</th><th></th></tr></thead>
      <tbody>@for (t of rows(); track t.id) {
        <tr><td class="mono">{{ t.code }}</td><td>{{ t.name }}</td><td>{{ t.category }}</td><td class="num">{{ t.bedrooms ?? '—' }} / {{ t.bathrooms ?? '—' }}</td>
          <td class="num">{{ t.defaultCarpetSqm | area }}</td><td class="num">{{ t.defaultBuiltUpSqm | area }}</td><td class="num">{{ t.defaultSuperSqm | area }}</td>
          <td class="num">{{ t.defaultPlotSqm | area }}</td><td>{{ t.isSaleable ? 'Yes' : 'No' }}</td><td class="num">{{ t.unitsUsing }}</td><td>{{ t.isActive ? 'Yes' : 'No' }}</td>
          <td>@if (auth.can('masters.edit')) { <button type="button" class="sm" (click)="open(t)">Edit</button> }</td></tr>
      }</tbody></table></div>
    <app-modal [heading]="edit()?.id ? 'Edit unit type' : 'Add unit type'" [open]="dialog()" (closed)="dialog.set(false)">
      <form [formGroup]="form" id="ut-form" (ngSubmit)="save()"><div class="grid">
        <div class="f"><label for="ut-code">Code <span class="req">*</span></label><input id="ut-code" formControlName="code" /></div>
        <div class="f"><label for="ut-name">Name <span class="req">*</span></label><input id="ut-name" formControlName="name" /></div>
        <div class="f"><label for="ut-cat">Category</label><select id="ut-cat" formControlName="category">@for (c of categories; track c) { <option [value]="c">{{ c }}</option> }</select></div>
        <div class="f"><label for="ut-bed">Bedrooms</label><input id="ut-bed" type="number" min="0" max="20" formControlName="bedrooms" /></div>
        <div class="f"><label for="ut-bath">Bathrooms</label><input id="ut-bath" type="number" min="0" max="20" formControlName="bathrooms" /></div>
        <div class="f"><label for="ut-carpet">Default carpet (sq m)</label><input id="ut-carpet" type="number" step="0.0001" min="0" formControlName="defaultCarpetSqm" /></div>
        <div class="f"><label for="ut-built">Default built-up (sq m)</label><input id="ut-built" type="number" step="0.0001" min="0" formControlName="defaultBuiltUpSqm" /></div>
        <div class="f"><label for="ut-super">Default super (sq m)</label><input id="ut-super" type="number" step="0.0001" min="0" formControlName="defaultSuperSqm" /></div>
        <div class="f"><label for="ut-plot">Default plot (sq m)</label><input id="ut-plot" type="number" step="0.0001" min="0" formControlName="defaultPlotSqm" /></div>
        <label class="chk"><input type="checkbox" formControlName="isSaleable" /> Saleable</label>
        <label class="chk"><input type="checkbox" formControlName="isActive" /> Active</label>
      </div>
      <p class="hint">A unit type used by units can be made inactive but not removed.</p>
      @if (formError()) { <p class="err" role="alert">{{ formError() }}</p> }</form>
      <ng-container actions><span class="grow"></span><button type="button" (click)="dialog.set(false)">Cancel</button>
        <button type="submit" form="ut-form" class="pri">Save</button></ng-container>
    </app-modal>`
})
export class UnitTypes {
  auth = inject(AuthService); private api = inject(Api); private fb = inject(FormBuilder);
  readonly categories = UNIT_CATEGORIES;
  rows = signal<UnitTypeItem[]>([]); error = signal(''); showInactive = signal(false);
  dialog = signal(false); edit = signal<UnitTypeItem | null>(null); formError = signal('');
  form = this.fb.group({
    code: ['', Validators.required], name: ['', Validators.required], category: ['Residential'], bedrooms: [null as number | null], bathrooms: [null as number | null],
    defaultCarpetSqm: [null as number | null], defaultBuiltUpSqm: [null as number | null], defaultSuperSqm: [null as number | null], defaultPlotSqm: [null as number | null],
    isSaleable: [true], isActive: [true]
  });
  constructor() { this.load(); }
  async load() { try { this.rows.set(await this.api.unitTypes(this.showInactive())); } catch (e) { this.error.set(errorText(e)); } }
  open(t: UnitTypeItem | null) {
    this.edit.set(t); this.formError.set('');
    this.form.reset(t ?? { category: 'Residential', isSaleable: true, isActive: true });
    this.dialog.set(true);
  }
  async save() {
    if (this.form.invalid) { this.formError.set('Enter the code and name.'); return; }
    try { await this.api.saveUnitType(this.edit()?.id ?? null, { ...this.form.getRawValue(), rowVersion: this.edit()?.rowVersion }); this.dialog.set(false); await this.load(); }
    catch (e) { this.formError.set(errorText(e)); }
  }
}

@Component({
  selector: 'app-level-types',
  imports: [ReactiveFormsModule, Modal],
  template: `
    <div class="bar"><div class="grow"><h1>Floor level types</h1><p class="lead">Used by the floor generator. Only habitable levels can hold units.</p></div>
      @if (auth.can('masters.edit')) { <button type="button" class="pri" (click)="open(null)">Add level type</button> }</div>
    @if (error()) { <p class="err">{{ error() }}</p> }
    <div class="tw"><table><thead><tr><th class="num">Sort</th><th>Code</th><th>Name</th><th>Habitable</th><th class="num">Floors using</th><th>Active</th><th></th></tr></thead>
      <tbody>@for (l of rows(); track l.id) {
        <tr><td class="num">{{ l.sortOrder }}</td><td class="mono">{{ l.code }}</td><td>{{ l.name }}</td><td>{{ l.isHabitable ? 'Yes' : 'No' }}</td>
          <td class="num">{{ l.floorsUsing }}</td><td>{{ l.isActive ? 'Yes' : 'No' }}</td>
          <td>@if (auth.can('masters.edit')) { <button type="button" class="sm" (click)="open(l)">Edit</button> }</td></tr>
      }</tbody></table></div>
    <app-modal [heading]="edit()?.id ? 'Edit level type' : 'Add level type'" [open]="dialog()" (closed)="dialog.set(false)">
      <form [formGroup]="form" id="lv-form" (ngSubmit)="save()"><div class="grid">
        <div class="f"><label for="lv-code">Code <span class="req">*</span></label><input id="lv-code" formControlName="code" /></div>
        <div class="f"><label for="lv-name">Name <span class="req">*</span></label><input id="lv-name" formControlName="name" /></div>
        <div class="f"><label for="lv-sort">Sort order</label><input id="lv-sort" type="number" formControlName="sortOrder" /></div>
        <label class="chk"><input type="checkbox" formControlName="isHabitable" /> Habitable (can hold units)</label>
        <label class="chk"><input type="checkbox" formControlName="isActive" /> Active</label></div>
        @if (formError()) { <p class="err" role="alert">{{ formError() }}</p> }</form>
      <ng-container actions><span class="grow"></span><button type="button" (click)="dialog.set(false)">Cancel</button>
        <button type="submit" form="lv-form" class="pri">Save</button></ng-container>
    </app-modal>`
})
export class LevelTypes {
  auth = inject(AuthService); private api = inject(Api); private fb = inject(FormBuilder);
  rows = signal<LevelTypeItem[]>([]); error = signal(''); dialog = signal(false); edit = signal<LevelTypeItem | null>(null); formError = signal('');
  form = this.fb.nonNullable.group({ code: ['', Validators.required], name: ['', Validators.required], sortOrder: [100], isHabitable: [false], isActive: [true] });
  constructor() { this.load(); }
  async load() { try { this.rows.set(await this.api.levelTypes()); } catch (e) { this.error.set(errorText(e)); } }
  open(l: LevelTypeItem | null) { this.edit.set(l); this.formError.set(''); this.form.reset(l ?? { code: '', name: '', sortOrder: 100, isHabitable: false, isActive: true }); this.dialog.set(true); }
  async save() {
    if (this.form.invalid) { this.formError.set('Enter the code and name.'); return; }
    try { await this.api.saveLevelType(this.edit()?.id ?? null, { ...this.form.getRawValue(), rowVersion: this.edit()?.rowVersion }); this.dialog.set(false); await this.load(); }
    catch (e) { this.formError.set(errorText(e)); }
  }
}

/** Countries, states and cities from the ERP masters (tbl_countrymaster, tbl_statemaster, tbl_City_master). Read-only here. */
@Component({
  selector: 'app-locations',
  imports: [FormsModule],
  template: `
    <h1>Locations</h1>
    <p class="lead">These lists come from the ERP masters and are maintained in the ERP. The city list isn't linked to states there, so it is shown on its own.</p>
    <div class="bar left">
      <label for="lc-country">Country</label>
      <select id="lc-country" [ngModel]="countryId()" (ngModelChange)="countryId.set(+$event)">
        @for (c of data()?.countries ?? []; track c.id) { <option [ngValue]="c.id">{{ c.name }}</option> }</select>
    </div>
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="split">
      <div class="tw"><table><thead><tr><th>State / emirate ({{ states().length }})</th></tr></thead><tbody>
        @for (s of states(); track s.id) { <tr><td>{{ s.name }}</td></tr> } @empty { <tr><td class="hint">No states for this country.</td></tr> }
      </tbody></table></div>
      <div class="tw"><table><thead><tr><th>Cities ({{ data()?.cities?.length ?? 0 }})</th></tr></thead><tbody>
        @for (c of data()?.cities ?? []; track c.id) { <tr><td>{{ c.name }}</td></tr> } @empty { <tr><td class="hint">No cities.</td></tr> }
      </tbody></table></div>
    </div>`
})
export class Locations {
  private api = inject(Api);
  data = signal<LocationLists | null>(null); countryId = signal(0); error = signal('');
  states = computed(() => (this.data()?.states ?? []).filter(s => s.countryId === this.countryId()));
  constructor() {
    this.api.locations().then(d => { this.data.set(d); this.countryId.set(d.countries.find(c => c.name === 'India')?.id ?? d.countries[0]?.id ?? 0); })
      .catch(e => this.error.set(errorText(e)));
  }
}
