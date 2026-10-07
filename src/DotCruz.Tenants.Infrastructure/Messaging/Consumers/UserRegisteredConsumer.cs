using DotCruz.CoreAuth.Application.Interfaces.Services.Messaging.Messages;
using DotCruz.Tenants.Application.UseCases.Tenants.Commands.ProvisionPersonalWorkspace;
using MassTransit;
using MediatR;
using Microsoft.Extensions.Logging;

namespace DotCruz.Tenants.Infrastructure.Messaging.Consumers;

public class UserRegisteredConsumer : IConsumer<UserRegisteredIntegrationEvent>
{
    private readonly ISender _sender;
    private readonly ILogger<UserRegisteredConsumer> _logger;

    public UserRegisteredConsumer(ISender sender, ILogger<UserRegisteredConsumer> logger)
    {
        _sender = sender;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<UserRegisteredIntegrationEvent> context)
    {
        var message = context.Message;
        _logger.LogInformation("Processing UserRegisteredIntegrationEvent for user {UserId} ({Email})", message.UserId, message.Email);

        var command = new ProvisionPersonalWorkspaceCommand(
            message.UserId,
            message.Name,
            message.Email
        );

        await _sender.Send(command, context.CancellationToken);

        _logger.LogInformation("Finished processing UserRegisteredIntegrationEvent for user {UserId}", message.UserId);
    }
}
