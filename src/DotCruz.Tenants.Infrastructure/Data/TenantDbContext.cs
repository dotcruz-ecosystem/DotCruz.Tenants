using DotCruz.Tenants.Domain.Entities.Base;
using DotCruz.Tenants.Domain.Entities.Tenants;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DotCruz.Tenants.Infrastructure.Data;

public class TenantDbContext(DbContextOptions<TenantDbContext> options) : DbContext(options)
{
    public DbSet<Tenant> Tenants { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasPostgresExtension("citext");
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(TenantDbContext).Assembly);

        foreach (var entityType in modelBuilder.Model.GetEntityTypes()
                    .Where(e => e.ClrType != null))
        {
            var parameter = Expression.Parameter(entityType.ClrType, "e");
            Expression? combinedExpr = null;

            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                var propertyDeletedAt = Expression.Property(parameter, nameof(BaseEntity.DeletedAt));
                var nullValue = Expression.Constant(null, typeof(DateTimeOffset?));
                combinedExpr = Expression.Equal(propertyDeletedAt, nullValue);
            }

            if (combinedExpr != null)
            {
                var lambda = Expression.Lambda(combinedExpr, parameter);
                modelBuilder.Entity(entityType.ClrType).HasQueryFilter(lambda);
            }
        }
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries())
        {
            if (entry.Entity is BaseEntity baseEntity && entry.State == EntityState.Modified)
            {
                baseEntity.Touch();
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}
