using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Communication;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace DotCruz.Workspaces.Application.Mappers.Workspaces;

public static class WorkspaceContactMapper
{
    public static WorkspaceContactDto ToDto(this WorkspaceContact WorkspaceContact)
    {
        return new WorkspaceContactDto(
            WorkspaceContact.Email,
            WorkspaceContact.Phone.CountryCode,
            WorkspaceContact.Phone.NationalNumber
        );
    }
    public static WorkspaceContact ToDomain(this WorkspaceContactDto WorkspaceContactDto)
    {
        return WorkspaceContact.Create(
            Email.Create(WorkspaceContactDto.Email),
            PhoneNumber.Create(WorkspaceContactDto.PhoneCountryCode, WorkspaceContactDto.PhoneNationalNumber)
        );
    }
}
