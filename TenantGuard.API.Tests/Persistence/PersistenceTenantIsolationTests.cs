using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using TenantGuard.API.Tests.Testing;
using TenantGuard.Domain.Entities;
using TenantGuard.Infrastructure.Persistence;
using Xunit;

namespace TenantGuard.API.Tests.Persistence;

/// <summary>
/// Integration tests that verify AppDbContext enforces tenant isolation
/// through global query filters and save guards.
/// Uses SQLite in-memory to avoid an external database dependency.
/// </summary>
public sealed class PersistenceTenantIsolationTests : IAsyncLifetime
{
    private readonly SqliteConnection _connection;
    private readonly FakeTenantContext _tenantContext;

    public PersistenceTenantIsolationTests()
    {
        _tenantContext = new FakeTenantContext();

        // Keep the connection open so the in-memory database persists across operations.
        _connection = new SqliteConnection("DataSource=:memory:");
    }

    public async Task InitializeAsync()
    {
        await _connection.OpenAsync();

        await using var ctx = CreateContext();
        await ctx.Database.EnsureCreatedAsync();
    }

    public async Task DisposeAsync()
    {
        await _connection.DisposeAsync();
    }

    // ------------------------------------------------------------------ //
    // Query filter tests
    // ------------------------------------------------------------------ //

    [Fact]
    public async Task QueryFilter_ReturnsOnlyCurrentTenantProjects()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        await SeedProjectsDirectlyAsync(
            (Guid.NewGuid(), tenantA, "Project A"),
            (Guid.NewGuid(), tenantB, "Project B"));

        _tenantContext.SetTenant(tenantA);

        await using var ctx = CreateContext();
        var results = await ctx.Projects.ToListAsync();

        Assert.Single(results);
        Assert.All(results, p => Assert.Equal(tenantA, p.TenantId));
    }

    [Fact]
    public async Task QueryFilter_ReturnsNoProjects_WhenTenantContextAbsent()
    {
        var tenantId = Guid.NewGuid();
        await SeedProjectsDirectlyAsync((Guid.NewGuid(), tenantId, "Project X"));

        // No SetTenant call – TenantId remains null.
        await using var ctx = CreateContext();
        var results = await ctx.Projects.ToListAsync();

        Assert.Empty(results);
    }

    // ------------------------------------------------------------------ //
    // Save guard tests
    // ------------------------------------------------------------------ //

    [Fact]
    public async Task SaveChangesAsync_ThrowsInvalidOperation_WhenTenantContextAbsent()
    {
        // TenantId not set – any write to a tenant-scoped entity must fail.
        await using var ctx = CreateContext();
        ctx.Projects.Add(new Project(Guid.NewGuid(), Guid.NewGuid(), "Orphan"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => ctx.SaveChangesAsync());
    }

    [Fact]
    public async Task SaveChangesAsync_AutoAssignsTenantId_WhenNewEntityHasEmptyTenantId()
    {
        var tenantId = Guid.NewGuid();
        _tenantContext.SetTenant(tenantId);

        await using var ctx = CreateContext();

        // Create a project with Guid.Empty – the save guard should fill it in.
        var project = new Project(Guid.NewGuid(), Guid.Empty, "Auto-assigned");
        ctx.Projects.Add(project);
        await ctx.SaveChangesAsync();

        Assert.Equal(tenantId, project.TenantId);
    }

    [Fact]
    public async Task SaveChangesAsync_ThrowsInvalidOperation_OnCrossTenantWrite()
    {
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        _tenantContext.SetTenant(tenantA);

        await using var ctx = CreateContext();

        // Deliberately create a project bearing tenantB's id while the
        // context is scoped to tenantA – the guard must reject this.
        ctx.Projects.Add(new Project(Guid.NewGuid(), tenantB, "Wrong tenant"));

        await Assert.ThrowsAsync<InvalidOperationException>(() => ctx.SaveChangesAsync());
    }

    [Fact]
    public async Task SaveChangesAsync_Succeeds_WhenEntityTenantMatchesContext()
    {
        var tenantId = Guid.NewGuid();
        _tenantContext.SetTenant(tenantId);

        await using var ctx = CreateContext();
        ctx.Projects.Add(new Project(Guid.NewGuid(), tenantId, "Valid project"));
        var affected = await ctx.SaveChangesAsync();

        Assert.Equal(1, affected);
    }

    // ------------------------------------------------------------------ //
    // Cross-tenant leakage test
    // ------------------------------------------------------------------ //

    [Fact]
    public async Task IgnoreQueryFilters_IsNotAffectedByTenantFilter_CanBeExplicitlyBypassed()
    {
        // This test confirms the filter works; callers can bypass via IgnoreQueryFilters
        // (host-level administrative operations), so we document that behaviour here.
        var tenantA = Guid.NewGuid();
        var tenantB = Guid.NewGuid();

        await SeedProjectsDirectlyAsync(
            (Guid.NewGuid(), tenantA, "A"),
            (Guid.NewGuid(), tenantB, "B"));

        _tenantContext.SetTenant(tenantA);

        await using var ctx = CreateContext();
        var allProjects = await ctx.Projects.IgnoreQueryFilters().ToListAsync();

        Assert.Equal(2, allProjects.Count);
    }

    // ------------------------------------------------------------------ //
    // Helpers
    // ------------------------------------------------------------------ //

    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        return new AppDbContext(options, _tenantContext);
    }

    /// <summary>
    /// Inserts rows directly via raw SQL to bypass EF query filters and save guards,
    /// allowing test setup that spans multiple tenants.
    /// </summary>
    private async Task SeedProjectsDirectlyAsync(params (Guid Id, Guid TenantId, string Name)[] projects)
    {
        await using var ctx = CreateContext();
        foreach (var (id, tenantId, name) in projects)
        {
            await ctx.Database.ExecuteSqlRawAsync(
                "INSERT INTO Projects (Id, TenantId, Name) VALUES ({0}, {1}, {2})",
                id, tenantId, name);
        }
    }
}
