// Shapes returned by the API (camelCase JSON of the .NET contracts).

export interface SessionUser { userId: number; regId: number; userName: string; fullName: string; email: string; tenantName: string; profileName: string; permissions: string[]; }
export interface SignInResult { accessToken: string; expiresAtUtc: string; user: SessionUser; }
export interface IdResult { id: number; }
export interface PagedResult<T> { total: number; items: T[]; }
export interface GenerateResult { created: number; skipped: number; }

export interface PersonItem { id: number; fullName: string; userName: string; email: string; profileId: number | null; profileName: string; lastSignInUtc: string | null; lockoutEndUtc: string | null; }
export interface UserLookup { id: number; fullName: string; email: string; }
export interface PermissionItem { code: string; module: string; name: string; }
export interface ProfileItem { id: number; name: string; isAdmin: boolean; people: number; permissionList: string[]; }
export interface AuditItem { id: number; action: string; reason: string | null; oldData: string | null; newData: string | null; atUtc: string; userName: string | null; }

export interface CountryItem { id: number; name: string; }
export interface StateItem { id: number; countryId: number | null; name: string; }
export interface CityItem { id: number; name: string; }
export interface LocationLists { countries: CountryItem[]; states: StateItem[]; cities: CityItem[]; }

export interface UnitTypeItem {
  id: number; code: string; name: string; category: string; bedrooms: number | null; bathrooms: number | null;
  defaultCarpetSqm: number | null; defaultBuiltUpSqm: number | null; defaultSuperSqm: number | null; defaultPlotSqm: number | null;
  isSaleable: boolean; isActive: boolean; rowVersion: string; unitsUsing: number;
}
export interface LevelTypeItem { id: number; code: string; name: string; isHabitable: boolean; sortOrder: number; isActive: boolean; rowVersion: string; floorsUsing: number; }

export interface LandOwner { ownerName: string; sharePct: number; }
export interface LandParcelListItem {
  id: number; surveyNo: string; locality: string | null; city: string | null; state: string; areaSqm: number; areaEntered: number; areaUnitEntered: string;
  tenure: string; registrationNo: string | null; registrationDate: string | null; encumbrance: string; responsible: string | null; areaUsedSqm: number; projects: string | null;
}
export interface LandParcelDetail {
  id: number; surveyNo: string; locality: string | null; cityId: number | null; stateId: number; countryId: number; areaSqm: number; areaEntered: number;
  areaUnitEntered: string; tenure: string; registrationNo: string | null; registrationDate: string | null; registryOffice: string | null; landUse: string | null;
  encumbrance: string; latitude: number | null; longitude: number | null; responsibleUserId: number | null; remarks: string | null; rowVersion: string; owners: LandOwner[];
}

export interface ProjectListItem {
  id: number; code: string; name: string; developerEntity: string; projectType: string; status: string; city: string | null;
  startDate: string | null; targetDate: string | null; buildings: number; units: number; landSqm: number;
}
export interface ProjectLand { landParcelId: number; surveyNo: string; parcelAreaSqm: number; areaUsedSqm: number; availableSqm: number; }
export interface ProjectDetail {
  id: number; code: string; name: string; developerEntity: string; projectType: string; usesPhases: boolean; areaUnit: 'sqm' | 'sqft';
  timeZoneId: string; currencyCode: string; address: string | null; cityId: number | null; stateId: number | null; countryId: number | null;
  startDate: string | null; targetDate: string | null; unitNumberPattern: string; skipFloorNumbers: string | null; status: string; rowVersion: string;
  land: ProjectLand[]; members: UserLookup[];
}

export interface PhaseItem { id: number; code: string; name: string; registrationNo: string | null; plannedStart: string | null; plannedFinish: string | null; registeredCompletion: string | null; sortOrder: number; rowVersion: string; units: number; }
export interface BuildingItem {
  id: number; phaseId: number | null; code: string; name: string; buildingType: string; lifts: number | null; staircases: number | null; structureType: string | null;
  plannedStart: string | null; plannedFinish: string | null; unitNumberPattern: string | null; sortOrder: number; rowVersion: string; floors: number; units: number;
}
export interface StructureTree { phases: PhaseItem[]; buildings: BuildingItem[]; }
export interface FloorItem { id: number; code: string; name: string; levelTypeId: number; levelType: string; isHabitable: boolean; sortOrder: number; heightM: number | null; rowVersion: string; units: number; }
export interface FloorPreviewItem { code: string; name: string; levelTypeId: number; sortOrder: number; heightM: number | null; alreadyExists: boolean; }

export interface UnitListItem {
  id: number; unitNo: string; phaseId: number | null; phase: string | null; buildingId: number | null; building: string | null; floorId: number | null; floor: string | null;
  position: number | null; unitTypeId: number; unitType: string; facing: string | null; carpetSqm: number | null; builtUpSqm: number | null; superSqm: number | null;
  balconySqm: number | null; plotSqm: number | null; availability: string;
}
export interface AvailabilityHistoryItem { id: number; fromStatus: string | null; toStatus: string; reason: string; changedAtUtc: string; changedBy: string | null; }
export interface UnitDetail {
  id: number; unitNo: string; phaseId: number | null; buildingId: number | null; building: string | null; floorId: number | null; floor: string | null; position: number | null;
  unitTypeId: number; facing: string | null; carpetSqm: number | null; builtUpSqm: number | null; superSqm: number | null; balconySqm: number | null; plotSqm: number | null;
  availability: string; rowVersion: string; history: AvailabilityHistoryItem[]; allowedNext: string[];
}
export interface UnitPreviewItem { unitNo: string; floorId: number | null; floorCode: string | null; position: number; unitTypeId: number; carpetSqm: number | null; alreadyExists: boolean; }
export interface UnitPreview { pattern: string; newCount: number; existingCount: number; units: UnitPreviewItem[]; }
export interface StackFloor { id: number; code: string; name: string; isHabitable: boolean; sortOrder: number; }
export interface StackUnit { id: number; unitNo: string; floorId: number | null; position: number | null; unitType: string; carpetSqm: number | null; availability: string; }
export interface StackingPlan { floors: StackFloor[]; units: StackUnit[]; }

export const AVAILABILITY = ['Available', 'Blocked', 'Booked', 'Sold', 'Registered', 'Handed over'] as const;
export const BUILDING_TYPES = ['Tower', 'Block', 'Commercial', 'Villa cluster', 'Plot sector'] as const;
export const PROJECT_TYPES = ['Residential', 'Commercial', 'Mixed', 'Villas', 'Plots'] as const;
export const PROJECT_STATUSES = ['Planning', 'Active', 'On hold', 'Completed'] as const;
export const UNIT_CATEGORIES = ['Residential', 'Villa', 'Plot', 'Commercial', 'Office', 'Parking', 'Other'] as const;
export const TENURES = ['Owned', 'Lease', 'Joint development'] as const;
export const ENCUMBRANCES = ['Clear', 'Charged', 'Under check'] as const;
export const LAND_UNITS = [{ v: 'sqm', t: 'sq m' }, { v: 'sqft', t: 'sq ft' }, { v: 'acre', t: 'acre' }, { v: 'hectare', t: 'hectare' }] as const;
