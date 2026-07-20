using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.ConfigureTenantSmtp;

public class ConfigureTenantSmtpCommandValidator : AbstractValidator<ConfigureTenantSmtpCommand>
{
    public ConfigureTenantSmtpCommandValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Request)
            .NotNull().WithMessage(ResourceMessagesException.SMTP_SETTINGS_REQUIRED);

        When(x => x.Request != null, () =>
        {
            RuleFor(x => x.Request.Host)
                .NotEmpty().WithMessage(ResourceMessagesException.SMTP_HOST_EMPTY);

            RuleFor(x => x.Request.Port)
                .GreaterThan(0).WithMessage(ResourceMessagesException.SMTP_PORT_INVALID);

            RuleFor(x => x.Request.Username)
                .NotEmpty().WithMessage(ResourceMessagesException.SMTP_USERNAME_EMPTY);

            RuleFor(x => x.Request.Password)
                .NotEmpty().WithMessage(ResourceMessagesException.SMTP_PASSWORD_EMPTY);

            RuleFor(x => x.Request.FromName)
                .NotEmpty().WithMessage(ResourceMessagesException.SMTP_FROM_NAME_EMPTY);
        });
    }
}
