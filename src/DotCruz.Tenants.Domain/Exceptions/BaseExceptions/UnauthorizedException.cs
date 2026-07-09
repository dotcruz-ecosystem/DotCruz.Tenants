using DotCruz.Tenants.Domain.Exceptions.Enums;

namespace DotCruz.Tenants.Domain.Exceptions.BaseExceptions;

public class UnauthorizedException : TenantException
{
    private readonly string _message;

    public UnauthorizedException(string message) : base(message)
    {
        _message = message;
    }

    public override IEnumerable<string> GetErrorsMessages() => [_message];

    public override ErrorType GetErrorType() => ErrorType.Unauthorized;
}
