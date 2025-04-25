using WhisperServer.Api.Dto;
using WhisperServer.Api.Repositories;

namespace WhisperServer.Api.Services;

public class ConversationService : IConversationService
{
    private readonly IMessageRepository _messageRepository;
    private readonly IUserRepository _userRepository;

    public ConversationService(IMessageRepository messageRepository, IUserRepository userRepository)
    {
        _messageRepository = messageRepository;
        _userRepository = userRepository;
    }

    /// <summary>
    /// Retrieves all conversations for a given user.
    /// </summary>
    /// <param name="contextUserId">The ID of the user for whom to retrieve the conversations.</param>
    /// <returns>A collection of <see cref="ConversationDto"/> representing all conversations for the user.</returns>
    public async Task<IEnumerable<ConversationDto>> GetAll(Guid contextUserId)
        => await _messageRepository.GetAllConversationsAsync(contextUserId);

    /// <summary>
    /// Retrieves all messages for a specific conversation between the given user and a recipient.
    /// </summary>
    /// <param name="contextUserId">The ID of the user requesting the messages.</param>
    /// <param name="recipientId">The ID of the recipient with whom the conversation is held.</param>
    /// <returns>A collection of <see cref="MessageDto"/> representing all messages in the conversation.</returns>
    public async Task<IEnumerable<MessageDto>> GetAllMessages(Guid contextUserId, Guid recipientId)
    {
        var messages = await _messageRepository.GetAllMessagesByConversationAsync(contextUserId, recipientId);

        return messages.Select(x => new MessageDto()
        {
            Id = x.Id,
            Username = _userRepository.GetByIdAsync(x.SenderId).Result.Username,
            Message = x.SenderId == contextUserId ? x.SenderMessage : x.RecipientMessage,
            Status = x.Status,
            UpdatedAt = x.UpdatedAt
        }).ToList();
    }
}