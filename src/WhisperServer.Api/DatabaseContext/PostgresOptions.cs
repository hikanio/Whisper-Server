namespace WhisperServer.Api.DatabaseContext;

public sealed class PostgresOptions
{
    public const string SectionName = "postgres";
    
    public string ConnectionString { get; init; }
}