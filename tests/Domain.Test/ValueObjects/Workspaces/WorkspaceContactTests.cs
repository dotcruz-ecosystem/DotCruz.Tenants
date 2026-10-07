using CommonTestUtilities.ValueObjects.Workspaces;

namespace Domain.Test.ValueObjects.Workspaces;

public class WorkspaceContactTests
{
    [Fact]
    public void Success()
    {
        var contact = WorkspaceContactBuilder.Build();

        Assert.NotNull(contact);
        Assert.NotNull(contact.Email);
        Assert.NotNull(contact.Phone);
    }
}
