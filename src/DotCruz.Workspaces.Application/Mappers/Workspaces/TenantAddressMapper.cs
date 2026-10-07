using DotCruz.Workspaces.Application.DTOs.Workspaces;
using DotCruz.Workspaces.Domain.ValueObjects.Location;
using DotCruz.Workspaces.Domain.ValueObjects.Workspaces;

namespace DotCruz.Workspaces.Application.Mappers.Workspaces;

public static class WorkspaceAddressMapper
{
    public static WorkspaceAddressDto ToDto(this WorkspaceAddress WorkspaceAddress)
    {
        return new WorkspaceAddressDto(
            WorkspaceAddress.Street,
            WorkspaceAddress.Number,
            WorkspaceAddress.Complement,
            WorkspaceAddress.Neighborhood,
            WorkspaceAddress.City,
            WorkspaceAddress.State,
            WorkspaceAddress.ZipCode
        );
    }
    public static WorkspaceAddress ToDomain(this WorkspaceAddressDto WorkspaceAddressDto)
    {
        return WorkspaceAddress.Create(
            WorkspaceAddressDto.Street,
            WorkspaceAddressDto.Number,
            WorkspaceAddressDto.Complement,
            WorkspaceAddressDto.Neighborhood,
            WorkspaceAddressDto.City,
            State.Create(WorkspaceAddressDto.State),
            ZipCode.Create(WorkspaceAddressDto.ZipCode)
        );
    }
}
