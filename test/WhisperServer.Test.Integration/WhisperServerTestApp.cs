using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using WhisperServer.Api;

namespace WhisperServer.Test.Integration;

public class WhisperServerTestApp : WebApplicationFactory<Program>
{
    public HttpClient Client { get; }
    
    public WhisperServerTestApp()
    {
        Client = WithWebHostBuilder(builder =>
        {
            builder.UseEnvironment("Test");
        }).CreateClient();
    }
}