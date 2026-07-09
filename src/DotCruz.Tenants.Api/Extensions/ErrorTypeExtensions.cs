using DotCruz.Tenants.Domain.Exceptions.Enums;

namespace DotCruz.Tenants.Api.Extensions;

public static class ErrorTypeExtensions
{
    public static int ToStatusCode(this ErrorType errorType) => errorType switch
    {
        ErrorType.Validation => StatusCodes.Status400BadRequest,
        ErrorType.NotFound => StatusCodes.Status404NotFound,
        ErrorType.Conflict => StatusCodes.Status409Conflict,
        ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
        ErrorType.Infrastructure => StatusCodes.Status500InternalServerError,
        _ => StatusCodes.Status500InternalServerError
    };
}
