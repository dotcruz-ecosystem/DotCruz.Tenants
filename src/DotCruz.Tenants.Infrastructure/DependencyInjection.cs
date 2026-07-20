using Amazon.SimpleSystemsManagement;
using DotCruz.Tenants.Application.Abstractions.Services.Smtp;
using DotCruz.Tenants.Infrastructure.Services.Smtp;
using DotCruz.Tenants.Application.Abstractions.Data;
using DotCruz.Tenants.Application.Abstractions.Data.Repositories.Tenants;
using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth;
using DotCruz.Tenants.Application.Abstractions.Services.Storage;
using DotCruz.Tenants.Infrastructure.Data;
using DotCruz.Tenants.Infrastructure.Data.Repositories.Tenants;
using DotCruz.Tenants.Infrastructure.Services.CoreAuth;
using DotCruz.Tenants.Infrastructure.Services.Storage;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DotCruz.Tenants.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        AddRepositories(services);
        AddUnitOfWork(services);
        AddDbContext(services, configuration);
        AddServices(services, configuration);

        return services;
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<ITenantReadRepository, TenantReadRepository>();
        services.AddScoped<ITenantWriteRepository, TenantWriteRepository>();
    }

    private static void AddUnitOfWork(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
    }

    private static void AddDbContext(IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<TenantDbContext>(opt =>
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
}
