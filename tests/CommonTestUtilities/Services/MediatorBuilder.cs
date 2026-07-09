using MediatR;
using Moq;

namespace CommonTestUtilities.Services;

public class MediatorBuilder
{
    public static IMediator Build()
    { 
        var mediator = new Mock<IMediator>();

        return mediator.Object;
    }
}
