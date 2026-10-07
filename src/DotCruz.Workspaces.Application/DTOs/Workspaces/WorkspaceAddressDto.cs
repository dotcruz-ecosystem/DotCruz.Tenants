namespace DotCruz.Workspaces.Application.DTOs.Workspaces;

public sealed record WorkspaceAddressDto(
    string Street,
    string Number,
    string? Complement,
    string Neighborhood,
    string City,
    string State,
    string ZipCode
);
