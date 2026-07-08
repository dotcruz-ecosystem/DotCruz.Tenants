namespace DotCruz.Tenants.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
