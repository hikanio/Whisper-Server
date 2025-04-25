using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Services;

namespace WhisperServer.Api.Controllers;

[ApiController]
[Route("[controller]")]
[ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status400BadRequest, "application/json")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }
    

    [HttpGet("search")]
    [Authorize]
    [EndpointSummary("Search users by partial username or email.")]
    [ProducesResponseType<IEnumerable<UserDto>>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<IEnumerable<UserDto>>> Get([FromQuery] string name, int? limitResults)
        => Ok(await _userService.FindAllByUsernameOrEmail(name, limitResults));

    [HttpPost("sign-up")]
    [EndpointSummary("Sign up new user.")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status409Conflict, "application/json")]
    public async Task<ActionResult> Post(SignUp command)
    {
        await _userService.SignUp(command);
        return Ok();
    }
    
    [HttpPost("sign-in")]
    [EndpointSummary("Sign in user.")]
    [ProducesResponseType<JwtDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status400BadRequest, "application/json")]
    [ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status409Conflict, "application/json")]
    public async Task<ActionResult<JwtDto>> Post(SignIn command)
        => Ok(await _userService.SignIn(command));
}
