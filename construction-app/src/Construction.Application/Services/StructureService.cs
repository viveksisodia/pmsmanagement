using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Contracts;
using Construction.Domain;
using Construction.Domain.Areas;
using Construction.Domain.Structure;

namespace Construction.Application.Services;

/// <summary>Floor and unit generators: preview first, then create. Rules live in the domain; the database re-checks them.</summary>
public sealed class StructureService(IStructureRepository structure, IUnitRepository units, IProjectRepository projects)
{
    public async Task<IReadOnlyList<FloorPreviewItem>> PreviewFloorsAsync(int regId, int userId, int projectId, int buildingId, FloorGenerateRequest r)
    {
        var plan = PlanFloors(r);
        var existing = (await structure.FloorsAsync(regId, userId, projectId, buildingId)).Select(f => f.Code).ToHashSet(StringComparer.OrdinalIgnoreCase);
        return plan.Select(p => new FloorPreviewItem(p.Code, p.Name, p.LevelTypeId, p.SortOrder, p.HeightM, existing.Contains(p.Code))).ToList();
    }

    public Task<GenerateResult> GenerateFloorsAsync(int regId, int userId, int projectId, int buildingId, FloorGenerateRequest r)
        => structure.GenerateFloorsAsync(regId, userId, projectId, buildingId, PlanFloors(r));

    public async Task<UnitPreview> PreviewUnitsAsync(int regId, int userId, int projectId, int buildingId, UnitGenerateRequest r)
    {
        var (pattern, plan) = await PlanUnitsAsync(regId, userId, projectId, buildingId, r);
        return new(pattern, plan.Count(u => !u.AlreadyExists), plan.Count(u => u.AlreadyExists),
                   plan.Select(u => new UnitPreviewItem(u.UnitNo, u.FloorId, u.FloorCode, u.Position, u.UnitTypeId, u.CarpetSqm, u.AlreadyExists)).ToList());
    }

    public async Task<GenerateResult> GenerateUnitsAsync(int regId, int userId, int projectId, int buildingId, UnitGenerateRequest r)
    {
        var (_, plan) = await PlanUnitsAsync(regId, userId, projectId, buildingId, r);
        var toCreate = plan.Where(u => !u.AlreadyExists).ToList();
        if (toCreate.Count == 0) return new GenerateResult { Created = 0, Skipped = plan.Count };
        var result = await units.GenerateAsync(regId, userId, projectId, buildingId, toCreate);
        return result with { Skipped = result.Skipped + (plan.Count - toCreate.Count) };
    }

    private static IReadOnlyList<PlannedFloor> PlanFloors(FloorGenerateRequest r)
    {
        try
        {
            return FloorPlanner.Plan(new FloorPlanOptions(r.Basements, r.BasementLevelTypeId, r.GroundLevelTypeId, r.Podiums, r.PodiumLevelTypeId,
                r.TypicalFrom, r.TypicalTo, r.TypicalLevelTypeId, FloorPlanner.ParseSkipList(r.SkipNumbers), r.TerraceLevelTypeId, r.HeightM));
        }
        catch (DomainException e) { throw AppException.BadRequest(e.Message); }
    }

    private async Task<(string Pattern, IReadOnlyList<PlannedUnit> Plan)> PlanUnitsAsync(int regId, int userId, int projectId, int buildingId, UnitGenerateRequest r)
    {
        var project = await projects.GetAsync(regId, userId, projectId) ?? throw AppException.NotFound("Project not found.");
        var tree = await structure.GetTreeAsync(regId, userId, projectId);
        var building = tree.Buildings.FirstOrDefault(b => b.Id == buildingId) ?? throw AppException.NotFound("Building not found.");
        var phaseCode = tree.Phases.FirstOrDefault(p => p.Id == building.PhaseId)?.Code;
        var pattern = string.IsNullOrWhiteSpace(building.UnitNumberPattern) ? project.UnitNumberPattern : building.UnitNumberPattern!;
        var positions = (r.Positions ?? []).Select(p => new UnitPosition(p.Position, p.UnitTypeId, Clean(p.Facing), p.CarpetSqm, p.BuiltUpSqm,
                                                                          p.SuperSqm, p.BalconySqm, p.PlotSqm)).ToList();
        var existing = await units.ExistingNumbersAsync(regId, userId, projectId);
        try
        {
            if (building.BuildingType is "Villa cluster" or "Plot sector")
            {
                if (pattern.Contains("{FF}")) pattern = pattern.Replace("{FF}", "");
                return (pattern, UnitPlanner.PlanWithoutFloors(pattern, phaseCode, building.Code, positions, existing));
            }
            if (r.FromFloorId is not int from || r.ToFloorId is not int to) throw AppException.BadRequest("Choose the first and last floor.");
            var floors = (await structure.FloorsAsync(regId, userId, projectId, buildingId))
                .Select(f => new FloorSlot(f.Id, f.Code, f.SortOrder, f.IsHabitable)).ToList();
            return (pattern, UnitPlanner.PlanOnFloors(pattern, phaseCode, building.Code, floors, from, to, positions, existing));
        }
        catch (DomainException e) { throw AppException.BadRequest(e.Message); }
    }

    private static string? Clean(string? s) => string.IsNullOrWhiteSpace(s) ? null : s.Trim();
}

public sealed class LandService(ILandRepository land)
{
    public Task<int> SaveAsync(int regId, int userId, int? id, LandParcelSaveRequest r)
    {
        if (r.AreaEntered <= 0) throw AppException.BadRequest("Enter an area greater than zero.");
        decimal sqm;
        try { sqm = AreaUnits.ToSquareMetres(r.AreaEntered, r.AreaUnitEntered); }
        catch (DomainException e) { throw AppException.BadRequest(e.Message); }
        var owners = (r.Owners ?? []).Where(o => !string.IsNullOrWhiteSpace(o.OwnerName)).ToList();
        if (owners.Count > 0 && owners.Sum(o => o.SharePct) != 100m) throw AppException.BadRequest("Owner shares must total 100 percent.");
        return land.SaveAsync(regId, userId, id, r with { Owners = owners }, sqm);
    }
}
