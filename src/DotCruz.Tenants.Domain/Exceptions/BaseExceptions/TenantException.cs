using DotCruz.Tenants.Domain.Exceptions.Enums;

namespace DotCruz.Tenants.Domain.Exceptions.BaseExceptions;

public abstract class TenantException : Exception
{
    protected TenantException(string message) : base(message) { }
    public abstract IEnumerable<string> GetErrorsMessages();
    public abstract ErrorType GetErrorType();
}
