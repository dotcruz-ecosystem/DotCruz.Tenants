using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Location;

public record class State
{
    public string Value { get; }

    public State(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length != 2)
            throw new ErrorOnValidationException(ResourceMessagesException.STATE_INVALID);

        Value = value.Trim().ToUpperInvariant();
    }

    public override string ToString() => Value;

    public static implicit operator string(State state) => state.Value;
}
