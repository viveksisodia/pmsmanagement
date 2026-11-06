using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Construction.Api.Security;

public sealed class JwtOptions
{
    public string Issuer { get; set; } = "construction-app";
    public string Audience { get; set; } = "construction-app";
    /// <summary>At least 32 characters. Set through user secrets or an environment variable, never in appsettings.json.</summary>
    public string SigningKey { get; set; } = "";
    public SymmetricSecurityKey Key() => new(Encoding.UTF8.GetBytes(SigningKey));
}

public static class ClaimNames
{
    public const string RegId = "regid";
    public const string Permission = "perm";
}

public sealed class JwtIssuer(JwtOptions jwt, AuthOptions auth, TimeProvider clock) : IAccessTokenIssuer
{
    public (string Token, DateTime ExpiresAtUtc) Issue(SessionInfo s)
    {
        var now = clock.GetUtcNow().UtcDateTime;
        var expires = now.AddMinutes(auth.AccessTokenMinutes);
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, s.UserId.ToString()),
            new(ClaimNames.RegId, s.RegId.ToString()),
            new("username", s.UserName),
            new("name", s.FullName),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N"))
        };
        claims.AddRange(s.Permissions.Select(p => new Claim(ClaimNames.Permission, p)));
        var token = new JwtSecurityToken(jwt.Issuer, jwt.Audience, claims, now, expires, new SigningCredentials(jwt.Key(), SecurityAlgorithms.HmacSha256));
        return (new JwtSecurityTokenHandler().WriteToken(token), expires);
    }
}

public sealed class HttpCurrentUser(IHttpContextAccessor accessor) : ICurrentUser
{
    private ClaimsPrincipal User => accessor.HttpContext?.User ?? throw new AppException(401, "Please sign in.");

    public int UserId => int.TryParse(User.FindFirstValue(JwtRegisteredClaimNames.Sub) ?? User.FindFirstValue(ClaimTypes.NameIdentifier), out var id)
        ? id : throw new AppException(401, "Please sign in.");

    public int RegId => int.TryParse(User.FindFirstValue(ClaimNames.RegId), out var id) ? id : throw new AppException(401, "Please sign in.");

    public bool HasPermission(string code) => User.HasClaim(ClaimNames.Permission, code);
}

/// <summary>[HasPermission("land.edit")] → policy "perm:land.edit". The stored procedures check the same rights again.</summary>
public sealed class HasPermissionAttribute(string code) : AuthorizeAttribute($"perm:{code}");

public sealed class PermissionPolicyProvider(IOptions<AuthorizationOptions> options) : DefaultAuthorizationPolicyProvider(options)
{
    public override async Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
    {
        if (policyName.StartsWith("perm:", StringComparison.Ordinal))
        {
            var codes = policyName[5..].Split('|');
            return new AuthorizationPolicyBuilder().RequireAuthenticatedUser()
                .RequireAssertion(c => codes.Any(code => c.User.HasClaim(ClaimNames.Permission, code))).Build();
        }
        return await base.GetPolicyAsync(policyName);
    }
}
