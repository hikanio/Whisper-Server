using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WhisperServer.Api.Commands;
using WhisperServer.Api.Repositories;
using WhisperServer.Api.Services;

namespace WhisperServer.Api.Hubs;

/// <summary>
/// A SignalR hub that facilitates real-time chat functionality for connected users.
/// This hub is responsible for managing the connection lifecycle, sending messages,
/// and updating message statuses.
/// </summary>
[Authorize]
public class ChatHub : Hub
{
    private readonly Guid _userId;
    private readonly IMessageRepository _messageRepository;
    private readonly IMessageService _messageService;

    public ChatHub(IMessageRepository messageRepository, IMessageService messageService)
    {
        var userId = Context.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId)) throw new HubException("Connection invalid.");

        _userId = Guid.Parse(userId);
        _messageRepository = messageRepository;
        _messageService = messageService;
    }

    public override async Task OnConnectedAsync()
    {
        var conversationIds = await _messageRepository.GetAllConversationIdsAsync(_userId);
        foreach (var id in conversationIds)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, id.ToString());
        }
        
        await Clients.Others.SendAsync("UserOnline", _userId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception exception)
    {
        await Clients.Others.SendAsync("UserOffline", _userId);   
        await base.OnDisconnectedAsync(exception);
    }

    public async Task SendMessage(CreateMessage command)
    {
        var messageId = await _messageService.Create(command, _userId);
        var message = await _messageService.Get(messageId, _userId);
        
        await Clients.Group(command.RecipientId.ToString()).SendAsync("ReceiveMessage", message);
    }

    public async Task UpdateMessageStatus(Guid recipientId, Guid messageId, UpdateMessageStatus command)
    {
        await _messageService.UpdateStatus(messageId, command);
        await Clients.Group(recipientId.ToString()).SendAsync("UpdateMessageStatus", messageId);
    }

    public async Task StartTyping(Guid recipientId)
    {
        await Clients.OthersInGroup(recipientId.ToString()).SendAsync("UserTyping", _userId.ToString());
    }

    public async Task StopTyping(Guid recipientId)
    {
        await Clients.OthersInGroup(recipientId.ToString()).SendAsync("UserStoppedTyping", _userId.ToString());
    }
}