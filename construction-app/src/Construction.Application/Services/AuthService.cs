using System.Security.Cryptography;
using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Contracts;

namespace Construction.Application.Services;

public sealed record AuthOptions
{
    public int MaxFailedAttempts { get; init; } = 5;
    public int LockoutMinutes { get; init; } = 15;
    public int AccessTokenMinutes { get; init; } = 15;
    public int RefreshTokenDays { get; init; } = 7;
}

public interface IAccessTokenIssuer
{
    (string Token, DateTime ExpiresAtUtc) Issue(SessionInfo session);
}

public sealed record SignInOutcome(SignInResult Result, string RefreshToken, DateTime RefreshExpiresAtUtc);

/// <summary>
/// Signs people in with their ERP username and password (tbl_employee). The password check happens inside
/// the database procedure; this app never stores or returns passwords and never writes to tbl_employee.
/// </summary>
public sealed class AuthService(IAuthRepository repo, IAccessTokenIssuer issuer, AuthOptions options, TimeProvider clock)
{
    private const string GenericFailure = "The username or password is incorrect.";

    public async Task<SignInOutcome> SignInAsync(SignInRequest r, string? ip)
    {
        if (string.IsNullOrWhiteSpace(r.UserName) || string.IsNullOrEmpty(r.Password)) throw AppException.BadRequest("Enter your username and password.");
        if (r.UserName.Length > 200 || r.Password.Length > 200) throw new AppException(401, GenericFailure);
        var check = await repo.SignInAsync(r.UserName.Trim(), r.Password, ip, options.MaxFailedAttempts, options.LockoutMinutes);
        var now = clock.GetUtcNow().UtcDateTime;
        switch (check.Result)
        {
            case "Ok": break;
            case "Locked":
                var minutes = check.LockoutEndUtc is DateTime until ? Math.Max(1, (int)Math.Ceiling((until - now).TotalMinutes)) : options.LockoutMinutes;
                throw new AppException(423, $"Too many failed attempts. Try again after {minutes} minute(s).");
            case "ErpLocked":
                throw new AppException(403, "Your ERP login is locked. Ask your administrator to unlock it in the ERP.");
            case "Ambiguous":
                throw new AppException(409, "This username belongs to more than one active person. Ask your administrator to make it unique in the ERP.");
            default:
                throw new AppException(401, GenericFailure);
        }
        var session = await repo.GetSessionAsync(check.UserId!.Value) ?? throw new AppException(401, GenericFailure);
        if (!session.IsActive) throw new AppException(403, "This account is inactive. Contact your administrator.");
        var refresh = NewRefreshToken();
        var refreshExpires = now.AddDays(options.RefreshTokenDays);
        await repo.CreateRefreshTokenAsync(session.UserId, Hash(refresh), refreshExpires);
        return new(ToResult(session), refresh, refreshExpires);
    }

    public async Task<SignInOutcome> RefreshAsync(string? refreshToken)
    {
        if (string.IsNullOrEmpty(refreshToken)) throw new AppException(401, "Please sign in.");
        var next = NewRefreshToken();
        var expires = clock.GetUtcNow().UtcDateTime.AddDays(options.RefreshTokenDays);
        var userId = await repo.RotateRefreshTokenAsync(Hash(refreshToken), Hash(next), expires)
                     ?? throw new AppException(401, "Your session has ended. Please sign in again.");
        var session = await repo.GetSessionAsync(userId);
        if (session is null || !session.IsActive)
        {
            await repo.RevokeRefreshTokenAsync(Hash(next));
            throw new AppException(401, "Your session has ended. Please sign in again.");
        }
        return new(ToResult(session), next, expires);
    }

    public Task SignOutAsync(string? refreshToken) => string.IsNullOrEmpty(refreshToken) ? Task.CompletedTask : repo.RevokeRefreshTokenAsync(Hash(refreshToken));

    public async Task<SessionUser> MeAsync(int userId)
    {
        var s = await repo.GetSessionAsync(userId) ?? throw new AppException(401, "Please sign in.");
        return ToUser(s);
    }

    private SignInResult ToResult(SessionInfo s)
    {
        var (token, expires) = issuer.Issue(s);
        return new(token, expires, ToUser(s));
    }

    private static SessionUser ToUser(SessionInfo s) => new(s.UserId, s.RegId, s.UserName, s.FullName, s.Email, s.TenantName, s.ProfileName, s.Permissions);

    private static string NewRefreshToken() => Convert.ToBase64String(RandomNumberGenerator.GetBytes(32)).TrimEnd('=').Replace('+', '-').Replace('/', '_');

    public static byte[] Hash(string token) => SHA256.HashData(System.Text.Encoding.UTF8.GetBytes(token));
}
