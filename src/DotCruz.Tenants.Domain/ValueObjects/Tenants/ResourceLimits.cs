using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;

namespace DotCruz.Tenants.Domain.ValueObjects.Tenants;

public record class ResourceLimits
{
    public int MaxUsers { get; }
    public int MaxEmailsPerMonth { get; }

    public ResourceLimits(int maxUsers, int maxEmailsPerMonth)
    {
        if (maxUsers <= 0 || maxEmailsPerMonth <= 0)
            throw new ErrorOnValidationException(ResourceMessagesException.RESOURCE_LIMITS_INVALID);

        MaxUsers = maxUsers;
        MaxEmailsPerMonth = maxEmailsPerMonth;
    }

    public static ResourceLimits CreateDefaultTrial() => new(5, 1000);
    public static ResourceLimits CreateDefaultFree() => new(1, 100);
    public static ResourceLimits CreateDefaultPro() => new(50, 50000);
    public static ResourceLimits CreateDefaultEnterprise() => new(9999, 1000000);
}
