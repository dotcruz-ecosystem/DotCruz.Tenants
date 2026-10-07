using DotCruz.Workspaces.Application.Abstractions.Data;
using Moq;

namespace CommonTestUtilities.Data;

public class UnitOfWorkerBuilder
{
    private readonly Mock<IUnitOfWork> _mock = new();

    public static IUnitOfWork Build()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        return unitOfWorkMock.Object;
    }

    public Mock<IUnitOfWork> Mock => _mock;
    public IUnitOfWork BuildInstance() => _mock.Object;
}
