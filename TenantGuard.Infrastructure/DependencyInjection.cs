using Microsoft.Extensions.DependencyInjection;
using TenantGuard.Application.Context;
using TenantGuard.Infrastructure.Context;

namespace TenantGuard.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<TenantContext>();
        services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<TenantContext>());

        return services;
    }
}
