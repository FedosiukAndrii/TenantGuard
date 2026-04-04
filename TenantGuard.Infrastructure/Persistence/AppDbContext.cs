using Microsoft.EntityFrameworkCore;
using TenantGuard.Application.Context;
using TenantGuard.Domain.Entities;

namespace TenantGuard.Infrastructure.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options, ITenantContext tenantContext): DbContext(options)
{
    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<Project> Projects => Set<Project>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);

        // Global query filter: returns only current-tenant rows.
        // If TenantId is null (no tenant context) the filter produces no matches,
        // which is safe – writes are blocked earlier by ApplyTenantIsolation.
        modelBuilder.Entity<Project>()
            .HasQueryFilter(p => p.TenantId == tenantContext.TenantId);

        base.OnModelCreating(modelBuilder);
    }

    public override int SaveChanges()
    {
        ApplyTenantIsolation();
        return base.SaveChanges();
    }

    public override int SaveChanges(bool acceptAllChangesOnSuccess)
    {
        ApplyTenantIsolation();
        return base.SaveChanges(acceptAllChangesOnSuccess);
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ApplyTenantIsolation();
        return base.SaveChangesAsync(cancellationToken);
    }

    // Enforces tenant boundaries before every write:
    //   1. Fails fast if a tenant context has not been established.
    //   2. Auto-assigns TenantId for new entities created without one.
    //   3. Rejects cross-tenant writes where entity TenantId differs from context.
    private void ApplyTenantIsolation()
    {
        var tenantScopedEntries = ChangeTracker
            .Entries<ITenantScoped>()
            .Where(e => e.State is EntityState.Added or EntityState.Modified or EntityState.Deleted)
            .ToList();

        if (tenantScopedEntries.Count == 0)
            return;

        if (tenantContext.TenantId is null)
            throw new InvalidOperationException(
                "A tenant context is required for tenant-scoped persistence operations.");

        var contextTenantId = tenantContext.TenantId.Value;

        foreach (var entry in tenantScopedEntries)
        {
            if (entry.State == EntityState.Added && entry.Entity.TenantId == Guid.Empty)
            {
                entry.Property(nameof(ITenantScoped.TenantId)).CurrentValue = contextTenantId;
                continue;
            }

            if (entry.Entity.TenantId != contextTenantId)
                throw new InvalidOperationException(
                    $"Cross-tenant write detected. Entity TenantId '{entry.Entity.TenantId}' " +
                    $"does not match context TenantId '{contextTenantId}'.");
        }
    }
}
