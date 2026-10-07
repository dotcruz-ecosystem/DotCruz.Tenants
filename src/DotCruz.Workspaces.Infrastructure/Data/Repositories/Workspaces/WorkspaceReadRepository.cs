using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Domain.Entities.Workspaces;
using DotCruz.Workspaces.Domain.Enums.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;
using DotCruz.Workspaces.Infrastructure.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace DotCruz.Workspaces.Infrastructure.Data.Repositories.Workspaces;

public class WorkspaceReadRepository(WorkspaceDbContext context) : BaseReadRepository<Workspace>(context), IWorkspaceReadRepository
{
    public async Task<bool> ExistsWithSlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(t => t.Slug == WorkspaceSlug.Create(slug), cancellationToken);
    }

    public async Task<bool> ExistsWithDocumentAsync(string documentNumber, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().AnyAsync(t => t.Document.Number == documentNumber, cancellationToken);
    }

    public async Task<Workspace?> GetBySlugAsync(string slug, CancellationToken cancellationToken)
    {
        return await _dbSet.AsNoTracking().FirstOrDefaultAsync(t => t.Slug == WorkspaceSlug.Create(slug), cancellationToken);
    }

    public async Task<(IReadOnlyCollection<Workspace> Items, int TotalCount)> SearchAsync(
        int pageNumber,
        int pageSize,
        WorkspaceStatus? status,
        PlanType? plan,
        string? searchTerm,
        CancellationToken cancellationToken)
    {
        var query = _dbSet.AsNoTracking();

        if (status.HasValue)
        {
            query = query.Where(t => t.Status == status.Value);
        }

        if (plan.HasValue)
        {
            query = query.Where(t => t.Subscription.Plan == plan.Value);
        }

        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            query = query.Where(t => EF.Property<string>(t, "Name").Contains(searchTerm) || 
                                     EF.Property<string>(t, "Slug").Contains(searchTerm));
        }

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .OrderBy(t => EF.Property<string>(t, "Name"))
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(cancellationToken);

        return (items, totalCount);
    }

    public async Task<IReadOnlyCollection<WorkspaceMember>> GetMembersAsync(Guid WorkspaceId, CancellationToken cancellationToken)
    {
        var members = await _context.WorkspaceMembers
            .AsNoTracking()
            .Where(m => m.WorkspaceId == WorkspaceId)
            .OrderBy(m => m.CreatedAt)
            .ToListAsync(cancellationToken);

        return members.AsReadOnly();
    }

    public async Task<IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)>> GetUserWorkspacesAsync(Guid userId, CancellationToken cancellationToken)
    {
        var list = await _context.WorkspaceMembers
            .AsNoTracking()
            .Where(m => m.UserId == userId && m.Status != MemberStatus.Suspended)
            .Include(m => m.Workspace)
            .OrderBy(m => EF.Property<string>(m.Workspace, "Name"))
            .ToListAsync(cancellationToken);

        return list.Select(m => (m.Workspace, m)).ToList().AsReadOnly();
    }

    public async Task<IReadOnlyCollection<(Workspace Workspace, WorkspaceMember Member)>> GetUserWorkspacesByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var normalizedEmail = email.Trim().ToLowerInvariant();

        var list = await _context.WorkspaceMembers
            .AsNoTracking()
            .Where(m => m.Email == DotCruz.Workspaces.Domain.ValueObjects.Communication.Email.Create(normalizedEmail) && m.Status != MemberStatus.Suspended)
            .Include(m => m.Workspace)
            .OrderBy(m => EF.Property<string>(m.Workspace, "Name"))
            .ToListAsync(cancellationToken);

        return list.Select(m => (m.Workspace, m)).ToList().AsReadOnly();
    }
}
