using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth;
using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth.Requests;
using System.Net.Http.Json;
using System.Text.Json;

namespace DotCruz.Tenants.Infrastructure.Services.CoreAuth;

public class CoreAuthClient(HttpClient httpClient) : ICoreAuthClient
{
    private const string CREATE_USER_ENDPOINT = "/api/users";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task CreateTenantAdminUser(CreateTenantAdminUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        await httpClient.PostAsJsonAsync(CREATE_USER_ENDPOINT, requestDto, _serializerOptions, cancellationToken);
    }
}
