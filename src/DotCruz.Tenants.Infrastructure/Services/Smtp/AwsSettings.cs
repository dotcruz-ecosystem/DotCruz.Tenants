namespace DotCruz.Tenants.Infrastructure.Services.Smtp;

public class AwsSettings
{
    public const string SectionName = "Settings:AWS";
    public string Region { get; set; } = string.Empty;
    public string AccessKey { get; set; } = string.Empty;
    public string SecretKey { get; set; } = string.Empty;
    public string SmtpParameterPath { get; set; } = string.Empty;
}
