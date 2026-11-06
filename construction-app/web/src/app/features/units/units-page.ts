import { Component, computed, effect, inject, input, signal } from '@angular/core';
import { FormArray, FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Api } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { AreaPipe, DateTimePipe, fromSqm, toSqm, unitLabel } from '../../core/format';
import { errorText } from '../../core/http';
import { AVAILABILITY, BuildingItem, FloorItem, StackingPlan, StructureTree, UnitDetail, UnitListItem, UnitPreview, UnitTypeItem } from '../../core/models';
import { Modal, Pill } from '../../shared/ui';
import { ProjectContext } from '../projects/project-context';

@Component({
  selector: 'app-units-page',
  imports: [ReactiveFormsModule, Modal, Pill, AreaPipe, DateTimePipe],
  template: `
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="bar">
      <label for="un-b">Building</label>
      <select id="un-b" (change)="pickBuilding(+$any($event.target).value)">
        @for (b of tree()?.buildings ?? []; track b.id) { <option [value]="b.id" [selected]="b.id === buildingId()">{{ b.name }} ({{ b.units }})</option> }</select>
      <div class="seg" role="group" aria-label="View">
        <button type="button" [class.on]="view() === 'stack'" (click)="view.set('stack')">Stacking plan</button>
        <button type="button" [class.on]="view() === 'list'" (click)="view.set('list'); loadList()">List</button>
      </div>
      <span class="grow"></span>
      @if (auth.can('units.generate') && building()) { <button type="button" class="pri" (click)="openGen()">Generate units</button> }
    </div>
    <div class="legend">@for (a of availability; track a) { <app-pill [value]="a" /> <span class="hint">{{ counts()[a] ?? 0 }}</span> }</div>

    @if (view() === 'stack') {
      @if (stack(); as s) {
        @if (hasFloors()) {
          <div class="tw stack"><table>
            <thead><tr><th>Floor</th>@for (p of positions(); track p) { <th class="num">{{ p }}</th> }</tr></thead>
            <tbody>@for (f of s.floors; track f.id) {
              <tr [class.dim]="!f.isHabitable"><th scope="row">{{ f.name }}</th>
                @for (p of positions(); track p) {
                  @if (cell(f.id, p); as u) {
                    <td><button type="button" class="unit" [attr.data-k]="u.availability" (click)="openUnit(u.id)"
                         [title]="u.unitType + ' · ' + u.availability"><b>{{ u.unitNo }}</b><small>{{ u.unitType }}</small></button></td>
                  } @else { <td></td> }
                }</tr>
            } @empty { <tr><td class="hint">No floors in this building yet.</td></tr> }</tbody>
          </table></div>
        } @else {
          <div class="grid-units">@for (u of s.units; track u.id) {
            <button type="button" class="unit" [attr.data-k]="u.availability" (click)="openUnit(u.id)"><b>{{ u.unitNo }}</b><small>{{ u.unitType }}</small></button>
          } @empty { <p class="hint">No units yet. Use Generate units.</p> }</div>
        }
      }
    } @else {
      <div class="bar left">
        <input type="search" placeholder="Unit number" aria-label="Search unit number" (input)="q.set($any($event.target).value); loadList()" />
        <select aria-label="Availability" (change)="qAvail.set($any($event.target).value); loadList()"><option value="">Any availability</option>
          @for (a of availability; track a) { <option [value]="a">{{ a }}</option> }</select>
        <small class="hint">{{ list().total }} unit(s)</small>
      </div>
      <div class="tw"><table>
        <thead><tr><th>Unit</th><th>Floor</th><th class="num">Pos</th><th>Type</th><th>Facing</th><th class="num">Carpet</th><th class="num">Built-up</th><th class="num">Super</th><th class="num">Plot</th><th>Availability</th></tr></thead>
        <tbody>@for (u of list().items; track u.id) {
          <tr><td><button type="button" class="link mono" (click)="openUnit(u.id)">{{ u.unitNo }}</button></td><td>{{ u.floor || '—' }}</td><td class="num">{{ u.position }}</td>
            <td>{{ u.unitType }}</td><td>{{ u.facing || '—' }}</td><td class="num">{{ u.carpetSqm | area: au() }}</td><td class="num">{{ u.builtUpSqm | area: au() }}</td>
            <td class="num">{{ u.superSqm | area: au() }}</td><td class="num">{{ u.plotSqm | area: au() }}</td><td><app-pill [value]="u.availability" /></td></tr>
        } @empty { <tr><td colspan="10" class="hint">No units match.</td></tr> }</tbody>
      </table></div>
      @if (list().total > list().items.length) { <p class="hint">Showing the first {{ list().items.length }}. Narrow the search to see others.</p> }
    }

    <!-- generator -->
    <app-modal heading="Generate units" [open]="genDlg()" [wide]="true" (closed)="genDlg.set(false)">
      <form [formGroup]="gen" id="ug-form" (ngSubmit)="generate()">
        @if (hasFloors()) {
          <div class="grid">
            <div class="f"><label for="ug-from">First floor</label><select id="ug-from" formControlName="fromFloorId">
              @for (f of habitable(); track f.id) { <option [ngValue]="f.id">{{ f.name }}</option> }</select></div>
            <div class="f"><label for="ug-to">Last floor</label><select id="ug-to" formControlName="toFloorId">
              @for (f of habitable(); track f.id) { <option [ngValue]="f.id">{{ f.name }}</option> }</select></div>
          </div>
          <p class="hint">The same positions are created on every habitable floor in the range. Numbers use {{ pattern() }}.</p>
        } @else { <p class="hint">One villa or plot per row. Numbers use {{ pattern() }}.</p> }
        <div class="tw" formArrayName="positions"><table>
          <thead><tr><th class="num">Position</th><th>Unit type</th><th>Facing</th><th class="num">Carpet ({{ unitText() }})</th><th class="num">Built-up</th><th class="num">Super</th>
            <th class="num">Balcony</th><th class="num">Plot</th><th></th></tr></thead>
          <tbody>@for (row of positionsArray.controls; track $index; let i = $index) {
            <tr [formGroupName]="i">
              <td class="num"><input type="number" min="1" formControlName="position" aria-label="Position" class="w5" /></td>
              <td><select formControlName="unitTypeId" aria-label="Unit type" (change)="applyDefaults(i)">@for (t of types(); track t.id) { <option [ngValue]="t.id">{{ t.name }}</option> }</select></td>
              <td><input formControlName="facing" aria-label="Facing" class="w8" /></td>
              <td class="num"><input type="number" step="0.01" min="0" formControlName="carpet" aria-label="Carpet area" class="w8" /></td>
              <td class="num"><input type="number" step="0.01" min="0" formControlName="builtUp" aria-label="Built-up area" class="w8" /></td>
              <td class="num"><input type="number" step="0.01" min="0" formControlName="super" aria-label="Super area" class="w8" /></td>
              <td class="num"><input type="number" step="0.01" min="0" formControlName="balcony" aria-label="Balcony area" class="w8" /></td>
              <td class="num"><input type="number" step="0.01" min="0" formControlName="plot" aria-label="Plot area" class="w8" /></td>
              <td><button type="button" class="sm" (click)="positionsArray.removeAt(i)">Remove</button></td></tr>
          }</tbody></table></div>
        <div class="bar left"><button type="button" class="sm" (click)="addPosition()">Add position</button>
          <button type="button" class="sm" (click)="previewGen()">Preview</button>
          @if (genPreview(); as p) { <small class="hint">{{ p.newCount }} new, {{ p.existingCount }} already exist and will be skipped.
            First {{ p.units[0]?.unitNo }}, last {{ p.units.at(-1)?.unitNo }}.</small> }</div>
        @if (dlgError()) { <p class="err" role="alert">{{ dlgError() }}</p> }
      </form>
      <ng-container actions><span class="grow"></span><button type="button" (click)="genDlg.set(false)">Cancel</button>
        <button type="submit" form="ug-form" class="pri" [disabled]="busy()">Create units</button></ng-container>
    </app-modal>

    <!-- unit -->
    <app-modal [heading]="'Unit ' + (unit()?.unitNo ?? '')" [open]="unitDlg()" [wide]="true" (closed)="unitDlg.set(false)">
      @if (unit(); as u) {
        <p class="hint">{{ u.building }}{{ u.floor ? ' · ' + u.floor : '' }}{{ u.position ? ' · position ' + u.position : '' }} · <app-pill [value]="u.availability" /></p>
        <form [formGroup]="unitForm" id="uf-form" (ngSubmit)="saveUnit()"><div class="grid">
          <div class="f"><label for="uf-no">Unit number</label><input id="uf-no" formControlName="unitNo" /></div>
          <div class="f"><label for="uf-type">Unit type</label><select id="uf-type" formControlName="unitTypeId">@for (t of allTypes(); track t.id) { <option [ngValue]="t.id">{{ t.name }}</option> }</select></div>
          <div class="f"><label for="uf-face">Facing</label><input id="uf-face" formControlName="facing" /></div>
          <div class="f"><label for="uf-c">Carpet ({{ unitText() }})</label><input id="uf-c" type="number" step="0.01" formControlName="carpet" /></div>
          <div class="f"><label for="uf-b">Built-up ({{ unitText() }})</label><input id="uf-b" type="number" step="0.01" formControlName="builtUp" /></div>
          <div class="f"><label for="uf-s">Super ({{ unitText() }})</label><input id="uf-s" type="number" step="0.01" formControlName="super" /></div>
          <div class="f"><label for="uf-bal">Balcony ({{ unitText() }})</label><input id="uf-bal" type="number" step="0.01" formControlName="balcony" /></div>
          <div class="f"><label for="uf-p">Plot ({{ unitText() }})</label><input id="uf-p" type="number" step="0.01" formControlName="plot" /></div>
          <div class="f wide"><label for="uf-r">Reason (needed when the number changes)</label><input id="uf-r" formControlName="reason" /></div>
        </div></form>
        @if (auth.can('availability.change') && u.allowedNext.length) {
          <h3>Change availability</h3>
          <div class="bar left"><select #to aria-label="New availability">@for (n of u.allowedNext; track n) { <option [value]="n">{{ n }}</option> }</select>
            <input #why placeholder="Reason" aria-label="Reason for availability change" class="grow" />
            <button type="button" class="sm pri" (click)="changeAvailability(to.value, why)">Change</button></div>
        }
        <h3>Availability history</h3>
        <div class="tw"><table><thead><tr><th>When</th><th>From</th><th>To</th><th>Reason</th><th>By</th></tr></thead><tbody>
          @for (h of u.history; track h.id) { <tr><td>{{ h.changedAtUtc | dt }}</td><td>{{ h.fromStatus || '—' }}</td><td>{{ h.toStatus }}</td><td>{{ h.reason }}</td><td>{{ h.changedBy || '—' }}</td></tr> }
        </tbody></table></div>
        @if (dlgError()) { <p class="err" role="alert">{{ dlgError() }}</p> }
      }
      <ng-container actions>
        @if (auth.can('units.edit') && unit()?.availability === 'Available') { <button type="button" class="dan" (click)="deleteUnit()">Delete unit</button> }
        <span class="grow"></span><button type="button" (click)="unitDlg.set(false)">Close</button>
        @if (auth.can('units.edit')) { <button type="submit" form="uf-form" class="pri">Save unit</button> }
      </ng-container>
    </app-modal>`
})
export class UnitsPage {
  building_ = input<string | undefined>(undefined, { alias: 'building' });
  auth = inject(AuthService); private ctx = inject(ProjectContext); private api = inject(Api); private fb = inject(FormBuilder);
  readonly availability = AVAILABILITY;
  project = this.ctx.project;
  au = computed(() => this.project()?.areaUnit ?? 'sqm'); unitText = computed(() => unitLabel(this.au()));
  tree = signal<StructureTree | null>(null); buildingId = signal<number | null>(null);
  building = computed<BuildingItem | null>(() => this.tree()?.buildings.find(b => b.id === this.buildingId()) ?? null);
  hasFloors = computed(() => !['Villa cluster', 'Plot sector'].includes(this.building()?.buildingType ?? ''));
  pattern = computed(() => this.building()?.unitNumberPattern || this.project()?.unitNumberPattern || '');
  view = signal<'stack' | 'list'>('stack');
  stack = signal<StackingPlan | null>(null); floors = signal<FloorItem[]>([]); habitable = computed(() => this.floors().filter(f => f.isHabitable));
  allTypes = signal<UnitTypeItem[]>([]); types = computed(() => this.allTypes().filter(t => t.isActive));
  list = signal<{ total: number; items: UnitListItem[] }>({ total: 0, items: [] }); q = signal(''); qAvail = signal('');
  error = signal(''); dlgError = signal(''); busy = signal(false);
  positions = computed(() => [...new Set((this.stack()?.units ?? []).map(u => u.position ?? 0))].sort((a, b) => a - b));
  counts = computed(() => (this.stack()?.units ?? []).reduce((m, u) => ({ ...m, [u.availability]: (m[u.availability] ?? 0) + 1 }), {} as Record<string, number>));
  private cells = computed(() => new Map((this.stack()?.units ?? []).map(u => [`${u.floorId}:${u.position}`, u])));
  cell(floorId: number, pos: number) { return this.cells().get(`${floorId}:${pos}`); }

