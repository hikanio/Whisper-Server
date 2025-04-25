using Microsoft.EntityFrameworkCore;
using WhisperServer.Api.DatabaseContext.Repositories;
using WhisperServer.Api.Repositories;
using WhisperServer.Api.Utils;

namespace WhisperServer.Api.DatabaseContext;

public static class Extensions
{
    /// <summary>
    /// Adds the <c>PostgreSQL</c> database connection and register repositories: <see cref="UserRepository"/>, <see cref="MessageRepository"/>.
    /// </summary>
    public static IServiceCollection AddDatabase(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<PostgresOptions>(configuration.GetRequiredSection(PostgresOptions.SectionName));
        var options = configuration.GetOptions<PostgresOptions>(PostgresOptions.SectionName);
        services.AddDbContext<WhisperServerDbContext>(x => x.UseNpgsql(options.ConnectionString));
        
        services
            .AddScoped<IUserRepository, UserRepository>()
            .AddScoped<IMessageRepository, MessageRepository>();

        return services;
    }
    
    /// <summary>
    /// Applies any pending EF Core migrations to the configured database at application startup.
    /// This ensures the database schema is up to date with the current model.
    /// Intended to be used in development or testing environments.
    /// </summary>
    /// <param name="app">The application builder instance used to access scoped services.</param>
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<WhisperServerDbContext>();
        db.Database.Migrate();
    }
}