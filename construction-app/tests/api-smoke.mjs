// End-to-end API check against a running API (default http://localhost:5160) and a test database.
// Usage: node tests/api-smoke.mjs   (reads TEST_ADMIN_USER / TEST_ADMIN_PASSWORD / TEST_VIEWER_PASSWORD from .local/dev.env)
// Expects the test fixture (tests/db/000_test_fixture.sql) in a cpmtest_ database.
// Never point this at absolutebook_2172 with real data: it creates records.
import { readFileSync } from 'node:fs';

const env = Object.fromEntries(readFileSync(new URL('../.local/dev.env', import.meta.url), 'utf8')
  .split(/\r?\n/).filter(l => /^[A-Za-z_]+=/.test(l)).map(l => [l.slice(0, l.indexOf('=')), l.slice(l.indexOf('=') + 1)]));
const base = process.env.API_URL ?? 'http://localhost:5160';
let token = '', cookie = '', failures = 0;
const stamp = Date.now().toString(36).toUpperCase();

async function call(method, path, body, { expect = [200, 201, 204], auth = true, headers = {} } = {}) {
  const res = await fetch(base + path, {
    method,
    headers: { 'Content-Type': 'application/json', 'X-Requested-With': 'XMLHttpRequest', ...(auth && token ? { Authorization: `Bearer ${token}` } : {}),
               ...(cookie ? { Cookie: cookie } : {}), ...headers },
    body: body === undefined ? undefined : JSON.stringify(body)
  });
  const set = res.headers.get('set-cookie'); if (set) cookie = set.split(';')[0];
  const text = await res.text(); const data = text ? JSON.parse(text) : null;
  if (!expect.includes(res.status)) throw new Error(`${method} ${path} → ${res.status} ${text}`);
  return { status: res.status, data };
}
async function check(name, fn) {
  try { const r = await fn(); console.log(`ok   ${name}${r ? ' · ' + r : ''}`); }
  catch (e) { failures++; console.log(`FAIL ${name} · ${e.message}`); }
}

await check('unauthenticated request is refused', async () => { await call('GET', '/api/projects', undefined, { expect: [401], auth: false }); });
await check('wrong password is refused', async () => {
  const r = await call('POST', '/api/auth/sign-in', { userName: env.TEST_ADMIN_USER, password: 'wrong-password-1' }, { expect: [401], auth: false });
  return r.data.message;
});
await check('sign in', async () => {
  const r = await call('POST', '/api/auth/sign-in', { userName: env.TEST_ADMIN_USER, password: env.TEST_ADMIN_PASSWORD }, { auth: false });
  token = r.data.accessToken; return `${r.data.user.fullName} of ${r.data.user.tenantName} (${r.data.user.profileName}), ${r.data.user.permissions.length} permissions, refresh cookie ${cookie ? 'set' : 'missing'}`;
});
await check('refresh rotates the token', async () => {
  const old = cookie; const r = await call('POST', '/api/auth/refresh', undefined, { auth: false });
  token = r.data.accessToken; if (cookie === old) throw new Error('cookie not rotated');
  await call('POST', '/api/auth/refresh', undefined, { auth: false, expect: [401], headers: { Cookie: old } });
  return 'old token refused after use';
});
await check('refresh after reuse is refused (all sessions revoked)', async () => {
  await call('POST', '/api/auth/refresh', undefined, { auth: false, expect: [401] });
  const r = await call('POST', '/api/auth/sign-in', { userName: env.TEST_ADMIN_USER, password: env.TEST_ADMIN_PASSWORD }, { auth: false });
  token = r.data.accessToken;
});

let locations, unitTypes, levels;
await check('masters', async () => {
  locations = (await call('GET', '/api/masters/locations')).data;
  unitTypes = (await call('GET', '/api/masters/unit-types')).data;
  levels = (await call('GET', '/api/masters/level-types')).data;
  return `${locations.countries.length} countries, ${locations.states.length} states, ${locations.cities.length} cities (ERP masters), ${unitTypes.length} unit types, ${levels.length} level types`;
});
const up = locations.states.find(s => s.name === 'Uttar Pradesh'); const noida = locations.cities.find(c => c.name === 'Noida');
const signInAs = async (userName, password) => { const keep = { token, cookie }; cookie = ''; const r = await call('POST', '/api/auth/sign-in', { userName, password }, { auth: false }); const t2 = r.data.accessToken; token = keep.token; cookie = keep.cookie; return t2; };
const lt = code => levels.find(l => l.code === code).id;

