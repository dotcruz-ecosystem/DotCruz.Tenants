using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth;
using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth.Requests;
using DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth.Responses;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;

namespace DotCruz.Workspaces.Infrastructure.Services.CoreAuth;

public class CoreAuthClient(HttpClient httpClient) : ICoreAuthClient
{
    private const string CREATE_USER_ENDPOINT = "/api/users";

    private readonly JsonSerializerOptions _serializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
    };

    public async Task CreateWorkspaceAdminUser(CreateWorkspaceAdminUserRequestDto requestDto, CancellationToken cancellationToken)
    {
        using var request = new HttpRequestMessage(HttpMethod.Post, CREATE_USER_ENDPOINT)
        {
            Content = JsonContent.Create(requestDto, options: _serializerOptions)
        };

        request.Headers.Add("X-Workspace-ID", requestDto.WorkspaceId.ToString());

        var response = await httpClient.SendAsync(request, cancellationToken);

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
