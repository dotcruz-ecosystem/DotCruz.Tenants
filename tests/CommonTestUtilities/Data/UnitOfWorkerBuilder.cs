using DotCruz.Tenants.Application.Abstractions.Data;
using Moq;

namespace CommonTestUtilities.Data;

public class UnitOfWorkerBuilder
{
    public static IUnitOfWork Build()
    {
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        return unitOfWorkMock.Object;
    }
}
