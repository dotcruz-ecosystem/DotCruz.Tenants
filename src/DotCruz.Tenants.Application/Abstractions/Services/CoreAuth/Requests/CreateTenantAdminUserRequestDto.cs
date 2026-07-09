namespace DotCruz.Tenants.Application.Abstractions.Services.CoreAuth.Requests;

public sealed record CreateTenantAdminUserRequestDto
{
    private const int ADMIN_USER_TYPE = 2;

    public string Name { get; private set; }
    public string Email { get; private set; }
    public Guid TenantId { get; private set; }
    public int Type { get; } = ADMIN_USER_TYPE;

    public CreateTenantAdminUserRequestDto(string name, string email, Guid tenantId)
    {
        Name = name;
        Email = email;
        TenantId = tenantId;
    }
}
