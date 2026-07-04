using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using System.Text.RegularExpressions;

namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class TenantSlug
{
    private static readonly Regex SlugRegex = new(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled);
    private static readonly HashSet<string> ReservedSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        "admin", "api", "support", "billing", "mail", "portal", "auth", 
        "tenant", "tenants", "root", "administrator", "system", "help", 
        "security", "assets", "static", "images", "jobs", "status"
    };

    public string Value { get; }

    public TenantSlug(string value)
    {
        var cleanValue = value?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(cleanValue))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_EMPTY);

        if (cleanValue.Length < 3 || cleanValue.Length > 50)
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_INVALID_LENGTH);

        if (!SlugRegex.IsMatch(cleanValue))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_INVALID_FORMAT);

        if (ReservedSlugs.Contains(cleanValue))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_RESERVED);

        Value = cleanValue;
    }

    public override string ToString() => Value;
}
