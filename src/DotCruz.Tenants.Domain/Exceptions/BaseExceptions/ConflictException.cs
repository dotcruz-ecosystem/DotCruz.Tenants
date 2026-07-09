using DotCruz.Tenants.Domain.Exceptions.Enums;

namespace DotCruz.Tenants.Domain.Exceptions.BaseExceptions;

public class ConflictException : TenantException
{
    private readonly string _message;

    public ConflictException(string message) : base(message)
    {
        _message = message;
    }

    public override IEnumerable<string> GetErrorsMessages() => [_message];

    public override ErrorType GetErrorType() => ErrorType.Conflict;
}
