using DotCruz.Workspaces.Domain.Exceptions.Resources;
using FluentValidation;

namespace DotCruz.Workspaces.Application.UseCases.Workspaces.Queries.GetUploadUrl;

public class GetUploadUrlQueryValidator : AbstractValidator<GetUploadUrlQuery>
{
    public GetUploadUrlQueryValidator()
    {
        RuleFor(x => x.WorkspaceId)
            .NotEmpty().WithMessage(ResourceMessagesException.ID_EMPTY);

        RuleFor(x => x.Purpose)
            .IsInEnum().WithMessage("Invalid upload purpose.");

        RuleFor(x => x.FileName)
            .NotEmpty().WithMessage(ResourceMessagesException.FILENAME_EMPTY);

        RuleFor(x => x.ContentType)
            .NotEmpty().WithMessage(ResourceMessagesException.CONTENT_TYPE_EMPTY);
    }
}
