using DotCruz.Workspaces.Domain.Exceptions.BaseExceptions;
using DotCruz.Workspaces.Domain.Exceptions.Resources;
using System.Text.RegularExpressions;

namespace DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

public partial record class WorkspaceSlug
{
    [GeneratedRegex(@"^[a-z0-9]+(?:-[a-z0-9]+)*$", RegexOptions.Compiled)]
    private static partial Regex SlugRegex();

    private static readonly HashSet<string> ReservedSlugs = new(StringComparer.OrdinalIgnoreCase)
    {
        "admin", "api", "support", "billing", "mail", "portal", "auth", 
        "Workspace", "tenants", "root", "administrator", "system", "help", 
        "security", "assets", "static", "images", "jobs", "status"
    };

    public string Value { get; }

    private WorkspaceSlug(string value) => Value = value;

    public static WorkspaceSlug Create(string value)
    {
        var cleanValue = value?.Trim().ToLowerInvariant();

        if (string.IsNullOrWhiteSpace(cleanValue))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_EMPTY);

        if (cleanValue.Length < 3 || cleanValue.Length > 50)
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_INVALID_LENGTH);

        if (!SlugRegex().IsMatch(cleanValue))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_INVALID_FORMAT);

        if (ReservedSlugs.Contains(cleanValue))
            throw new ErrorOnValidationException(ResourceMessagesException.SLUG_RESERVED);

        return new WorkspaceSlug(cleanValue);
    }

    public override string ToString() => Value;
    public static implicit operator string(WorkspaceSlug slug) => slug.Value;
}
