using MassTransit;
using Moq;

namespace CommonTestUtilities.Services;

public class ConsumeContextBuilder
{
    public static ConsumeContext<T> Build<T>(T message, CancellationToken cancellationToken = default) where T : class
    {
        var mock = new Mock<ConsumeContext<T>>();
        mock.Setup(c => c.Message).Returns(message);
        mock.Setup(c => c.CancellationToken).Returns(cancellationToken);
        return mock.Object;
    }
}
