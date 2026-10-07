using DotCruz.Workspaces.Application.Abstractions.Security;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ActivateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.CreateWorkspace;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.DeactivateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.ProvisionPersonalWorkspace;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.SuspendTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.TerminateTenant;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Commands.UpdateWorkspaceSubscription;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetTenantBySlug;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUserWorkspaces;
using DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.SearchTenants;
using MediatR;
using System.Reflection;

namespace UseCases.Test.Behaviors;

public class WorkspaceScopedRequestCoverageTests
{
    private static readonly Type[] PlatformScopedRequests =
    [
        typeof(CreateTenantCommand),
        typeof(CreateWorkspaceCommand),
        typeof(ProvisionPersonalWorkspaceCommand),
        typeof(SearchTenantsQuery),
        typeof(GetTenantBySlugQuery),
        typeof(GetUserWorkspacesQuery),
        typeof(ActivateTenantCommand),
        typeof(DeactivateTenantCommand),
        typeof(SuspendTenantCommand),
        typeof(UpdateWorkspaceSubscriptionCommand),
        typeof(TerminateTenantCommand)
    ];

    [Fact]
    public void Every_Request_Declares_Whether_It_Is_Tenant_Scoped()
    {
        var undeclared = RequestTypes()
            .Where(type => !typeof(IWorkspaceScopedRequest).IsAssignableFrom(type))
            .Where(type => !PlatformScopedRequests.Contains(type))
            .Select(type => type.Name)
            .OrderBy(name => name)
            .ToList();

        Assert.True(
            undeclared.Count == 0,
            $"Requests sem decisao de escopo: {string.Join(", ", undeclared)}. " +
            "Implemente IWorkspaceScopedRequest para restringir ao Workspace do chamador, " +
            "ou registre em PlatformScopedRequests se a operacao e de plataforma.");
    }

    [Fact]
    public void Tenant_Scoped_Requests_Expose_A_Non_Empty_Tenant_Id()
    {
        var scoped = RequestTypes()
            .Where(type => typeof(IWorkspaceScopedRequest).IsAssignableFrom(type))
            .ToList();

        Assert.NotEmpty(scoped);

        foreach (var type in scoped)
        {
            var property = type.GetInterfaceMap(typeof(IWorkspaceScopedRequest))
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
