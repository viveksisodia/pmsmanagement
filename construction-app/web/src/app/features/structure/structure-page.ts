import { Component, computed, effect, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { RouterLink } from '@angular/router';
import { Api } from '../../core/api.service';
import { AuthService } from '../../core/auth.service';
import { DatePipe } from '../../core/format';
import { errorText } from '../../core/http';
import { BUILDING_TYPES, BuildingItem, FloorItem, FloorPreviewItem, LevelTypeItem, PhaseItem, StructureTree } from '../../core/models';
import { DateInput, Modal } from '../../shared/ui';
import { ProjectContext } from '../projects/project-context';

@Component({
  selector: 'app-structure-page',
  imports: [ReactiveFormsModule, RouterLink, Modal, DateInput, DatePipe],
  template: `
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="split">
      <aside class="tree card" aria-label="Structure">
        <div class="bar"><b class="grow">{{ project()?.name }}</b></div>
        @if (project()?.usesPhases) {
          @for (ph of tree()?.phases ?? []; track ph.id) {
            <div class="node phase"><button type="button" class="link" (click)="editPhase(ph)">{{ ph.name }}</button>
              <small class="hint mono">{{ ph.registrationNo }}</small></div>
            @for (b of buildingsOf(ph.id); track b.id) {
              <button type="button" class="node" [class.on]="b.id === selectedId()" (click)="select(b.id)"><span>{{ b.name }}</span><small>{{ b.floors }} fl · {{ b.units }} u</small></button>
            } @empty { <p class="hint ind">No buildings in this phase.</p> }
          } @empty { <p class="hint">Add the first phase.</p> }
        } @else {
          @for (b of tree()?.buildings ?? []; track b.id) {
            <button type="button" class="node" [class.on]="b.id === selectedId()" (click)="select(b.id)"><span>{{ b.name }}</span><small>{{ b.floors }} fl · {{ b.units }} u</small></button>
          } @empty { <p class="hint">Add the first building.</p> }
        }
        @if (auth.can('structure.edit')) {
          <div class="tree-acts">
            @if (project()?.usesPhases) { <button type="button" class="sm" (click)="editPhase(null)">Add phase</button> }
            <button type="button" class="sm" (click)="editBuilding(null)">Add building</button>
          </div>
        }
      </aside>

      <section class="detail">
        @if (selected(); as b) {
          <div class="card">
            <div class="bar"><div class="grow"><h2>{{ b.name }} <small class="pill">{{ b.buildingType }}</small></h2>
              <p class="hint">Code {{ b.code }} · {{ phaseName(b.phaseId) }} · planned {{ b.plannedStart | d }} – {{ b.plannedFinish | d }}
                · units numbered {{ b.unitNumberPattern || project()?.unitNumberPattern }}</p></div>
              @if (auth.can('structure.edit')) { <button type="button" class="sm" (click)="editBuilding(b)">Edit building</button> }
              @if (hasFloors() && auth.can('units.generate')) { <button type="button" class="sm pri" (click)="openFloorGen()">Generate floors</button> }
              <a class="btn sm" [routerLink]="['..', 'units']" [queryParams]="{ building: b.id }">Units</a>
            </div>
          </div>
          @if (hasFloors()) {
            <div class="tw"><table>
              <thead><tr><th class="num">Sort</th><th>Code</th><th>Name</th><th>Level type</th><th>Habitable</th><th class="num">Height (m)</th><th class="num">Units</th><th></th></tr></thead>
              <tbody>@for (f of floors(); track f.id) {
                <tr><td class="num">{{ f.sortOrder }}</td><td class="mono">{{ f.code }}</td><td>{{ f.name }}</td><td>{{ f.levelType }}</td><td>{{ f.isHabitable ? 'Yes' : 'No' }}</td>
                  <td class="num">{{ f.heightM ?? '—' }}</td><td class="num">{{ f.units }}</td>
                  <td>@if (auth.can('structure.edit')) { <button type="button" class="sm" (click)="editFloor(f)">Edit</button> }</td></tr>
              } @empty { <tr><td colspan="8" class="hint">No floors yet. Use Generate floors.</td></tr> }</tbody>
            </table></div>
          } @else { <p class="hint card">Villa clusters and plot sectors hold units directly, without floors. Add them from the Units tab.</p> }
        } @else { <p class="hint card">Choose a building on the left, or add one.</p> }
      </section>
    </div>

    <!-- phase -->
    <app-modal [heading]="phaseEdit()?.id ? 'Edit phase' : 'Add phase'" [open]="phaseDlg()" (closed)="phaseDlg.set(false)">
      <form [formGroup]="phaseForm" id="ph-form" (ngSubmit)="savePhase()"><div class="grid">
        <div class="f"><label for="ph-code">Code <span class="req">*</span></label><input id="ph-code" formControlName="code" /></div>
        <div class="f"><label for="ph-name">Name <span class="req">*</span></label><input id="ph-name" formControlName="name" /></div>
        <div class="f"><label for="ph-reg">Registration no. (e.g. RERA)</label><input id="ph-reg" formControlName="registrationNo" /></div>
        <div class="f"><label for="ph-sort">Sort order</label><input id="ph-sort" type="number" formControlName="sortOrder" /></div>
        <div class="f"><label for="ph-ps">Planned start</label><app-date-input inputId="ph-ps" formControlName="plannedStart" /></div>
        <div class="f"><label for="ph-pf">Planned finish</label><app-date-input inputId="ph-pf" formControlName="plannedFinish" /></div>
        <div class="f"><label for="ph-rc">Registered completion</label><app-date-input inputId="ph-rc" formControlName="registeredCompletion" /></div>
      </div>@if (dlgError()) { <p class="err" role="alert">{{ dlgError() }}</p> }</form>
      <ng-container actions>@if (phaseEdit()?.id) { <button type="button" class="dan" (click)="deletePhase()">Delete</button> }<span class="grow"></span>
        <button type="button" (click)="phaseDlg.set(false)">Cancel</button><button type="submit" form="ph-form" class="pri">Save phase</button></ng-container>
    </app-modal>

    <!-- building -->
    <app-modal [heading]="buildingEdit()?.id ? 'Edit building' : 'Add building'" [open]="buildingDlg()" (closed)="buildingDlg.set(false)">
      <form [formGroup]="buildingForm" id="bd-form" (ngSubmit)="saveBuilding()"><div class="grid">
        @if (project()?.usesPhases) {
          <div class="f"><label for="bd-phase">Phase <span class="req">*</span></label><select id="bd-phase" formControlName="phaseId">
            <option [ngValue]="null">Choose…</option>@for (p of tree()?.phases ?? []; track p.id) { <option [ngValue]="p.id">{{ p.name }}</option> }</select></div>
        }
        <div class="f"><label for="bd-code">Code <span class="req">*</span></label><input id="bd-code" formControlName="code" /></div>
        <div class="f"><label for="bd-name">Name <span class="req">*</span></label><input id="bd-name" formControlName="name" /></div>
        <div class="f"><label for="bd-type">Type</label><select id="bd-type" formControlName="buildingType">@for (t of buildingTypes; track t) { <option [value]="t">{{ t }}</option> }</select></div>
        <div class="f"><label for="bd-lifts">Lifts</label><input id="bd-lifts" type="number" min="0" formControlName="lifts" /></div>
        <div class="f"><label for="bd-stairs">Staircases</label><input id="bd-stairs" type="number" min="0" formControlName="staircases" /></div>
        <div class="f"><label for="bd-struct">Structure type</label><input id="bd-struct" formControlName="structureType" placeholder="RCC frame" /></div>
        <div class="f"><label for="bd-sort">Sort order</label><input id="bd-sort" type="number" formControlName="sortOrder" /></div>
        <div class="f"><label for="bd-ps">Planned start</label><app-date-input inputId="bd-ps" formControlName="plannedStart" /></div>
        <div class="f"><label for="bd-pf">Planned finish</label><app-date-input inputId="bd-pf" formControlName="plannedFinish" /></div>
        <div class="f"><label for="bd-pat">Own unit number pattern</label><input id="bd-pat" formControlName="unitNumberPattern" [placeholder]="project()?.unitNumberPattern ?? ''" />
          <small class="hint">Leave empty to use the project's pattern.</small></div>
      </div>@if (dlgError()) { <p class="err" role="alert">{{ dlgError() }}</p> }</form>
      <ng-container actions>@if (buildingEdit()?.id) { <button type="button" class="dan" (click)="deleteBuilding()">Delete</button> }<span class="grow"></span>
        <button type="button" (click)="buildingDlg.set(false)">Cancel</button><button type="submit" form="bd-form" class="pri">Save building</button></ng-container>
    </app-modal>

    <!-- floor generator -->
    <app-modal heading="Generate floors" [open]="genDlg()" [wide]="true" (closed)="genDlg.set(false)">
      <form [formGroup]="genForm" id="fg-form" (ngSubmit)="generateFloors()"><div class="grid">
        <div class="f"><label for="fg-bas">Basements</label><input id="fg-bas" type="number" min="0" max="10" formControlName="basements" /></div>
        <div class="f"><label for="fg-grd">Ground level</label><select id="fg-grd" formControlName="groundLevelTypeId"><option [ngValue]="null">No ground floor</option>
          @for (l of levels(); track l.id) { <option [ngValue]="l.id">{{ l.name }}{{ l.isHabitable ? ' (gets units)' : '' }}</option> }</select></div>
        <div class="f"><label for="fg-pod">Podiums</label><input id="fg-pod" type="number" min="0" max="10" formControlName="podiums" /></div>
        <div class="f"><label for="fg-from">Typical floors from</label><input id="fg-from" type="number" min="1" formControlName="typicalFrom" /></div>
        <div class="f"><label for="fg-to">Typical floors to</label><input id="fg-to" type="number" min="0" formControlName="typicalTo" /></div>
        <div class="f"><label for="fg-skip">Skip floor numbers</label><input id="fg-skip" formControlName="skipNumbers" placeholder="13" /></div>
        <label class="chk"><input type="checkbox" formControlName="terrace" /> Add terrace</label>
        <div class="f"><label for="fg-h">Floor height (m)</label><input id="fg-h" type="number" step="0.01" min="0" formControlName="heightM" /></div>
      </div>
      <div class="bar left"><button type="button" class="sm" (click)="previewFloors()">Preview</button>
        @if (preview().length) { <small class="hint">{{ newCount() }} new floors, {{ preview().length - newCount() }} already exist and will be skipped.</small> }</div>
      @if (preview().length) {
        <div class="chips">@for (p of preview(); track p.code) { <span class="chip" [class.dim]="p.alreadyExists">{{ p.code }}</span> }</div>
      }
      @if (dlgError()) { <p class="err" role="alert">{{ dlgError() }}</p> }</form>
      <ng-container actions><span class="grow"></span><button type="button" (click)="genDlg.set(false)">Cancel</button>
        <button type="submit" form="fg-form" class="pri" [disabled]="busy()">Create floors</button></ng-container>
    </app-modal>

    <!-- floor edit -->
    <app-modal heading="Edit floor" [open]="floorDlg()" (closed)="floorDlg.set(false)">
      <form [formGroup]="floorForm" id="fl-form" (ngSubmit)="saveFloor()"><div class="grid">
        <div class="f"><label for="fl-code">Code</label><input id="fl-code" formControlName="code" /></div>
        <div class="f"><label for="fl-name">Name</label><input id="fl-name" formControlName="name" /></div>
        <div class="f"><label for="fl-lt">Level type</label><select id="fl-lt" formControlName="levelTypeId">@for (l of levels(); track l.id) { <option [ngValue]="l.id">{{ l.name }}</option> }</select></div>
        <div class="f"><label for="fl-sort">Sort order</label><input id="fl-sort" type="number" formControlName="sortOrder" /></div>
        <div class="f"><label for="fl-h">Height (m)</label><input id="fl-h" type="number" step="0.01" formControlName="heightM" /></div>
      </div>@if (dlgError()) { <p class="err" role="alert">{{ dlgError() }}</p> }</form>
      <ng-container actions><button type="button" class="dan" (click)="deleteFloor()">Delete</button><span class="grow"></span>
        <button type="button" (click)="floorDlg.set(false)">Cancel</button><button type="submit" form="fl-form" class="pri">Save floor</button></ng-container>
    </app-modal>`
})
export class StructurePage {
  auth = inject(AuthService); private ctx = inject(ProjectContext); private api = inject(Api); private fb = inject(FormBuilder);
  readonly buildingTypes = BUILDING_TYPES;
  project = this.ctx.project;
  tree = signal<StructureTree | null>(null); floors = signal<FloorItem[]>([]); levels = signal<LevelTypeItem[]>([]);
  selectedId = signal<number | null>(null); error = signal(''); dlgError = signal(''); busy = signal(false);
  selected = computed(() => this.tree()?.buildings.find(b => b.id === this.selectedId()) ?? null);
  hasFloors = computed(() => !['Villa cluster', 'Plot sector'].includes(this.selected()?.buildingType ?? ''));
  phaseDlg = signal(false); phaseEdit = signal<PhaseItem | null>(null);
  buildingDlg = signal(false); buildingEdit = signal<BuildingItem | null>(null);
  genDlg = signal(false); preview = signal<FloorPreviewItem[]>([]); newCount = computed(() => this.preview().filter(p => !p.alreadyExists).length);
  floorDlg = signal(false); floorEdit = signal<FloorItem | null>(null);

  phaseForm = this.fb.group({ code: ['', Validators.required], name: ['', Validators.required], registrationNo: [''], sortOrder: [0],
    plannedStart: [null as string | null], plannedFinish: [null as string | null], registeredCompletion: [null as string | null] });
  buildingForm = this.fb.group({ phaseId: [null as number | null], code: ['', Validators.required], name: ['', Validators.required], buildingType: ['Tower'],
    lifts: [null as number | null], staircases: [null as number | null], structureType: [''], sortOrder: [0],
    plannedStart: [null as string | null], plannedFinish: [null as string | null], unitNumberPattern: [''] });
  genForm = this.fb.nonNullable.group({ basements: [0], groundLevelTypeId: [null as number | null], podiums: [0], typicalFrom: [1], typicalTo: [10],
    skipNumbers: [''], terrace: [true], heightM: [3 as number | null] });
  floorForm = this.fb.nonNullable.group({ code: [''], name: [''], levelTypeId: [0], sortOrder: [0], heightM: [null as number | null] });

  constructor() {
    this.api.levelTypes().then(l => this.levels.set(l));
    effect(() => { if (this.project()) this.loadTree(); });
  }

  private pid() { return this.project()!.id; }
  buildingsOf(phaseId: number) { return (this.tree()?.buildings ?? []).filter(b => b.phaseId === phaseId); }
  phaseName(id: number | null) { return this.tree()?.phases.find(p => p.id === id)?.name ?? 'No phase'; }
  level(code: string) { return this.levels().find(l => l.code === code)?.id ?? null; }

  async loadTree() {
    try {
      const t = await this.api.structure(this.pid()); this.tree.set(t);
      if (!this.selectedId() && t.buildings.length) this.select(t.buildings[0].id);
      else if (this.selectedId()) await this.loadFloors();
    } catch (e) { this.error.set(errorText(e)); }
  }
  async select(id: number) { this.selectedId.set(id); await this.loadFloors(); }
  async loadFloors() {
    const b = this.selected(); if (!b || !this.hasFloors()) { this.floors.set([]); return; }
    this.floors.set(await this.api.floors(this.pid(), b.id));
  }

  editPhase(p: PhaseItem | null) { this.dlgError.set(''); this.phaseEdit.set(p); this.phaseForm.reset(p ?? { sortOrder: (this.tree()?.phases.length ?? 0) + 1 }); this.phaseDlg.set(true); }
  async savePhase() {
    if (this.phaseForm.invalid) { this.dlgError.set('Enter the code and name.'); return; }
    try { await this.api.savePhase(this.pid(), this.phaseEdit()?.id ?? null, { ...this.phaseForm.getRawValue(), rowVersion: this.phaseEdit()?.rowVersion }); this.phaseDlg.set(false); await this.loadTree(); }
    catch (e) { this.dlgError.set(errorText(e)); }
  }
  async deletePhase() {
    const reason = window.prompt('Reason for deleting this phase'); if (!reason) return;
    try { await this.api.deletePhase(this.pid(), this.phaseEdit()!.id, reason); this.phaseDlg.set(false); await this.loadTree(); } catch (e) { this.dlgError.set(errorText(e)); }
  }

  editBuilding(b: BuildingItem | null) {
    this.dlgError.set(''); this.buildingEdit.set(b);
    this.buildingForm.reset(b ? { ...b, structureType: b.structureType ?? '', unitNumberPattern: b.unitNumberPattern ?? '' }
                              : { buildingType: 'Tower', sortOrder: (this.tree()?.buildings.length ?? 0) + 1, phaseId: this.tree()?.phases.at(-1)?.id ?? null });
    this.buildingDlg.set(true);
  }
  async saveBuilding() {
    if (this.buildingForm.invalid) { this.dlgError.set('Enter the code and name.'); return; }
    try {
      const r = await this.api.saveBuilding(this.pid(), this.buildingEdit()?.id ?? null, { ...this.buildingForm.getRawValue(), rowVersion: this.buildingEdit()?.rowVersion });
      this.buildingDlg.set(false); this.selectedId.set(r.id); await this.loadTree();
    } catch (e) { this.dlgError.set(errorText(e)); }
  }
  async deleteBuilding() {
    const reason = window.prompt('Reason for deleting this building'); if (!reason) return;
    try { await this.api.deleteBuilding(this.pid(), this.buildingEdit()!.id, reason); this.buildingDlg.set(false); this.selectedId.set(null); await this.loadTree(); }
    catch (e) { this.dlgError.set(errorText(e)); }
  }

  openFloorGen() {
    this.dlgError.set(''); this.preview.set([]);
    this.genForm.patchValue({ groundLevelTypeId: this.level('GRD') });
    this.genDlg.set(true);
  }
  private genBody() {
    const v = this.genForm.getRawValue();
    return { basements: v.basements, basementLevelTypeId: this.level('BAS') ?? 0, groundLevelTypeId: v.groundLevelTypeId, podiums: v.podiums,
             podiumLevelTypeId: this.level('POD') ?? 0, typicalFrom: v.typicalFrom, typicalTo: v.typicalTo, typicalLevelTypeId: this.level('TYP') ?? 0,
             skipNumbers: v.skipNumbers, terraceLevelTypeId: v.terrace ? this.level('TER') : null, heightM: v.heightM };
  }
  async previewFloors() {
    this.dlgError.set('');
    try { this.preview.set(await this.api.previewFloors(this.pid(), this.selectedId()!, this.genBody())); } catch (e) { this.dlgError.set(errorText(e)); }
  }
  async generateFloors() {
    this.busy.set(true); this.dlgError.set('');
    try { await this.api.generateFloors(this.pid(), this.selectedId()!, this.genBody()); this.genDlg.set(false); await this.loadTree(); }
    catch (e) { this.dlgError.set(errorText(e)); }
    finally { this.busy.set(false); }
  }

  editFloor(f: FloorItem) { this.dlgError.set(''); this.floorEdit.set(f); this.floorForm.reset(f); this.floorDlg.set(true); }
  async saveFloor() {
    const f = this.floorEdit()!;
    try { await this.api.saveFloor(this.pid(), f.id, { ...this.floorForm.getRawValue(), rowVersion: f.rowVersion }); this.floorDlg.set(false); await this.loadFloors(); }
    catch (e) { this.dlgError.set(errorText(e)); }
  }
  async deleteFloor() {
    const reason = window.prompt('Reason for deleting this floor'); if (!reason) return;
    try { await this.api.deleteFloor(this.pid(), this.floorEdit()!.id, reason); this.floorDlg.set(false); await this.loadTree(); } catch (e) { this.dlgError.set(errorText(e)); }
  }
}
