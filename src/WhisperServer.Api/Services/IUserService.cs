using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;

namespace WhisperServer.Api.Services;

public interface IUserService
{
    Task SignUp(SignUp command);
    Task<JwtDto> SignIn(SignIn command);
    Task<IEnumerable<UserDto>> FindAllByUsernameOrEmail(string name, int? limitResults);
}