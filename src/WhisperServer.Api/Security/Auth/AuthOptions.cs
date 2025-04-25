namespace WhisperServer.Api.Security.Auth;

public sealed record AuthOptions
{
    public const string SectionName = "auth";
    
    public string Issuer { get; init; }
    public string Audience { get; init; }
    public string SigningKey { get; init; }
    public TimeSpan? Expiry { get; init; }
}