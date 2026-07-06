using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Location;

public record class State
{
    public string Value { get; }

    private State(string value)
    {
        Value = value;
    }

    public static State Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Trim().Length != 2)
            throw new ErrorOnValidationException(ResourceMessagesException.STATE_INVALID);

        return new State(value.Trim().ToUpperInvariant());
    }

    public override string ToString() => Value;

    public static implicit operator string(State state) => state.Value;
}
