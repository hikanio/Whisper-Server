using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Hubs;
using WhisperServer.Api.Services;

namespace WhisperServer.Api.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
[ProducesResponseType(typeof(void),StatusCodes.Status401Unauthorized)]
[ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status400BadRequest, "application/json")]
public class MessagesController : ControllerBase
{
    private readonly Guid _userId;
    private readonly IMessageService _messageService;
    private readonly IHubContext<ChatHub> _hubContext;

    public MessagesController(IMessageService messageService, IHttpContextAccessor httpContextAccessor, IHubContext<ChatHub> hubContext)
    {
        _userId = Guid.Parse(httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
                             ?? throw new InvalidCredentialsException());
        _messageService = messageService;
        _hubContext = hubContext;
    }
    
    [HttpPost]
    [EndpointSummary("Create a new message.")]
    [ProducesResponseType<MessageDto>(StatusCodes.Status201Created, "application/json")]
    [ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<ActionResult<MessageDto>> Post(CreateMessage command)
    {
        var id = await _messageService.Create(command, _userId);
        var message = await _messageService.Get(id, _userId);
        
        await _hubContext.Clients.Group(command.RecipientId.ToString()).SendAsync("ReceiveMessage", message);
        return CreatedAtAction(nameof(Get), new{id}, message);
    }
    
    [HttpGet("{id:guid}")]
    [EndpointSummary("Get information about a specific message.")]
    [ProducesResponseType<MessageDto>(StatusCodes.Status200OK, "application/json")]
    [ProducesResponseType<ExceptionMiddleware.Error>(StatusCodes.Status404NotFound, "application/json")]
    public async Task<ActionResult<MessageDto>> Get(Guid id) 
        => Ok(await _messageService.Get(id, _userId));
    
    [HttpPatch("{id:guid}")]
    [EndpointSummary("Update message status.")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    public async Task<ActionResult> Patch(Guid id, UpdateMessageStatus command)
    {
        var message = await _messageService.UpdateStatus(id, command);
        
        await _hubContext.Clients.Group(message.RecipientId.ToString()).SendAsync("UpdateMessageStatus", id);
        return NoContent();
    }
}