using DotCruz.Workspaces.Domain.Exceptions.Enums;

namespace DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;

public class NotFoundException : WorkspaceException
{
    private readonly string _message;

    public NotFoundException(string message) : base(message)
    {
        _message = message;
    }

    public override IEnumerable<string> GetErrorsMessages() => [_message];

    public override ErrorType GetErrorType() => ErrorType.NotFound;
}