  genDlg = signal(false); genPreview = signal<UnitPreview | null>(null);
  gen = this.fb.group({ fromFloorId: [null as number | null], toFloorId: [null as number | null], positions: this.fb.array<ReturnType<UnitsPage['positionGroup']>>([]) });
  get positionsArray() { return this.gen.controls.positions as FormArray; }

  unitDlg = signal(false); unit = signal<UnitDetail | null>(null);
  unitForm = this.fb.group({ unitNo: ['', Validators.required], unitTypeId: [0], facing: [''], carpet: [null as number | null], builtUp: [null as number | null],
    super: [null as number | null], balcony: [null as number | null], plot: [null as number | null], reason: [''] });

  constructor() {
    this.api.unitTypes(true).then(t => this.allTypes.set(t));
    effect(() => { if (this.project()) this.loadTree(); });
  }
  private pid() { return this.project()!.id; }

  async loadTree() {
    try {
      const t = await this.api.structure(this.pid()); this.tree.set(t);
      const wanted = this.building_() ? +this.building_()! : this.buildingId();
      const id = t.buildings.find(b => b.id === wanted)?.id ?? t.buildings[0]?.id ?? null;
      if (id) await this.pickBuilding(id);
    } catch (e) { this.error.set(errorText(e)); }
  }
  async pickBuilding(id: number) { this.buildingId.set(id); await this.refresh(); }
  async refresh() {
    const b = this.buildingId(); if (!b) return;
    try {
      const [s, f] = await Promise.all([this.api.stack(this.pid(), b), this.hasFloors() ? this.api.floors(this.pid(), b) : Promise.resolve([])]);
      this.stack.set(s); this.floors.set(f);
      if (this.view() === 'list') await this.loadList();
    } catch (e) { this.error.set(errorText(e)); }
  }
  async loadList() {
    try { this.list.set(await this.api.units(this.pid(), { buildingId: this.buildingId(), search: this.q(), availability: this.qAvail(), pageSize: 500 })); }
    catch (e) { this.error.set(errorText(e)); }
  }

