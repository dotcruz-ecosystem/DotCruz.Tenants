using DotCruz.Tenants.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Tenants.Application.UseCases.Tenants.Queries.GetUploadUrl;

public class GetUploadUrlQueryValidator : AbstractValidator<GetUploadUrlQuery>
{
    public GetUploadUrlQueryValidator()
    {
        RuleFor(x => x.TenantId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Purpose)
            .IsInEnum().WithMessage("Invalid upload purpose.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage(ResourceMessagesException.FILENAME_EMPTY);

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage(ResourceMessagesException.CONTENT_TYPE_EMPTY);
    }
}
