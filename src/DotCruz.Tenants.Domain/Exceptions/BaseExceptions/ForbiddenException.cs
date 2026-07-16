using DotCruz.Tenants.Domain.Exceptions.Enums;

namespace DotCruz.Tenants.Domain.Exceptions.BaseExceptions;

public sealed class ForbiddenException(string message) : TenantException(string.Empty)
{
    public override IEnumerable<string> GetErrorsMessages() => [message];
    public override ErrorType GetErrorType() => ErrorType.Forbidden;
}
