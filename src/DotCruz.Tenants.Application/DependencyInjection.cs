using System.Reflection;
using DotCruz.Tenants.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DotCruz.Tenants.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        AddMediatR(services, assembly);
        AddValidators(services, assembly);

        return services;
    }

    private static void AddMediatR(IServiceCollection services, Assembly assembly)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(assembly);
            config.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
    }

    private static void AddValidators(IServiceCollection services, Assembly assembly)
    {
        services.AddValidatorsFromAssembly(assembly);
    }
}
