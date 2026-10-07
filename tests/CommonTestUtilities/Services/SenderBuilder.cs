using MediatR;
using Moq;

namespace CommonTestUtilities.Services;

public class SenderBuilder
{
    private readonly Mock<ISender> _mock = new();

    public SenderBuilder SetupSend<TRequest>(TRequest request) where TRequest : IRequest
    {
        _mock.Setup(s => s.Send(request, It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return this;
    }

    public SenderBuilder SetupSendAny<TRequest>() where TRequest : IRequest
    {
        _mock.Setup(s => s.Send(It.IsAny<TRequest>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        return this;
    }

    public ISender Build() => _mock.Object;
    public Mock<ISender> Mock => _mock;
}
