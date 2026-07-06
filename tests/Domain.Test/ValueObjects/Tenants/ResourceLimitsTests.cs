using CommonTestUtilities.ValueObjects.Tenants;
using DotCruz.Tenants.Domain.Exceptions.BaseExceptions;
using DotCruz.Tenants.Domain.Exceptions.Resources;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;

namespace Domain.Test.ValueObjects.Tenants;

public class ResourceLimitsTests
{
    [Fact]
    public void Success()
    {
        var limits = ResourceLimitsBuilder.Build();

        Assert.NotNull(limits);
        Assert.True(limits.MaxUsers > 0);
        Assert.True(limits.MaxEmailsPerMonth > 0);
    }

    [Fact]
    public void Success_Default_Limits()
    {
        var trial = ResourceLimits.CreateDefaultTrial();
        Assert.Equal(5, trial.MaxUsers);
        Assert.Equal(1000, trial.MaxEmailsPerMonth);

        var free = ResourceLimits.CreateDefaultFree();
        Assert.Equal(1, free.MaxUsers);
        Assert.Equal(100, free.MaxEmailsPerMonth);

        var pro = ResourceLimits.CreateDefaultPro();
        Assert.Equal(50, pro.MaxUsers);
        Assert.Equal(50000, pro.MaxEmailsPerMonth);

        var enterprise = ResourceLimits.CreateDefaultEnterprise();
        Assert.Equal(9999, enterprise.MaxUsers);
        Assert.Equal(1000000, enterprise.MaxEmailsPerMonth);
    }

    [Fact]
    public void Error_ResourceLimits_Invalid_MaxUsers()
    {
        static ResourceLimits act() => ResourceLimitsBuilder.Build(hasInvalidMaxUsers: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.RESOURCE_LIMITS_INVALID, exceptionMessage);
    }

    [Fact]
    public void Error_ResourceLimits_Invalid_MaxEmails()
    {
        static ResourceLimits act() => ResourceLimitsBuilder.Build(hasInvalidMaxEmails: true);

        var exception = Assert.Throws<ErrorOnValidationException>(act);
        var exceptionMessage = Assert.Single(exception.GetErrorsMessages());
        Assert.Equal(ResourceMessagesException.RESOURCE_LIMITS_INVALID, exceptionMessage);
    }
}
