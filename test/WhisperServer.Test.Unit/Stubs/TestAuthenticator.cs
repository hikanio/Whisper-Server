using WhisperServer.Api.Dto;
using WhisperServer.Api.Security.Auth;

namespace WhisperServer.Test.Unit.Stubs;

public class TestAuthenticator : IAuthenticator
{
    public JwtDto CreateToken(Guid userId, string username, string email)
    {
        return new JwtDto(){ AccessToken = "access token"};
    }
}