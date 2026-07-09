using DotCruz.Tenants.Api.Extensions;
using DotCruz.Tenants.Application.DTOs.Base;
using DotCruz.Tenants.CrossCutting.LogResources;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using Microsoft.AspNetCore.Diagnostics;

namespace DotCruz.Tenants.Api.Handlers;

public class TenantExceptionHandler(ILogger<TenantExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken
        )
    {
        if (exception is TenantException tenantException)
        {
            var (domainError, domainStatus) = HandleDomainException(tenantException);

            httpContext.Response.StatusCode = domainStatus;

            await httpContext.Response.WriteAsJsonAsync(domainError, cancellationToken);
            return true;
        }

        var (unknownError, unknownStatus) = HandleUnknownException(exception);

        httpContext.Response.StatusCode = unknownStatus;
        await httpContext.Response.WriteAsJsonAsync(unknownError, cancellationToken);

        return true;
    }

    private (ErrorResponseDto errorResponse, int statusCode) HandleDomainException(TenantException tenantException)
    {
        logger.LogWarning(
            tenantException,
            string.Format(
                ResourceMessagesLog.HANDLED_EXCEPTION, 
                tenantException.GetErrorType(), 
                tenantException.GetErrorType().ToStatusCode(), 
                tenantException.GetErrorsMessages()
            )
        );

        return (
            new ErrorResponseDto(tenantException.GetErrorsMessages()),
            tenantException.GetErrorType().ToStatusCode()
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
