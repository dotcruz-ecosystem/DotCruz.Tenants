using Amazon.SimpleSystemsManagement;
using DotCruz.Workspaces.Application.Abstractions.Services.Smtp;
using DotCruz.Workspaces.Infrastructure.Services.Smtp;
using DotCruz.Workspaces.Application.Abstractions.Data;
using DotCruz.Workspaces.Application.Abstractions.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth;
using DotCruz.Workspaces.Application.Abstractions.Services.Storage;
using DotCruz.Workspaces.Infrastructure.Data;
using DotCruz.Workspaces.Infrastructure.Data.Repositories.Workspaces;
using DotCruz.Workspaces.Infrastructure.Services.CoreAuth;
using DotCruz.Workspaces.Infrastructure.Services.Storage;
using DotCruz.Workspaces.Infrastructure.Messaging.Consumers;
using DotCruz.Workspaces.Infrastructure.Messaging.Settings;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotCruz.Workspaces.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddUnitOfWork(services);
        AddDbContext(services, configuration);
        AddServices(services, configuration);
        AddMessaging(services, configuration);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IWorkspaceReadRepository, WorkspaceReadRepository>();
        services.AddScoped<IWorkspaceWriteRepository, WorkspaceWriteRepository>();
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<WorkspaceDbContext>(opt =>
        {
            opt.UseNpgsql(connectionString);
            opt.UseSnakeCaseNamingConvention();
        });
    }

    private static void AddServices(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<CloudflareR2Settings>(configuration.GetSection(CloudflareR2Settings.SectionName));
        services.AddScoped<IStorageService, CloudflareR2StorageService>();

        services.Configure<AwsSettings>(configuration.GetSection(AwsSettings.SectionName));
        services.AddScoped<ISmtpConfigService, SmtpConfigService>();

        services.AddSingleton<IAmazonSimpleSystemsManagement>(sp =>
        {
            var awsSettings = configuration.GetSection(AwsSettings.SectionName).Get<AwsSettings>();
            var config = new AmazonSimpleSystemsManagementConfig
            {
                RegionEndpoint = Amazon.RegionEndpoint.GetBySystemName(awsSettings?.Region ?? "us-east-1")
            };

            if (awsSettings != null && !string.IsNullOrEmpty(awsSettings.AccessKey) && !string.IsNullOrEmpty(awsSettings.SecretKey))
            {
                return new AmazonSimpleSystemsManagementClient(awsSettings.AccessKey, awsSettings.SecretKey, config);
            }

            return new AmazonSimpleSystemsManagementClient(config);
        });

        services.AddHttpClient<ICoreAuthClient, CoreAuthClient>(client =>
        {
            var baseAddress = configuration["CoreAuth:BaseAddress"];
            if (!string.IsNullOrEmpty(baseAddress))
            {
                client.BaseAddress = new Uri(baseAddress);
            }
        })
        .AddServiceApiKeyPropagation();
    }

    private static void AddMessaging(IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<WorkspaceMessagingSettings>(configuration.GetSection(WorkspaceMessagingSettings.SectionName));
        var messagingSettings = configuration.GetSection(WorkspaceMessagingSettings.SectionName).Get<WorkspaceMessagingSettings>()
            ?? new WorkspaceMessagingSettings();

        services.AddMassTransit(x =>
        {
            x.AddConsumer<UserRegisteredConsumer>();

            if (messagingSettings.UseInMemory)
            {
                x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                });
            }
            else
            {
                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(messagingSettings.Host, messagingSettings.Port, "/", h =>
                    {
                        h.Username(messagingSettings.Username);
                        h.Password(messagingSettings.Password);
                    });
                    cfg.ConfigureEndpoints(context);
                });
            }
        });
    }
}