  /* generator */
  private positionGroup(p: number) {
    return this.fb.group({ position: [p], unitTypeId: [this.types()[0]?.id ?? 0], facing: [''], carpet: [null as number | null], builtUp: [null as number | null],
      super: [null as number | null], balcony: [null as number | null], plot: [null as number | null] });
  }
  addPosition() { this.positionsArray.push(this.positionGroup(this.positionsArray.length + 1)); this.applyDefaults(this.positionsArray.length - 1); }
  applyDefaults(i: number) {
    const g = this.positionsArray.at(i); const t = this.allTypes().find(x => x.id === g.value.unitTypeId); if (!t) return;
    g.patchValue({ carpet: fromSqm(t.defaultCarpetSqm, this.au()), builtUp: fromSqm(t.defaultBuiltUpSqm, this.au()), super: fromSqm(t.defaultSuperSqm, this.au()), plot: fromSqm(t.defaultPlotSqm, this.au()) });
  }
  openGen() {
    this.dlgError.set(''); this.genPreview.set(null); this.positionsArray.clear();
    const h = this.habitable();
    this.gen.patchValue({ fromFloorId: h[0]?.id ?? null, toFloorId: h.at(-1)?.id ?? null });
    for (let i = 0; i < (this.hasFloors() ? 4 : 1); i++) this.addPosition();
    this.genDlg.set(true);
  }
  private genBody() {
    const v = this.gen.getRawValue(); const u = this.au();
    return { fromFloorId: this.hasFloors() ? v.fromFloorId : null, toFloorId: this.hasFloors() ? v.toFloorId : null,
      positions: (v.positions as { position: number; unitTypeId: number; facing: string; carpet: number | null; builtUp: number | null; super: number | null; balcony: number | null; plot: number | null }[])
        .map(p => ({ position: p.position, unitTypeId: p.unitTypeId, facing: p.facing || null, carpetSqm: toSqm(p.carpet, u), builtUpSqm: toSqm(p.builtUp, u),
                     superSqm: toSqm(p.super, u), balconySqm: toSqm(p.balcony, u), plotSqm: toSqm(p.plot, u) })) };
  }
  async previewGen() {
    this.dlgError.set('');
    try { this.genPreview.set(await this.api.previewUnits(this.pid(), this.buildingId()!, this.genBody())); } catch (e) { this.dlgError.set(errorText(e)); }
  }
  async generate() {
    this.busy.set(true); this.dlgError.set('');
    try { await this.api.generateUnits(this.pid(), this.buildingId()!, this.genBody()); this.genDlg.set(false); await this.loadTree(); }
    catch (e) { this.dlgError.set(errorText(e)); }
    finally { this.busy.set(false); }
  }

