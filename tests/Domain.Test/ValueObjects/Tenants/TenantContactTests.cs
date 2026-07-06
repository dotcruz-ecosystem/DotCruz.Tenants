using CommonTestUtilities.ValueObjects.Tenants;

namespace Domain.Test.ValueObjects.Tenants;

public class TenantContactTests
{
    [Fact]
    public void Success()
    {
        var contact = TenantContactBuilder.Build();

        Assert.NotNull(contact);
        Assert.NotNull(contact.Email);
        Assert.NotNull(contact.Phone);
    }
}
