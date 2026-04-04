using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using TenantGuard.Application.Context;
// ReSharper disable once UnusedMember.Global

namespace TenantGuard.Infrastructure.Persistence;

/// <summary>
/// Provides a design-time AppDbContext instance for EF Core tooling (migrations).
/// Uses a no-op tenant context because migrations do not run in a request scope.
/// </summary>
public sealed class AppDbContextDesignTimeFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=TenantGuardDb;Trusted_Connection=True;TrustServerCertificate=True")
            .Options;

        return new AppDbContext(options, NoOpTenantContext.Instance);
    }
}

/// <summary>
/// A no-op ITenantContext used only during design-time tooling.
/// Never resolves a real tenant; throws if a write is attempted.
/// </summary>
internal sealed class NoOpTenantContext : ITenantContext
{
    public static readonly NoOpTenantContext Instance = new();

    public Guid? TenantId => null;
    public string? ExternalUserId => null;

    public void SetTenant(Guid tenantId) =>
        throw new InvalidOperationException("Cannot set tenant on the design-time no-op context.");

    public void SetUser(string externalUserId) =>
        throw new InvalidOperationException("Cannot set user on the design-time no-op context.");
}
