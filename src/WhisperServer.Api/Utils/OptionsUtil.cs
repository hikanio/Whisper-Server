namespace WhisperServer.Api.Utils;

/// <summary>
/// Utility class providing extensions for working with configuration options.
/// </summary>
public static class OptionsUtil
{
    /// <summary>
    /// Binds a configuration section to a strongly-typed options class.
    /// </summary>
    /// <typeparam name="T">The type of the options class to bind to. Must have a parameterless constructor.</typeparam>
    /// <param name="configuration">The application configuration instance.</param>
    /// <param name="sectionName">The name of the configuration section to bind.</param>
    /// <returns>An instance of <typeparamref name="T"/> populated with values from the specified section.</returns>
    /// <exception cref="InvalidOperationException">Thrown if the section is not found in the configuration.</exception>
    public static T GetOptions<T>(this IConfiguration configuration, string sectionName) where T : class, new()
    {
        var options = new T();
        var section = configuration.GetRequiredSection(sectionName);
        section.Bind(options);

        return options;
    }
}