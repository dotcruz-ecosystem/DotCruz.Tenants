using DotCruz.Tenants.Application.Abstractions.Security;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ActivateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.CreateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.DeactivateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.SuspendTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.TerminateTenant;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantSubscription;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetTenantBySlug;
using DotCruz.Tenants.Application.UseCases.Tenants.Queries.SearchTenants;
using MediatR;
using System.Reflection;

namespace UseCases.Test.Behaviors;

public class TenantScopedRequestCoverageTests
{
    private static readonly Type[] PlatformScopedRequests =
    [
        typeof(CreateTenantCommand),
        typeof(SearchTenantsQuery),
        typeof(GetTenantBySlugQuery),
        typeof(ActivateTenantCommand),
        typeof(DeactivateTenantCommand),
        typeof(SuspendTenantCommand),
        typeof(UpdateTenantSubscriptionCommand),
        typeof(TerminateTenantCommand)
    ];

    [Fact]
    public void Every_Request_Declares_Whether_It_Is_Tenant_Scoped()
    {
        var undeclared = RequestTypes()
            .Where(type => !typeof(ITenantScopedRequest).IsAssignableFrom(type))
            .Where(type => !PlatformScopedRequests.Contains(type))
            .Select(type => type.Name)
            .OrderBy(name => name)
            .ToList();

        Assert.True(
            undeclared.Count == 0,
            $"Requests sem decisao de escopo: {string.Join(", ", undeclared)}. " +
            "Implemente ITenantScopedRequest para restringir ao tenant do chamador, " +
            "ou registre em PlatformScopedRequests se a operacao e de plataforma.");
    }

    [Fact]
    public void Tenant_Scoped_Requests_Expose_A_Non_Empty_Tenant_Id()
    {
        var scoped = RequestTypes()
            .Where(type => typeof(ITenantScopedRequest).IsAssignableFrom(type))
            .ToList();

        Assert.NotEmpty(scoped);

        foreach (var type in scoped)
        {
            var property = type.GetInterfaceMap(typeof(ITenantScopedRequest))
                .TargetMethods
                .FirstOrDefault(method => method.ReturnType == typeof(Guid));

            Assert.NotNull(property);
        }
    }

    private static IEnumerable<Type> RequestTypes()
    {
        return Assembly.GetAssembly(typeof(CreateTenantCommand))!
            .GetTypes()
            .Where(type => type is { IsAbstract: false, IsInterface: false })
            .Where(typeof(IBaseRequest).IsAssignableFrom);
    }
}
