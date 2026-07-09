using DotCruz.Tenants.Api.Configurations;
using DotCruz.Tenants.Api.Handlers;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


builder.Services.AddExceptionHandler<TenantExceptionHandler>();
builder.Services.AddApiConventions();
builder.Services.AddOpenApiDocumentation();
builder.Services.AddProblemDetails();

var app = builder.Build();

app.UseExceptionHandler();

app.MapOpenApiDocumentation();


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
