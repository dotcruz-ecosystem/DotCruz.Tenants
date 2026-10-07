using DotCruz.Shared.Security.Context;
using DotCruz.Workspaces.Application.Abstractions.Security;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Workspaces.Application.Behaviors;

public class WorkspaceOwnershipBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const string SuperAdminRole = "SuperAdmin";

    private readonly ISecurityContext _securityContext;

    public WorkspaceOwnershipBehavior(ISecurityContext securityContext)
    {
        _securityContext = securityContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is IWorkspaceScopedRequest scopedRequest && !IsAllowed(scopedRequest))
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        return await next(cancellationToken);
    }

    private bool IsAllowed(IWorkspaceScopedRequest request)
    {
        if (_securityContext.IsAuthenticatedService)
            return true;

        if (_securityContext.Roles.Contains(SuperAdminRole, StringComparer.OrdinalIgnoreCase))
            return true;

        var callerWorkspaceId = _securityContext.WorkspaceId;

        return callerWorkspaceId.HasValue
            && callerWorkspaceId.Value != Guid.Empty
            && callerWorkspaceId.Value == request.WorkspaceId;
    }
}
