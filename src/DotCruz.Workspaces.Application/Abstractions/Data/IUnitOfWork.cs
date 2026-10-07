namespace DotCruz.Workspaces.Application.Abstractions.Data;

public interface IUnitOfWork
{
    Task CommitAsync(CancellationToken cancellationToken);
}