let landId;
await check('land parcel in acres is stored in sq m', async () => {
  landId = (await call('POST', '/api/land', { surveyNo: `GH-${stamp}`, locality: 'Sector 121', cityId: noida.id, stateId: up.id, areaEntered: 24.665,
    areaUnitEntered: 'acre', tenure: 'Lease', encumbrance: 'Under check', owners: [{ ownerName: 'Developer (lessee)', sharePct: 100 }] })).data.id;
  const p = (await call('GET', `/api/land/${landId}`)).data; return `${p.areaSqm} sq m`;
});
await check('owner shares must total 100', async () => {
  const r = await call('POST', '/api/land', { surveyNo: `X-${stamp}`, stateId: up.id, areaEntered: 10, areaUnitEntered: 'sqm', tenure: 'Owned',
    encumbrance: 'Clear', owners: [{ ownerName: 'A', sharePct: 60 }] }, { expect: [400] }); return r.data.message;
});

let projectId;
await check('project with land link', async () => {
  const land = (await call('GET', `/api/land/${landId}`)).data;
  projectId = (await call('POST', '/api/projects', { code: `CC${stamp}`, name: 'Cleo County (test)', developerEntity: 'IV County Pvt Ltd',
    projectType: 'Mixed', usesPhases: true, areaUnit: 'sqm', timeZoneId: 'Asia/Kolkata', currencyCode: 'INR', cityId: noida.id, stateId: up.id,
    startDate: '2012-05-01', targetDate: '2023-02-27', unitNumberPattern: '{T}-{FF}{NN}', status: 'Active',
    land: [{ landParcelId: landId, areaUsedSqm: land.areaSqm }], memberIds: [] })).data.id;
  const p = (await call('GET', `/api/projects/${projectId}`)).data; return `${p.code}, dates ${p.startDate} → ${p.targetDate}`;
});
await check('second project cannot over-use the land', async () => {
  const r = await call('POST', '/api/projects', { code: `OV${stamp}`, name: 'Over', developerEntity: 'X', projectType: 'Residential', usesPhases: false,
    areaUnit: 'sqm', timeZoneId: 'Asia/Kolkata', currencyCode: 'INR', unitNumberPattern: '{T}-{FF}{NN}', status: 'Planning',
    land: [{ landParcelId: landId, areaUsedSqm: 1 }], memberIds: [] }, { expect: [400] }); return r.data.message;
});

let phaseId, towerId, villaId;
await check('phase, tower and villa cluster', async () => {
  phaseId = (await call('POST', `/api/projects/${projectId}/phases`, { code: 'H1', name: 'Phase 1', registrationNo: 'UPRERAPRJ5931', sortOrder: 1 })).data.id;
  towerId = (await call('POST', `/api/projects/${projectId}/buildings`, { phaseId, code: 'A1', name: 'Tower A-1', buildingType: 'Tower', sortOrder: 1 })).data.id;
  villaId = (await call('POST', `/api/projects/${projectId}/buildings`, { phaseId, code: 'VC', name: 'Villa cluster', buildingType: 'Villa cluster',
    unitNumberPattern: 'V-{NN}', sortOrder: 2 })).data.id;
  return `phase ${phaseId}, tower ${towerId}, cluster ${villaId}`;
});
const floorReq = { basements: 1, basementLevelTypeId: lt('BAS'), groundLevelTypeId: lt('GRD'), podiums: 0, podiumLevelTypeId: lt('POD'),
  typicalFrom: 1, typicalTo: 27, typicalLevelTypeId: lt('TYP'), skipNumbers: '', terraceLevelTypeId: lt('TER'), heightM: 3.0 };
