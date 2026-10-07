namespace DotCruz.Workspaces.Application.Abstractions.Services.CoreAuth.Responses;

public sealed record CoreAuthErrorResponse(IEnumerable<string> Errors);
