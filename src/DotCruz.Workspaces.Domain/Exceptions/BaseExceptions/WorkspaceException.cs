using DotCruz.Workspaces.Domain.Exceptions.Enums;

namespace DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;

public abstract class WorkspaceException : Exception
{
    protected WorkspaceException(string message) : base(message) { }
    public abstract IEnumerable<string> GetErrorsMessages();
    public abstract ErrorType GetErrorType();
}
