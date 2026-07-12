using DotCruz.Tenants.Api.Configurations;
using DotCruz.Tenants.Api.Handlers;
using DotCruz.Tenants.Application;
using DotCruz.Tenants.Infrastructure;
using DotCruz.Shared.Security;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddSharedSecurity(builder.Configuration);

builder.Services.AddExceptionHandler<TenantExceptionHandler>();
builder.Services.AddApiConventions();
builder.Services.AddOpenApiDocumentation();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApiDocumentation();


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
