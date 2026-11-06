using Construction.Api.Security;
using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Contracts;
using Construction.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;
using SignInResult = Construction.Application.Contracts.SignInResult;

namespace Construction.Api.Controllers;

/// <summary>Sign-in with the ERP username and password (tbl_employee). Passwords are changed in the ERP, not here.</summary>
[ApiController]
[Route("api/auth")]
public sealed class AuthController(AuthService auth, ICurrentUser me, IWebHostEnvironment env) : ControllerBase
{
    private const string RefreshCookie = "cpm_rt";

    [HttpPost("sign-in"), AllowAnonymous, EnableRateLimiting("sign-in")]
    public async Task<SignInResult> SignIn(SignInRequest r)
    {
        var outcome = await auth.SignInAsync(r, HttpContext.Connection.RemoteIpAddress?.ToString());
        SetRefreshCookie(outcome.RefreshToken, outcome.RefreshExpiresAtUtc);
        return outcome.Result;
    }

    /// <summary>Exchanges the refresh cookie for a new access token. Needs the X-Requested-With header as a cross-site guard.</summary>
    [HttpPost("refresh"), AllowAnonymous, EnableRateLimiting("refresh")]
    public async Task<SignInResult> Refresh()
    {
        RequireAjaxHeader();
        var outcome = await auth.RefreshAsync(Request.Cookies[RefreshCookie]);
        SetRefreshCookie(outcome.RefreshToken, outcome.RefreshExpiresAtUtc);
        return outcome.Result;
    }

    [HttpPost("sign-out"), AllowAnonymous]
    public async Task<IActionResult> SignOutNow()
    {
        RequireAjaxHeader();
        await auth.SignOutAsync(Request.Cookies[RefreshCookie]);
        Response.Cookies.Delete(RefreshCookie, CookieOptions(DateTime.UnixEpoch));
        return NoContent();
    }

    [HttpGet("me")]
    public Task<SessionUser> Me() => auth.MeAsync(me.UserId);

    private void RequireAjaxHeader()
    {
        if (Request.Headers["X-Requested-With"] != "XMLHttpRequest") throw new AppException(400, "Missing request header.");
    }

    private void SetRefreshCookie(string token, DateTime expiresUtc) => Response.Cookies.Append(RefreshCookie, token, CookieOptions(expiresUtc));

    private CookieOptions CookieOptions(DateTime expiresUtc) => new()
    {
        HttpOnly = true,
        Secure = !env.IsDevelopment() || Request.IsHttps,
        SameSite = SameSiteMode.Strict,
        Path = "/api/auth",
        Expires = expiresUtc
    };
}

/// <summary>People and profiles come from the ERP; this app only sets construction permissions per profile.</summary>
[ApiController]
[Route("api/admin")]
public sealed class AdminController(IPeopleRepository people, IProfileRepository profiles, IAuditRepository audit, ICurrentUser me) : ControllerBase
{
    [HttpGet("people"), HasPermission(Permissions.AdminRoles)]
    public Task<IReadOnlyList<PersonItem>> People(string? search) => people.ListAsync(me.RegId, me.UserId, search);

    [HttpPost("people/{id:int}/unlock"), HasPermission(Permissions.AdminRoles)]
    public async Task<IActionResult> Unlock(int id) { await people.UnlockAsync(me.RegId, me.UserId, id); return NoContent(); }

    [HttpGet("permissions"), HasPermission(Permissions.AdminRoles)]
    public Task<IReadOnlyList<PermissionItem>> AllPermissions() => profiles.PermissionsAsync();

    [HttpGet("profiles"), HasPermission(Permissions.AdminRoles)]
    public Task<IReadOnlyList<ProfileItem>> Profiles() => profiles.ListAsync(me.RegId, me.UserId);

    [HttpPut("profiles/{id:int}/permissions"), HasPermission(Permissions.AdminRoles)]
    public async Task<IActionResult> SaveProfile(int id, ProfilePermissionsRequest r)
    { await profiles.SavePermissionsAsync(me.RegId, me.UserId, id, r.Permissions ?? []); return NoContent(); }

    [HttpGet("audit/{entityType}/{entityId:long}"), HasPermission(Permissions.AuditView)]
    public Task<IReadOnlyList<AuditItem>> Audit(string entityType, long entityId) => audit.ListAsync(me.RegId, me.UserId, entityType, entityId);
}

[ApiController]
[Route("api/masters")]
public sealed class MastersController(IMasterRepository masters, IPeopleRepository people, ICurrentUser me) : ControllerBase
{
    /// <summary>Countries, states and cities from the ERP masters (read-only).</summary>
    [HttpGet("locations")]
    public Task<LocationLists> Locations() => masters.LocationsAsync(me.RegId);

    [HttpGet("unit-types")]
    public Task<IReadOnlyList<UnitTypeItem>> UnitTypes(bool includeInactive = false) => masters.UnitTypesAsync(me.RegId, includeInactive);

    [HttpPost("unit-types"), HasPermission(Permissions.MastersEdit)]
    public async Task<IdResult> CreateUnitType(UnitTypeSaveRequest r) => new(await masters.SaveUnitTypeAsync(me.RegId, me.UserId, null, r));

    [HttpPut("unit-types/{id:int}"), HasPermission(Permissions.MastersEdit)]
    public async Task<IdResult> UpdateUnitType(int id, UnitTypeSaveRequest r) => new(await masters.SaveUnitTypeAsync(me.RegId, me.UserId, id, r));

    [HttpGet("level-types")]
    public Task<IReadOnlyList<LevelTypeItem>> LevelTypes() => masters.LevelTypesAsync(me.RegId);

    [HttpPost("level-types"), HasPermission(Permissions.MastersEdit)]
    public async Task<IdResult> CreateLevelType(LevelTypeSaveRequest r) => new(await masters.SaveLevelTypeAsync(me.RegId, me.UserId, null, r));

    [HttpPut("level-types/{id:int}"), HasPermission(Permissions.MastersEdit)]
    public async Task<IdResult> UpdateLevelType(int id, LevelTypeSaveRequest r) => new(await masters.SaveLevelTypeAsync(me.RegId, me.UserId, id, r));

    /// <summary>Active people of this company (tbl_employee), for team and responsibility pickers.</summary>
    [HttpGet("people")]
    public Task<IReadOnlyList<UserLookup>> People() => people.LookupAsync(me.RegId);
}
