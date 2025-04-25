using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Security.Auth;
using WhisperServer.Test.Integration.Helpers;

namespace WhisperServer.Test.Integration.Controllers;

[Collection("api")]
public abstract class BaseControllerTests : IClassFixture<OptionsProvider>
{
    private readonly Authenticator _authenticator;
    protected HttpClient Client { get; }

    protected BaseControllerTests(OptionsProvider optionsProvider)
    {
        var authOptions = optionsProvider.Get<AuthOptions>(AuthOptions.SectionName);
        _authenticator = new Authenticator(new OptionsWrapper<AuthOptions>(authOptions));
        
        var app = new WhisperServerTestApp();
        Client = app.Client;
    }

    protected JwtDto Authorize(Guid userId, string username, string email)
    {
        var jwt = _authenticator.CreateToken(userId, username, email);
        Client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, jwt.AccessToken);

        return jwt;
    }
}