namespace DotCruz.Workspaces.Application.DTOs.Base;

public sealed record ErrorResponseDto
{
    public IEnumerable<string> Errors { get; }

    public ErrorResponseDto(IEnumerable<string> errors)
    {
        Errors = errors;
    }

    public ErrorResponseDto(string error)
    {
        Errors = [error];
    }
}
