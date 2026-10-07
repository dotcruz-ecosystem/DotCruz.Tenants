using DotCruz.Workspaces.Api.Configurations;
using DotCruz.Workspaces.Api.Handlers;
using DotCruz.Workspaces.Application;
using DotCruz.Workspaces.Infrastructure;
using DotCruz.Shared.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSharedSecurity(builder.Configuration);

// Sobrescreve as políticas do Shared Security para permitir acesso do SuperAdmin
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(DotCruz.Shared.Security.Authorization.SecurityPolicies.AdminOnly, policy =>
    {
        policy.AddAuthenticationSchemes(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
        policy.RequireRole("Admin", "SuperAdmin");
    });

    options.AddPolicy(DotCruz.Shared.Security.Authorization.SecurityPolicies.WorkspaceAdminOrAdmin, policy =>
    {
        policy.AddAuthenticationSchemes(Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme);
        policy.RequireRole("WorkspaceAdmin", "Admin", "SuperAdmin", "Owner");
    });
});

builder.Services.AddExceptionHandler<WorkspaceExceptionHandler>();
builder.Services.AddApiConventions();
builder.Services.AddOpenApiDocumentation();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApiDocumentation();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
