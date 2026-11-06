using Construction.Api.Security;
using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Contracts;
using Construction.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace Construction.Api.Controllers;

[ApiController]
[Route("api/land")]
public sealed class LandController(ILandRepository land, LandService service, ICurrentUser me) : ControllerBase
{
    [HttpGet, HasPermission(Permissions.LandView)]
    public Task<IReadOnlyList<LandParcelListItem>> List(string? search) => land.ListAsync(me.RegId, me.UserId, search);

    [HttpGet("{id:int}"), HasPermission(Permissions.LandView)]
    public async Task<LandParcelDetail> Get(int id) => await land.GetAsync(me.RegId, me.UserId, id) ?? throw AppException.NotFound("Land parcel not found.");

    [HttpPost, HasPermission(Permissions.LandEdit)]
    public async Task<IdResult> Create(LandParcelSaveRequest r) => new(await service.SaveAsync(me.RegId, me.UserId, null, r));

    [HttpPut("{id:int}"), HasPermission(Permissions.LandEdit)]
    public async Task<IdResult> Update(int id, LandParcelSaveRequest r) => new(await service.SaveAsync(me.RegId, me.UserId, id, r));

    [HttpPost("{id:int}/delete"), HasPermission(Permissions.LandEdit)]
    public async Task<IActionResult> Delete(int id, DeleteRequest r) { await land.DeleteAsync(me.RegId, me.UserId, id, r.Reason); return NoContent(); }
}

