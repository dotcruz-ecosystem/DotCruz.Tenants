using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;
using System;
using System.Text.RegularExpressions;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Commands.UpdateTenantBranding;

public partial class UpdateTenantBrandingCommandValidator : AbstractValidator<UpdateTenantBrandingCommand>
{
    public UpdateTenantBrandingCommandValidator()
    {
        RuleFor(x => x.Id)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.TenantBranding)
            .NotNull().WithMessage(ResourceMessagesException.BRANDING_REQUIRED);

        When(x => x.TenantBranding != null, () =>
        {
            RuleFor(x => x.TenantBranding.LogoUrl)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.TENANT_LOGO_URL_EMPTY)
                .Must(BeValidUrl)
                .WithMessage(ResourceMessagesException.TENANT_LOGO_URL_INVALID);

            RuleFor(x => x.TenantBranding.Website)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.TENANT_WEBSITE_URL_EMPTY)
                .Must(BeValidUrl)
                .WithMessage(ResourceMessagesException.TENANT_WEBSITE_URL_INVALID);

            RuleFor(x => x.TenantBranding.UnsubscribeUrl)
                .NotEmpty()
                .WithMessage(ResourceMessagesException.UNSUBSCRIBE_URL_EMPTY)
                .Must(BeValidUrl)
                .WithMessage(ResourceMessagesException.UNSUBSCRIBE_URL_INVALID);

            RuleFor(x => x.TenantBranding.HeaderBackgroundColor)
                .Must(BeValidHexColor)
                .WithMessage(ResourceMessagesException.HEADER_BACKGROUND_COLOR_INVALID)
                .When(x => !string.IsNullOrEmpty(x.TenantBranding.HeaderBackgroundColor));
        });
    }

    private static bool BeValidUrl(string url)
    {
        return Uri.TryCreate(url, UriKind.Absolute, out var result) 
            && (result.Scheme == Uri.UriSchemeHttp || result.Scheme == Uri.UriSchemeHttps);
    }

    private static bool BeValidHexColor(string hexColor)
    {
        return HexColorRegex().IsMatch(hexColor);
    }

    [GeneratedRegex(@"^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", RegexOptions.Compiled)]
    private static partial Regex HexColorRegex();
}