await check('floor preview and generate (B1, G, 01–27, T)', async () => {
  const pv = (await call('POST', `/api/projects/${projectId}/buildings/${towerId}/floors/preview`, floorReq)).data;
  const g = (await call('POST', `/api/projects/${projectId}/buildings/${towerId}/floors/generate`, floorReq)).data;
  const again = (await call('POST', `/api/projects/${projectId}/buildings/${towerId}/floors/generate`, floorReq)).data;
  return `preview ${pv.length}, created ${g.created}, second run skipped ${again.skipped}`;
});
let floors;
await check('unit preview and generate (4 per floor, G–27)', async () => {
  floors = (await call('GET', `/api/projects/${projectId}/buildings/${towerId}/floors`)).data;
  const t2 = unitTypes.find(u => u.code === '2BHK').id, t3 = unitTypes.find(u => u.code === '3BHK').id;
  const req = { fromFloorId: floors[0].id, toFloorId: floors[floors.length - 1].id, positions: [
    { position: 1, unitTypeId: t2, facing: 'East', carpetSqm: 72.12 }, { position: 2, unitTypeId: t2, facing: 'West', carpetSqm: 72.12 },
    { position: 3, unitTypeId: t3, facing: 'North', carpetSqm: 78.44 }, { position: 4, unitTypeId: t3, facing: 'South', carpetSqm: 78.44 }] };
  const pv = (await call('POST', `/api/projects/${projectId}/buildings/${towerId}/units/preview`, req)).data;
  const g = (await call('POST', `/api/projects/${projectId}/buildings/${towerId}/units/generate`, req)).data;
  const again = (await call('POST', `/api/projects/${projectId}/buildings/${towerId}/units/generate`, req)).data;
  return `preview ${pv.newCount} new (first ${pv.units[0].unitNo}), created ${g.created}, second run created ${again.created} skipped ${again.skipped}`;
});
await check('villas without floors', async () => {
  const villa = unitTypes.find(u => u.code === 'VILLA').id;
  const g = (await call('POST', `/api/projects/${projectId}/buildings/${villaId}/units/generate`,
    { positions: [1, 2, 3].map(n => ({ position: n, unitTypeId: villa, plotSqm: 240 })) })).data;
  const list = (await call('GET', `/api/projects/${projectId}/units?buildingId=${villaId}`)).data;
  return `created ${g.created}: ${list.items.map(u => u.unitNo).join(', ')}`;
});
await check('stacking plan', async () => {
  const s = (await call('GET', `/api/projects/${projectId}/buildings/${towerId}/stack`)).data;
  return `${s.floors.length} floors (top ${s.floors[0].code}), ${s.units.length} units`;
});
let unit;
await check('availability: allowed path with history, stale save refused', async () => {
  const list = (await call('GET', `/api/projects/${projectId}/units?buildingId=${towerId}&pageSize=1`)).data;
  unit = (await call('GET', `/api/projects/${projectId}/units/${list.items[0].id}`)).data;
  const bad = await call('POST', `/api/projects/${projectId}/units/${unit.id}/availability`, { toStatus: 'Handed over', reason: 'x', rowVersion: unit.rowVersion }, { expect: [400] });
  await call('POST', `/api/projects/${projectId}/units/${unit.id}/availability`, { toStatus: 'Booked', reason: 'Token received', rowVersion: unit.rowVersion });
  const stale = await call('POST', `/api/projects/${projectId}/units/${unit.id}/availability`, { toStatus: 'Sold', reason: 'x', rowVersion: unit.rowVersion }, { expect: [409] });
  const after = (await call('GET', `/api/projects/${projectId}/units/${unit.id}`)).data;
  return `${unit.unitNo}: ${after.availability}, history ${after.history.length}, next ${after.allowedNext.join('/')}; refused: "${bad.data.message}" / "${stale.data.message}"`;
});
await check('unit number change needs a reason', async () => {
  const u = (await call('GET', `/api/projects/${projectId}/units/${unit.id}`)).data;
  const r = await call('PUT', `/api/projects/${projectId}/units/${u.id}`, { unitNo: u.unitNo + 'X', unitTypeId: u.unitTypeId, carpetSqm: u.carpetSqm, rowVersion: u.rowVersion }, { expect: [400] });
  return r.data.message;
});
await check('audit history', async () => {
  const a = (await call('GET', `/api/admin/audit/Unit/${unit.id}`)).data; return `${a.length} entries: ${a.map(x => x.action).join(', ')}`;
});
await check('viewer profile: no project access, no edit rights', async () => {
  const keep = token; token = await signInAs('viewer1', env.TEST_VIEWER_PASSWORD);
  try {
    const list = (await call('GET', '/api/projects', undefined, { expect: [200, 403] })).data;
    await call('GET', `/api/projects/${projectId}`, undefined, { expect: [403, 404] });
    await call('POST', `/api/projects/${projectId}/phases`, { code: 'Z', name: 'Z', sortOrder: 9 }, { expect: [403] });
    return `projects visible: ${Array.isArray(list) ? list.length : 'none (no permission)'}; get refused; create phase 403`;
  } finally { token = keep; }
});
await check('profile permissions: give the Viewer profile project view, then it sees projects it is a member of', async () => {
  const profiles = (await call('GET', '/api/admin/profiles')).data; const viewer = profiles.find(p => p.name === 'Viewer');
  await call('PUT', `/api/admin/profiles/${viewer.id}/permissions`, { permissions: ['projects.view', 'land.view'] });
  const people = (await call('GET', '/api/admin/people')).data; const v = people.find(p => p.userName === 'viewer1');
  const p = (await call('GET', `/api/projects/${projectId}`)).data;
  await call('PUT', `/api/projects/${projectId}`, { ...p, land: p.land.map(l => ({ landParcelId: l.landParcelId, areaUsedSqm: l.areaUsedSqm })), memberIds: [...p.members.map(m => m.id), v.id] });
  const keep = token; token = await signInAs('viewer1', env.TEST_VIEWER_PASSWORD);
  try {
    const list = (await call('GET', '/api/projects')).data;
    await call('POST', `/api/projects/${projectId}/phases`, { code: 'Z', name: 'Z', sortOrder: 9 }, { expect: [403] });
    return `${profiles.length} profiles; viewer now sees ${list.length} project(s), still can't edit`;
  } finally { token = keep; }
});
await check('Admin profile cannot be edited', async () => {
  const profiles = (await call('GET', '/api/admin/profiles')).data; const admin = profiles.find(p => p.isAdmin);
  const r = await call('PUT', `/api/admin/profiles/${admin.id}/permissions`, { permissions: [] }, { expect: [400] }); return r.data.message;
});
await check('other company sees none of these projects', async () => {
  const keep = token; token = await signInAs('otheradmin', env.TEST_ADMIN_PASSWORD);
  try {
    const list = (await call('GET', '/api/projects')).data;
    await call('GET', `/api/projects/${projectId}`, undefined, { expect: [404] });
    return `other company sees ${list.length} projects; direct get 404`;
  } finally { token = keep; }
});
await check('username shared by two people is refused', async () => {
  const r = await call('POST', '/api/auth/sign-in', { userName: 'shared', password: env.TEST_VIEWER_PASSWORD }, { auth: false, expect: [409] }); return r.data.message;
});
await check('ERP-locked and archived employees are refused', async () => {
  const a = await call('POST', '/api/auth/sign-in', { userName: 'erplocked', password: env.TEST_VIEWER_PASSWORD }, { auth: false, expect: [403] });
  await call('POST', '/api/auth/sign-in', { userName: 'leftco', password: env.TEST_VIEWER_PASSWORD }, { auth: false, expect: [401] });
  return a.data.message;
});
await check('password is case-sensitive, like the ERP', async () => {
  await call('POST', '/api/auth/sign-in', { userName: 'viewer1', password: env.TEST_VIEWER_PASSWORD.toUpperCase() }, { auth: false, expect: [401] });
});

console.log(failures ? `\n${failures} check(s) failed` : '\nAll checks passed');
process.exit(failures ? 1 : 0);
