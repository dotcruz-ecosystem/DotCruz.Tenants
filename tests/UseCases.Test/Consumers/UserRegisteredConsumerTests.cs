using CommonTestUtilities.Services;
using DotCruz.CoreAuth.Application.Interfaces.Services.Messaging.Messages;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ProvisionPersonalWorkspace;
using DotCruz.Tenants.Infrastructure.Messaging.Consumers;
using Moq;

namespace UseCases.Test.Consumers;

public class UserRegisteredConsumerTests
{
    [Fact]
    public async Task Success_Dispatches_ProvisionPersonalWorkspaceCommand()
    {
        var message = new UserRegisteredIntegrationEvent(
            UserId: Guid.NewGuid(),
            Name: "Carlos Drumond",
            Email: "carlos@example.com",
            OccurredOn: DateTime.UtcNow
        );

        var senderBuilder = new SenderBuilder();
        senderBuilder.SetupSendAny<ProvisionPersonalWorkspaceCommand>();

        var logger = LoggerBuilder.Build<UserRegisteredConsumer>();
        var consumer = new UserRegisteredConsumer(senderBuilder.Build(), logger);

        var consumeContext = ConsumeContextBuilder.Build(message, TestContext.Current.CancellationToken);

        await consumer.Consume(consumeContext);

        senderBuilder.Mock.Verify(s => s.Send(
            It.Is<ProvisionPersonalWorkspaceCommand>(cmd =>
                cmd.UserId == message.UserId &&
                cmd.Name == message.Name &&
                cmd.Email == message.Email
            ),
            TestContext.Current.CancellationToken
        ), Times.Once);
    }
}
