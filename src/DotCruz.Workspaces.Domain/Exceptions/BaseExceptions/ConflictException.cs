using DotCruz.Workspaces.Domain.Exceptions.Enums;

namespace DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;

public class ConflictException : WorkspaceException
{
    private readonly string _message;

    public ConflictException(string message) : base(message)
    {
        _message = message;
    }

    public override IEnumerable<string> GetErrorsMessages() => [_message];

    public override ErrorType GetErrorType() => ErrorType.Conflict;
}
