using WhisperServer.Api.Dto;

namespace WhisperServer.Api.Security.Auth;

public interface IAuthenticator
{
    JwtDto CreateToken(Guid userId, string username, string email);
}