namespace WhisperServer.Api.Services;

public static class Extensions
{
    /// <summary>
    /// Adds application services to the dependency injection container, including: <see cref="UserService"/>, <see cref="MessageService"/>,
    /// and <see cref="ConversationService"/>.
    /// </summary>
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddScoped<IUserService, UserService>()
            .AddScoped<IMessageService, MessageService>()
            .AddScoped<IConversationService, ConversationService>();

        return services;
    }
}