using DotCruz.Workspaces.Api.Extensions;
using DotCruz.Workspaces.Application.DTOs.Base;
using DotCruz.Workspaces.CrossCutting.LogResources;
using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using Microsoft.AspNetCore.Diagnostics;

namespace DotCruz.Workspaces.Api.Handlers;

public class WorkspaceExceptionHandler(ILogger<WorkspaceExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
    {
        if (exception is WorkspaceException WorkspaceException)
        {
            var (domainError, domainStatus) = HandleDomainException(WorkspaceException);

            httpContext.Response.StatusCode = domainStatus;

            await httpContext.Response.WriteAsJsonAsync(domainError, cancellationToken);
            return true;
        }

        var (unknownError, unknownStatus) = HandleUnknownException(exception);

        httpContext.Response.StatusCode = unknownStatus;
        await httpContext.Response.WriteAsJsonAsync(unknownError, cancellationToken);

        return true;
    }

    private (ErrorResponseDto errorResponse, int statusCode) HandleDomainException(WorkspaceException WorkspaceException)
    {
        logger.LogWarning(
            WorkspaceException,
            string.Format(
                ResourceMessagesLog.HANDLED_EXCEPTION, 
                WorkspaceException.GetErrorType(), 
                WorkspaceException.GetErrorType().ToStatusCode(), 
                WorkspaceException.GetErrorsMessages()
            )
        );

        return (
            new ErrorResponseDto(WorkspaceException.GetErrorsMessages()),
            WorkspaceException.GetErrorType().ToStatusCode()
        );
    }

    private (ErrorResponseDto errorResponse, int statusCode) HandleUnknownException(Exception exception)
    {
        logger.LogError(
            exception,
            string.Format(
                ResourceMessagesLog.UNHANDLED_EXCEPTION,
                StatusCodes.Status500InternalServerError,
                exception.Message
            )
        );
        
        return (
            new ErrorResponseDto(ResourceMessagesException.UNKNOWN_ERROR),
            StatusCodes.Status500InternalServerError
        );
    }
}
