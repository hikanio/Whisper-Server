using Microsoft.EntityFrameworkCore;
using WhisperServer.Api.DatabaseContext;

namespace WhisperServer.Test.Integration.Helpers;

public sealed class TestDatabase : IDisposable
{
    public WhisperServerDbContext DbContext { get; }

    public TestDatabase(OptionsProvider optionsProvider)
    {
        var options = optionsProvider.Get<PostgresOptions>(PostgresOptions.SectionName);
        DbContext = new WhisperServerDbContext(new DbContextOptionsBuilder<WhisperServerDbContext>()
            .UseNpgsql(options.ConnectionString).Options);
    }

    public void Dispose()
    {
        DbContext.Database.EnsureDeleted();
        DbContext.Dispose();
    }
}