[ApiController]
[Route("api/projects")]
public sealed class ProjectsController(IProjectRepository projects, ICurrentUser me) : ControllerBase
{
    [HttpGet, HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public Task<IReadOnlyList<ProjectListItem>> List() => projects.ListAsync(me.RegId, me.UserId);

    [HttpGet("{id:int}"), HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public async Task<ProjectDetail> Get(int id) => await projects.GetAsync(me.RegId, me.UserId, id) ?? throw AppException.NotFound("Project not found.");

    [HttpPost, HasPermission(Permissions.ProjectsEdit)]
    public async Task<IdResult> Create(ProjectSaveRequest r) => new(await projects.SaveAsync(me.RegId, me.UserId, null, r));

    [HttpPut("{id:int}"), HasPermission(Permissions.ProjectsEdit)]
    public async Task<IdResult> Update(int id, ProjectSaveRequest r) => new(await projects.SaveAsync(me.RegId, me.UserId, id, r));

    [HttpPost("{id:int}/delete"), HasPermission(Permissions.ProjectsEdit)]
    public async Task<IActionResult> Delete(int id, DeleteRequest r) { await projects.DeleteAsync(me.RegId, me.UserId, id, r.Reason); return NoContent(); }
}

[ApiController]
[Route("api/projects/{projectId:int}")]
public sealed class StructureController(IStructureRepository structure, StructureService service, ICurrentUser me) : ControllerBase
{
    [HttpGet("structure"), HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public Task<StructureTree> Tree(int projectId) => structure.GetTreeAsync(me.RegId, me.UserId, projectId);

    [HttpPost("phases"), HasPermission(Permissions.StructureEdit)]
    public async Task<IdResult> CreatePhase(int projectId, PhaseSaveRequest r) => new(await structure.SavePhaseAsync(me.RegId, me.UserId, projectId, null, r));

    [HttpPut("phases/{id:int}"), HasPermission(Permissions.StructureEdit)]
    public async Task<IdResult> UpdatePhase(int projectId, int id, PhaseSaveRequest r) => new(await structure.SavePhaseAsync(me.RegId, me.UserId, projectId, id, r));

    [HttpPost("phases/{id:int}/delete"), HasPermission(Permissions.StructureEdit)]
    public async Task<IActionResult> DeletePhase(int projectId, int id, DeleteRequest r)
    { await structure.DeletePhaseAsync(me.RegId, me.UserId, projectId, id, r.Reason); return NoContent(); }

    [HttpPost("buildings"), HasPermission(Permissions.StructureEdit)]
    public async Task<IdResult> CreateBuilding(int projectId, BuildingSaveRequest r) => new(await structure.SaveBuildingAsync(me.RegId, me.UserId, projectId, null, r));

    [HttpPut("buildings/{id:int}"), HasPermission(Permissions.StructureEdit)]
    public async Task<IdResult> UpdateBuilding(int projectId, int id, BuildingSaveRequest r) => new(await structure.SaveBuildingAsync(me.RegId, me.UserId, projectId, id, r));

    [HttpPost("buildings/{id:int}/delete"), HasPermission(Permissions.StructureEdit)]
    public async Task<IActionResult> DeleteBuilding(int projectId, int id, DeleteRequest r)
    { await structure.DeleteBuildingAsync(me.RegId, me.UserId, projectId, id, r.Reason); return NoContent(); }

    [HttpGet("buildings/{buildingId:int}/floors"), HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public Task<IReadOnlyList<FloorItem>> Floors(int projectId, int buildingId) => structure.FloorsAsync(me.RegId, me.UserId, projectId, buildingId);

    [HttpPost("buildings/{buildingId:int}/floors/preview"), HasPermission(Permissions.UnitsGenerate)]
    public Task<IReadOnlyList<FloorPreviewItem>> PreviewFloors(int projectId, int buildingId, FloorGenerateRequest r)
        => service.PreviewFloorsAsync(me.RegId, me.UserId, projectId, buildingId, r);

    [HttpPost("buildings/{buildingId:int}/floors/generate"), HasPermission(Permissions.UnitsGenerate)]
    public Task<GenerateResult> GenerateFloors(int projectId, int buildingId, FloorGenerateRequest r)
        => service.GenerateFloorsAsync(me.RegId, me.UserId, projectId, buildingId, r);

    [HttpPut("floors/{id:int}"), HasPermission(Permissions.StructureEdit)]
    public async Task<IdResult> UpdateFloor(int projectId, int id, FloorSaveRequest r) => new(await structure.SaveFloorAsync(me.RegId, me.UserId, projectId, id, r));

    [HttpPost("floors/{id:int}/delete"), HasPermission(Permissions.StructureEdit)]
    public async Task<IActionResult> DeleteFloor(int projectId, int id, DeleteRequest r)
    { await structure.DeleteFloorAsync(me.RegId, me.UserId, projectId, id, r.Reason); return NoContent(); }
}

[ApiController]
[Route("api/projects/{projectId:int}")]
public sealed class UnitsController(IUnitRepository units, StructureService service, ICurrentUser me) : ControllerBase
{
    [HttpGet("units"), HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public Task<PagedResult<UnitListItem>> List(int projectId, [FromQuery] UnitQuery q) => units.ListAsync(me.RegId, me.UserId, projectId, q);

    [HttpGet("buildings/{buildingId:int}/stack"), HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public Task<StackingPlan> Stack(int projectId, int buildingId) => units.StackAsync(me.RegId, me.UserId, projectId, buildingId);

    [HttpPost("buildings/{buildingId:int}/units/preview"), HasPermission(Permissions.UnitsGenerate)]
    public Task<UnitPreview> Preview(int projectId, int buildingId, UnitGenerateRequest r) => service.PreviewUnitsAsync(me.RegId, me.UserId, projectId, buildingId, r);

    [HttpPost("buildings/{buildingId:int}/units/generate"), HasPermission(Permissions.UnitsGenerate)]
    public Task<GenerateResult> Generate(int projectId, int buildingId, UnitGenerateRequest r) => service.GenerateUnitsAsync(me.RegId, me.UserId, projectId, buildingId, r);

    [HttpGet("units/{id:int}"), HasPermission(Permissions.ProjectsView + "|" + Permissions.ProjectsAll)]
    public async Task<UnitDetail> Get(int projectId, int id) => await units.GetAsync(me.RegId, me.UserId, projectId, id) ?? throw AppException.NotFound("Unit not found.");

    [HttpPut("units/{id:int}"), HasPermission(Permissions.UnitsEdit)]
    public async Task<IdResult> Update(int projectId, int id, UnitSaveRequest r) => new(await units.SaveAsync(me.RegId, me.UserId, projectId, id, r));

    [HttpPost("units/{id:int}/availability"), HasPermission(Permissions.AvailabilityChange)]
    public async Task<IActionResult> ChangeAvailability(int projectId, int id, AvailabilityChangeRequest r)
    { await units.ChangeAvailabilityAsync(me.RegId, me.UserId, projectId, id, r); return NoContent(); }

    [HttpPost("units/delete"), HasPermission(Permissions.UnitsEdit)]
    public async Task<object> Delete(int projectId, UnitDeleteRequest r) => new { deleted = await units.DeleteAsync(me.RegId, me.UserId, projectId, r) };
}
