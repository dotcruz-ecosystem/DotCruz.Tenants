using CommonTestUtilities.Security;
using DotCruz.Shared.Security.Context;
using DotCruz.Tenants.Application.Abstractions.Security;
using DotCruz.Tenants.Application.Behaviors;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using MediatR;

namespace UseCases.Test.Behaviors;

public class TenantOwnershipBehaviorTests
{
    private sealed record ScopedRequest(Guid TenantId) : IRequest<string>, ITenantScopedRequest;

    private sealed record UnscopedRequest : IRequest<string>;

    [Fact]
    public async Task Success_Caller_Owns_The_Tenant()
    {
        var tenantId = Guid.NewGuid();
        var securityContext = new SecurityContextBuilder().AsTenantAdmin(tenantId).Build();

        var result = await Execute(new ScopedRequest(tenantId), securityContext);

        Assert.Equal("handled", result);
    }

    [Fact]
    public async Task Success_Super_Admin_Reaches_Any_Tenant()
    {
        var securityContext = new SecurityContextBuilder().AsSuperAdmin(Guid.NewGuid()).Build();

        var result = await Execute(new ScopedRequest(Guid.NewGuid()), securityContext);

        Assert.Equal("handled", result);
    }

    [Fact]
    public async Task Success_Service_Caller_Has_No_Tenant()
    {
        var securityContext = new SecurityContextBuilder().AsService().Build();

        var result = await Execute(new ScopedRequest(Guid.NewGuid()), securityContext);

        Assert.Equal("handled", result);
    }

    [Fact]
    public async Task Success_Unscoped_Request_Is_Not_Checked()
    {
        var securityContext = new SecurityContextBuilder().AsTenantAdmin().Build();

        var result = await ExecuteUnscoped(new UnscopedRequest(), securityContext);

        Assert.Equal("handled", result);
    }

    [Fact]
    public async Task Error_Tenant_Admin_Reaching_Another_Tenant()
    {
        var securityContext = new SecurityContextBuilder().AsTenantAdmin(Guid.NewGuid()).Build();

        Task act() => Execute(new ScopedRequest(Guid.NewGuid()), securityContext);

        var exception = await Assert.ThrowsAsync<ForbiddenException>(act);
        var message = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.USER_WITHOUT_PERMISSION_ACCESS_RESOURCE, message);
    }

    [Fact]
    public async Task Error_Caller_Without_Tenant()
    {
        var securityContext = new SecurityContextBuilder().AsTenantAdmin(tenantId: null).Build();

        Task act() => Execute(new ScopedRequest(Guid.NewGuid()), securityContext);

        await Assert.ThrowsAsync<ForbiddenException>(act);
    }

    [Fact]
    public async Task Error_Caller_With_Empty_Tenant()
    {
        var securityContext = new SecurityContextBuilder().AsTenantAdmin(Guid.Empty).Build();

        Task act() => Execute(new ScopedRequest(Guid.Empty), securityContext);

        await Assert.ThrowsAsync<ForbiddenException>(act);
    }

    private static Task<string> Execute(ScopedRequest request, ISecurityContext securityContext)
    {
        var behavior = new TenantOwnershipBehavior<ScopedRequest, string>(securityContext);

        return behavior.Handle(request, _ => Task.FromResult("handled"), TestContext.Current.CancellationToken);
    }

    private static Task<string> ExecuteUnscoped(UnscopedRequest request, ISecurityContext securityContext)
    {
        var behavior = new TenantOwnershipBehavior<UnscopedRequest, string>(securityContext);

        return behavior.Handle(request, _ => Task.FromResult("handled"), TestContext.Current.CancellationToken);
    }
}
