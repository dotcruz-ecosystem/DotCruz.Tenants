using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth;
using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth.Requests;
using DotCruz.Tenants.Application.Abstractions.Services.CoreAuth.Responses;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using System.Net;
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
        var response = await httpClient.PostAsJsonAsync(CREATE_USER_ENDPOINT, requestDto, _serializerOptions, cancellationToken);

        if (!response.IsSuccessStatusCode)
            await HandleErrorResponse(response, cancellationToken);
    }

    private async Task HandleErrorResponse(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        var errors = await GetErrorsAsync(response, cancellationToken);
        var primaryError = errors.FirstOrDefault() ?? ResourceMessagesException.UNKNOWN_ERROR_CORE_AUTH;

        throw response.StatusCode switch
        {
            HttpStatusCode.BadRequest => new ErrorOnValidationException(errors),
            HttpStatusCode.NotFound => new NotFoundException(primaryError),
            HttpStatusCode.Conflict => new ConflictException(primaryError),
            HttpStatusCode.Unauthorized => new UnauthorizedException(primaryError),
            HttpStatusCode.Forbidden => new UnauthorizedException(primaryError),
            _ => new InfrastructureException(string.Format(ResourceMessagesException.EXTERNAL_INTEGRATION_ERROR, response.StatusCode, primaryError))
        };
    }

    private async Task<IEnumerable<string>> GetErrorsAsync(HttpResponseMessage response, CancellationToken cancellationToken)
    {
        try
        {
            var errorResponse = await response.Content.ReadFromJsonAsync<CoreAuthErrorResponse>(_serializerOptions, cancellationToken);
            return errorResponse?.Errors ?? [];
        }
        catch
        {
            return [];
        }
    }
}
