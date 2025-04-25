using Microsoft.Extensions.Configuration;
using WhisperServer.Api.Utils;

namespace WhisperServer.Test.Integration.Helpers;

public class OptionsProvider
{
    private readonly IConfiguration _configuration;

    public OptionsProvider()
    {
        _configuration = GetConfigurationRoot();
    }

    public T Get<T>(string sectionName) where T : class, new()
        => _configuration.GetOptions<T>(sectionName);
    
    private static IConfigurationRoot GetConfigurationRoot()
        => new ConfigurationBuilder()
            .AddJsonFile("appsettings.Test.json", false)
            .AddEnvironmentVariables()
            .Build();
}