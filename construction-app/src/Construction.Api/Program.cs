using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Construction.Api;
using Construction.Api.Security;
using Construction.Application.Abstractions;
using Construction.Application.Common;
using Construction.Application.Services;
using Construction.Infrastructure;
using Construction.Infrastructure.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

var dbOptions = builder.Configuration.GetSection("ConstructionDb").Get<DbOptions>() ?? new DbOptions();
var jwt = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
var auth = builder.Configuration.GetSection("Auth").Get<AuthOptions>() ?? new AuthOptions();

// Command line: set up construction masters for an existing company, then exit.
if (args.Length > 0 && args[0] == "setup-company")
{
    return await Cli.SetupCompanyAsync(args, dbOptions);
}

if (jwt.SigningKey.Length < 32)
    throw new InvalidOperationException("Jwt:SigningKey must be at least 32 characters. Set it with user secrets or the Jwt__SigningKey environment variable.");

builder.Services.AddConstructionInfrastructure(dbOptions);
builder.Services.AddSingleton(jwt);
builder.Services.AddSingleton(auth);
builder.Services.AddSingleton<IAccessTokenIssuer, JwtIssuer>();
builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ICurrentUser, HttpCurrentUser>();
builder.Services.AddSingleton<IAuthorizationPolicyProvider, PermissionPolicyProvider>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(o =>
{
    o.MapInboundClaims = false;
    o.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = jwt.Issuer, ValidAudience = jwt.Audience, IssuerSigningKey = jwt.Key(),
        ValidateIssuer = true, ValidateAudience = true, ValidateIssuerSigningKey = true, ValidateLifetime = true,
        ClockSkew = TimeSpan.FromSeconds(30), NameClaimType = "name"
    };
});
builder.Services.AddAuthorization(o => o.FallbackPolicy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build());

builder.Services.AddRateLimiter(o =>
{
    o.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    o.AddPolicy("sign-in", ctx => RateLimitPartition.GetFixedWindowLimiter(ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 10, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
    o.AddPolicy("refresh", ctx => RateLimitPartition.GetFixedWindowLimiter(ctx.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions { PermitLimit = 30, Window = TimeSpan.FromMinutes(1), QueueLimit = 0 }));
});

builder.Services.AddControllers().AddJsonOptions(o => o.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never);
builder.Services.AddProblemDetails();
builder.Services.AddOpenApi();

var app = builder.Build();

// Errors: business rules → their status and message; anything else → 500 without internals.
app.UseExceptionHandler(handler => handler.Run(async ctx =>
{
    var error = ctx.Features.Get<IExceptionHandlerFeature>()?.Error;
    var (status, message) = error switch
    {
        AppException a => (a.Status, a.Message),
        BadHttpRequestException b => (400, "The request couldn't be read."),
        _ => (500, "Something went wrong on the server. Try again, and contact support if it keeps happening.")
    };
    if (status == 500) app.Logger.LogError(error, "Unhandled error on {Path}", ctx.Request.Path);
    ctx.Response.StatusCode = status;
    await ctx.Response.WriteAsJsonAsync(new { status, message });
}));

app.Use(async (ctx, next) =>
{
    var h = ctx.Response.Headers;
    h.XContentTypeOptions = "nosniff";
    h.XFrameOptions = "DENY";
    h["Referrer-Policy"] = "no-referrer";
    h.CacheControl = "no-store";
    await next();
});

if (app.Environment.IsDevelopment()) app.MapOpenApi().AllowAnonymous();
else app.UseHsts();

app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
return 0;
