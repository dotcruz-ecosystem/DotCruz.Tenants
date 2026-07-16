namespace DotCruz.Tenants.Infrastructure.Services.Storage;

public class CloudflareR2Settings
{
    public const string SectionName = "Settings:CloudflareR2";
    public string AccountId { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string BucketName { get; set; } = string.Empty;
    public string PublicBaseUrl { get; set; } = string.Empty;
}
