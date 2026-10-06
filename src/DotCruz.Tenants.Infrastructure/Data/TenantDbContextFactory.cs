using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotCruz.Tenants.Infrastructure.Data;

public class TenantDbContextFactory : IDesignTimeDbContextFactory<TenantDbContext>
{
    public TenantDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<TenantDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tenants;Username=dotcruz;Password=mudar123");
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new TenantDbContext(optionsBuilder.Options);
    }
}
