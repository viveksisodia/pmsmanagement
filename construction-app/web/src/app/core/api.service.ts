import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable, inject } from '@angular/core';
import { firstValueFrom } from 'rxjs';
import * as M from './models';

/** Typed calls to the construction API. Every method returns a promise. */
@Injectable({ providedIn: 'root' })
export class Api {
  private http = inject(HttpClient);
  private get = <T>(url: string, params?: Record<string, unknown>) => firstValueFrom(this.http.get<T>(url, { params: toParams(params) }));
  private post = <T>(url: string, body: unknown) => firstValueFrom(this.http.post<T>(url, body));
  private put = <T>(url: string, body: unknown) => firstValueFrom(this.http.put<T>(url, body));

  // auth and admin
  peopleList = (search?: string) => this.get<M.PersonItem[]>('/api/admin/people', { search });
  unlock = (id: number) => this.post<void>(`/api/admin/people/${id}/unlock`, null);
  permissions = () => this.get<M.PermissionItem[]>('/api/admin/permissions');
  profiles = () => this.get<M.ProfileItem[]>('/api/admin/profiles');
  saveProfile = (id: number, permissions: string[]) => this.put<void>(`/api/admin/profiles/${id}/permissions`, { permissions });
  audit = (entityType: string, entityId: number) => this.get<M.AuditItem[]>(`/api/admin/audit/${entityType}/${entityId}`);

  // masters
  locations = () => this.get<M.LocationLists>('/api/masters/locations');
  unitTypes = (includeInactive = false) => this.get<M.UnitTypeItem[]>('/api/masters/unit-types', { includeInactive });
  saveUnitType = (id: number | null, body: unknown) => id ? this.put<M.IdResult>(`/api/masters/unit-types/${id}`, body) : this.post<M.IdResult>('/api/masters/unit-types', body);
  levelTypes = () => this.get<M.LevelTypeItem[]>('/api/masters/level-types');
  saveLevelType = (id: number | null, body: unknown) => id ? this.put<M.IdResult>(`/api/masters/level-types/${id}`, body) : this.post<M.IdResult>('/api/masters/level-types', body);
  people = () => this.get<M.UserLookup[]>('/api/masters/people');

  // land
  landList = (search?: string) => this.get<M.LandParcelListItem[]>('/api/land', { search });
  land = (id: number) => this.get<M.LandParcelDetail>(`/api/land/${id}`);
  saveLand = (id: number | null, body: unknown) => id ? this.put<M.IdResult>(`/api/land/${id}`, body) : this.post<M.IdResult>('/api/land', body);
  deleteLand = (id: number, reason: string) => this.post<void>(`/api/land/${id}/delete`, { reason });

  // projects
  projects = () => this.get<M.ProjectListItem[]>('/api/projects');
  project = (id: number) => this.get<M.ProjectDetail>(`/api/projects/${id}`);
  saveProject = (id: number | null, body: unknown) => id ? this.put<M.IdResult>(`/api/projects/${id}`, body) : this.post<M.IdResult>('/api/projects', body);
  deleteProject = (id: number, reason: string) => this.post<void>(`/api/projects/${id}/delete`, { reason });

  // structure
  structure = (p: number) => this.get<M.StructureTree>(`/api/projects/${p}/structure`);
  savePhase = (p: number, id: number | null, body: unknown) => id ? this.put<M.IdResult>(`/api/projects/${p}/phases/${id}`, body) : this.post<M.IdResult>(`/api/projects/${p}/phases`, body);
  deletePhase = (p: number, id: number, reason: string) => this.post<void>(`/api/projects/${p}/phases/${id}/delete`, { reason });
  saveBuilding = (p: number, id: number | null, body: unknown) => id ? this.put<M.IdResult>(`/api/projects/${p}/buildings/${id}`, body) : this.post<M.IdResult>(`/api/projects/${p}/buildings`, body);
  deleteBuilding = (p: number, id: number, reason: string) => this.post<void>(`/api/projects/${p}/buildings/${id}/delete`, { reason });
  floors = (p: number, b: number) => this.get<M.FloorItem[]>(`/api/projects/${p}/buildings/${b}/floors`);
  previewFloors = (p: number, b: number, body: unknown) => this.post<M.FloorPreviewItem[]>(`/api/projects/${p}/buildings/${b}/floors/preview`, body);
  generateFloors = (p: number, b: number, body: unknown) => this.post<M.GenerateResult>(`/api/projects/${p}/buildings/${b}/floors/generate`, body);
  saveFloor = (p: number, id: number, body: unknown) => this.put<M.IdResult>(`/api/projects/${p}/floors/${id}`, body);
  deleteFloor = (p: number, id: number, reason: string) => this.post<void>(`/api/projects/${p}/floors/${id}/delete`, { reason });

  // units
  units = (p: number, q: Record<string, unknown>) => this.get<M.PagedResult<M.UnitListItem>>(`/api/projects/${p}/units`, q);
  stack = (p: number, b: number) => this.get<M.StackingPlan>(`/api/projects/${p}/buildings/${b}/stack`);
  previewUnits = (p: number, b: number, body: unknown) => this.post<M.UnitPreview>(`/api/projects/${p}/buildings/${b}/units/preview`, body);
  generateUnits = (p: number, b: number, body: unknown) => this.post<M.GenerateResult>(`/api/projects/${p}/buildings/${b}/units/generate`, body);
  unit = (p: number, id: number) => this.get<M.UnitDetail>(`/api/projects/${p}/units/${id}`);
  saveUnit = (p: number, id: number, body: unknown) => this.put<M.IdResult>(`/api/projects/${p}/units/${id}`, body);
  changeAvailability = (p: number, id: number, body: unknown) => this.post<void>(`/api/projects/${p}/units/${id}/availability`, body);
  deleteUnits = (p: number, ids: number[], reason: string) => this.post<{ deleted: number }>(`/api/projects/${p}/units/delete`, { ids, reason });
}

function toParams(p?: Record<string, unknown>): HttpParams | undefined {
  if (!p) return undefined;
  let h = new HttpParams();
  for (const [k, v] of Object.entries(p)) if (v !== undefined && v !== null && v !== '') h = h.set(k, String(v));
  return h;
}
