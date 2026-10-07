using DotCruz.Workspaces.Domain.Exceptions.Enums;

namespace DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;

public class InfrastructureException : WorkspaceException
{
    private readonly string _message;

    public InfrastructureException(string message) : base(message)
    {
        _message = message;
    }

    public override IEnumerable<string> GetErrorsMessages() => [_message];

    public override ErrorType GetErrorType() => ErrorType.Infrastructure;
}
