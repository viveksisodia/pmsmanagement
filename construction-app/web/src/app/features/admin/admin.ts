import { Component, computed, inject, signal } from '@angular/core';
import { Api } from '../../core/api.service';
import { DateTimePipe } from '../../core/format';
import { errorText } from '../../core/http';
import { PermissionItem, PersonItem, ProfileItem } from '../../core/models';
import { Modal } from '../../shared/ui';

/** People come from tbl_employee and are managed in the ERP. Here they are only listed, with their sign-in state. */
@Component({
  selector: 'app-people',
  imports: [DateTimePipe],
  template: `
    <div class="bar"><div class="grow"><h1>People</h1>
      <p class="lead">Active employees of this company from the ERP. Add people, change passwords and profiles in the ERP; they apply here straight away.</p></div>
      <input type="search" placeholder="Search name, username or email" aria-label="Search people" (input)="search.set($any($event.target).value); load()" /></div>
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="tw"><table><thead><tr><th>Name</th><th>Username</th><th>Email</th><th>Profile</th><th>Last sign-in here</th><th>Sign-in</th><th></th></tr></thead><tbody>
      @for (p of rows(); track p.id) {
        <tr><td>{{ p.fullName }}</td><td class="mono">{{ p.userName }}</td><td>{{ p.email || '—' }}</td><td>{{ p.profileName || '—' }}</td>
          <td>{{ p.lastSignInUtc | dt }}</td><td>{{ locked(p) ? 'Locked until ' + (p.lockoutEndUtc | dt) : 'OK' }}</td>
          <td>@if (locked(p)) { <button type="button" class="sm" (click)="unlock(p.id)">Unlock</button> }</td></tr>
      } @empty { <tr><td colspan="7" class="hint">No people found.</td></tr> }
    </tbody></table></div>`
})
export class People {
  private api = inject(Api);
  rows = signal<PersonItem[]>([]); search = signal(''); error = signal('');
  constructor() { this.load(); }
  locked(p: PersonItem) { return !!p.lockoutEndUtc && new Date(p.lockoutEndUtc.endsWith('Z') ? p.lockoutEndUtc : p.lockoutEndUtc + 'Z') > new Date(); }
  async load() { try { this.rows.set(await this.api.peopleList(this.search())); } catch (e) { this.error.set(errorText(e)); } }
  async unlock(id: number) { try { await this.api.unlock(id); await this.load(); } catch (e) { this.error.set(errorText(e)); } }
}

/** Construction permissions for each ERP profile (Manage_Profile). The Admin profile always has all of them. */
@Component({
  selector: 'app-profiles',
  imports: [Modal],
  template: `
    <div class="bar"><div class="grow"><h1>Profile permissions</h1>
      <p class="lead">Profiles come from the ERP. Choose what each profile may do in the construction app. The ERP Admin profile always has every permission.</p></div></div>
    @if (error()) { <p class="err" role="alert">{{ error() }}</p> }
    <div class="tw"><table><thead><tr><th>Profile</th><th class="num">People</th><th>Construction permissions</th><th></th></tr></thead><tbody>
      @for (p of rows(); track p.id) {
        <tr><td>{{ p.name }} @if (p.isAdmin) { <small class="hint">ERP Admin</small> }</td><td class="num">{{ p.people }}</td>
          <td>{{ names(p.permissionList) }}</td>
          <td>@if (!p.isAdmin) { <button type="button" class="sm" (click)="open(p)">Edit</button> }</td></tr>
      }</tbody></table></div>
    <app-modal [heading]="'Permissions · ' + (edit()?.name ?? '')" [open]="dialog()" [wide]="true" (closed)="dialog.set(false)">
      @for (m of modules(); track m) {
        <h3>{{ m }}</h3>
        <div class="checks">@for (p of byModule(m); track p.code) {
          <label class="chk"><input type="checkbox" [checked]="selected().includes(p.code)" (change)="toggle(p.code)" /> {{ p.name }}</label> }</div>
      }
      @if (formError()) { <p class="err" role="alert">{{ formError() }}</p> }
      <ng-container actions><span class="grow"></span><button type="button" (click)="dialog.set(false)">Cancel</button>
        <button type="button" class="pri" (click)="save()">Save permissions</button></ng-container>
    </app-modal>`
})
export class Profiles {
  private api = inject(Api);
  rows = signal<ProfileItem[]>([]); perms = signal<PermissionItem[]>([]); error = signal('');
  dialog = signal(false); edit = signal<ProfileItem | null>(null); selected = signal<string[]>([]); formError = signal('');
  modules = computed(() => [...new Set(this.perms().map(p => p.module))]);
  constructor() { this.load(); this.api.permissions().then(p => this.perms.set(p)).catch(e => this.error.set(errorText(e))); }
  async load() { try { this.rows.set(await this.api.profiles()); } catch (e) { this.error.set(errorText(e)); } }
  byModule(m: string) { return this.perms().filter(p => p.module === m); }
  names(codes: string[]) { return codes.map(c => this.perms().find(p => p.code === c)?.name ?? c).join(', ') || 'None'; }
  toggle(c: string) { this.selected.update(s => s.includes(c) ? s.filter(x => x !== c) : [...s, c]); }
  open(p: ProfileItem) { this.edit.set(p); this.formError.set(''); this.selected.set(p.permissionList); this.dialog.set(true); }
  async save() {
    try { await this.api.saveProfile(this.edit()!.id, this.selected()); this.dialog.set(false); await this.load(); }
    catch (e) { this.formError.set(errorText(e)); }
  }
}
