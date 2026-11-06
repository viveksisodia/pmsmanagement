using Construction.Application.Abstractions;
using Construction.Application.Services;
using Construction.Infrastructure.Data;
using Microsoft.Extensions.DependencyInjection;

namespace Construction.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddConstructionInfrastructure(this IServiceCollection services, DbOptions dbOptions)
    {
        services.AddSingleton(dbOptions);
        services.AddSingleton<Db>();
        services.AddScoped<IAuthRepository, AuthRepository>();
        services.AddScoped<IPeopleRepository, PeopleRepository>();
        services.AddScoped<IProfileRepository, ProfileRepository>();
        services.AddScoped<IAuditRepository, AuditRepository>();
        services.AddScoped<IMasterRepository, MasterRepository>();
        services.AddScoped<ILandRepository, LandRepository>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IStructureRepository, StructureRepository>();
        services.AddScoped<IUnitRepository, UnitRepository>();
        services.AddScoped<AuthService>();
        services.AddScoped<StructureService>();
        services.AddScoped<LandService>();
        services.AddSingleton(TimeProvider.System);
        return services;
    }
}
