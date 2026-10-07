using DotCruz.Workspaces.Domain.Exceptions.Enums;

namespace DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;

public sealed class ForbiddenException(string message) : WorkspaceException(string.Empty)
{
    public override IEnumerable<string> GetErrorsMessages() => [message];
    public override ErrorType GetErrorType() => ErrorType.Forbidden;
}
