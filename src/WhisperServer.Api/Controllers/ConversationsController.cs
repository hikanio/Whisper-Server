using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Services;

namespace WhisperServer.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
[ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status400BadRequest, "application/json")]
public class ConversationsController : ControllerBase
{
    private readonly Guid _userId;
    private readonly IConversationService _conversationService;

    public ConversationsController(IConversationService conversationService, IHttpContextAccessor httpContextAccessor)
    {
        _userId = Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                             ?? throw new InvalidCredentialsException());
        _conversationService = conversationService;
    }

    [HttpGet]
    [EndpointSummary("Get all conversations associated with the authenticated user.")]
    [ProducesResponseType<IEnumerable<ConversationDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IEnumerable<ConversationDto>>> Get()
        => Ok(await _conversationService.GetAll(_userId));
    
    [HttpGet("{id:guid}/messages")]
    [EndpointSummary("Get all messages from a specific conversation.")]
    [ProducesResponseType<IEnumerable<MessageDto>>(StatusCodes.Status200OK, "application/json")]
    public async Task<ActionResult<IEnumerable<MessageDto>>> Get(Guid id)
        => Ok(await _conversationService.GetAllMessages(_userId, id));
}