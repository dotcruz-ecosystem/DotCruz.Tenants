using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace DotCruz.Workspaces.Infrastructure.Data;

public class WorkspaceDbContextFactory : IDesignTimeDbContextFactory<WorkspaceDbContext>
{
    public WorkspaceDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<WorkspaceDbContext>();
        optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=tenants;Username=dotcruz;Password=mudar123");
        optionsBuilder.UseSnakeCaseNamingConvention();

        return new WorkspaceDbContext(optionsBuilder.Options);
    }
}
