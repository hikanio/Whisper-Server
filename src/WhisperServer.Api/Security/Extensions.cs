using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Security.Auth;
using WhisperServer.Api.Utils;

namespace WhisperServer.Api.Security;

public static class Extensions
{
    private const string AllowAnyOriginPolicy = nameof(AllowAnyOriginPolicy);

    /// <summary>
    /// Adds security services to the dependency injection container, including authentication 
    /// configuration with JWT, authorization, and CORS policies.
    /// </summary>
    public static IServiceCollection AddSecurity(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<AuthOptions>(configuration.GetRequiredSection(AuthOptions.SectionName));
        var options = configuration.GetOptions<AuthOptions>(AuthOptions.SectionName);

        services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(x =>
            {
                x.Audience = options.Audience;
                x.TokenValidationParameters = new TokenValidationParameters()
                {
                    ValidIssuer = options.Issuer,
                    ClockSkew = TimeSpan.Zero,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SigningKey))
                };
            });

        services.AddCors(opt =>
        {
            opt.AddPolicy(name: AllowAnyOriginPolicy,
                policy =>
                {
                    policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
        });

        services
            .AddAuthorization()
            .AddSingleton<IAuthenticator, Authenticator>()
            .AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>()
            .AddSingleton<IPasswordManager, PasswordManager>();

        return services;
    }

    /// <summary>
    /// Configures security middleware for development environments by enabling CORS <see cref="AllowAnyOriginPolicy"/>, 
    /// authentication, and authorization.
    /// </summary>
    public static WebApplication UseSecurityDev(this WebApplication app)
    {
        app.UseCors(AllowAnyOriginPolicy);
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }

    /// <summary>
    /// Configures security middleware for production environments by enabling 
    /// authentication and authorization.
    /// </summary>
    public static WebApplication UseSecurityProd(this WebApplication app)
    {
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}