  /* unit */
  async openUnit(id: number) {
    this.dlgError.set('');
    try {
      const u = await this.api.unit(this.pid(), id); const a = this.au(); this.unit.set(u);
      this.unitForm.reset({ unitNo: u.unitNo, unitTypeId: u.unitTypeId, facing: u.facing ?? '', carpet: fromSqm(u.carpetSqm, a), builtUp: fromSqm(u.builtUpSqm, a),
        super: fromSqm(u.superSqm, a), balcony: fromSqm(u.balconySqm, a), plot: fromSqm(u.plotSqm, a), reason: '' });
      if (!this.auth.can('units.edit')) this.unitForm.disable(); else this.unitForm.enable();
      this.unitDlg.set(true);
    } catch (e) { this.error.set(errorText(e)); }
  }
  async saveUnit() {
    const u = this.unit()!; const v = this.unitForm.getRawValue(); const a = this.au();
    try {
      await this.api.saveUnit(this.pid(), u.id, { unitNo: v.unitNo, unitTypeId: v.unitTypeId, facing: v.facing || null, carpetSqm: toSqm(v.carpet, a), builtUpSqm: toSqm(v.builtUp, a),
        superSqm: toSqm(v.super, a), balconySqm: toSqm(v.balcony, a), plotSqm: toSqm(v.plot, a), reason: v.reason || null, rowVersion: u.rowVersion });
      await this.openUnit(u.id); await this.refresh();
    } catch (e) { this.dlgError.set(errorText(e)); }
  }
  async changeAvailability(to: string, reasonInput: HTMLInputElement) {
    const u = this.unit()!;
    try {
      await this.api.changeAvailability(this.pid(), u.id, { toStatus: to, reason: reasonInput.value, rowVersion: u.rowVersion });
      reasonInput.value = ''; await this.openUnit(u.id); await this.refresh();
    }
    catch (e) { this.dlgError.set(errorText(e)); }
  }
  async deleteUnit() {
    const reason = window.prompt('Reason for deleting this unit'); if (!reason) return;
    try { await this.api.deleteUnits(this.pid(), [this.unit()!.id], reason); this.unitDlg.set(false); await this.loadTree(); } catch (e) { this.dlgError.set(errorText(e)); }
  }
}
