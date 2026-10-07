using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;

namespace DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

public record class SuspensionReason
{
    public string Value { get; }

    private SuspensionReason(string value)
    {
        Value = value;
    }

    public static SuspensionReason Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ErrorOnValidationException(ResourceMessagesException.SUSPENSION_REASON_EMPTY);

        var cleanValue = value.Trim();

        if (cleanValue.Length < 5 || cleanValue.Length > 500)
            throw new ErrorOnValidationException(ResourceMessagesException.SUSPENSION_REASON_INVALID);

        return new SuspensionReason(cleanValue);
    }

    public override string ToString() => Value;

    public static implicit operator string(SuspensionReason reason) => reason.Value;
}
