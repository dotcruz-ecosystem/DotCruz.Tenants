namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceContactDto(
    string Email,
    int PhoneCountryCode,
    string PhoneNationalNumber
);