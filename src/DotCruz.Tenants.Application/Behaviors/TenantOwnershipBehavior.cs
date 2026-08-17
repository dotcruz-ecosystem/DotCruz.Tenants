using DotCruz.Shared.Security.Context;
using DotCruz.Tenants.Application.Abstractions.Security;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace DotCruz.Tenants.Application.Behaviors;

public class TenantOwnershipBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private const string SuperAdminRole = "SuperAdmin";

    private readonly ISecurityContext _securityContext;

    public TenantOwnershipBehavior(ISecurityContext securityContext)
    {
        _securityContext = securityContext;
    }

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (request is ITenantScopedRequest scopedRequest && !IsAllowed(scopedRequest))
            throw new ForbiddenException(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE);

        return await next(cancellationToken);
    }

    private bool IsAllowed(ITenantScopedRequest request)
    {
        if (_securityContext.IsAuthenticatedService)
            return true;

        if (_securityContext.Roles.Contains(SuperAdminRole, StringComparer.OrdinalIgnoreCase))
            return true;

        var callerTenantId = _securityContext.TenantId;

        return callerTenantId.HasValue
            && callerTenantId.Value != Guid.Empty
            && callerTenantId.Value == request.TenantId;
    }
}
