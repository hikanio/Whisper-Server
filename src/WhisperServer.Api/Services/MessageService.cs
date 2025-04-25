using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Repositories;

namespace WhisperServer.Api.Services;

public class MessageService : IMessageService
{
    private readonly IUserRepository _userRepository;
    private readonly IMessageRepository _messageRepository;

    public MessageService(IUserRepository userRepository, IMessageRepository messageRepository)
    {
        _userRepository = userRepository;
        _messageRepository = messageRepository;
    }
    
    /// <summary>
    /// Creates a new message between users.
    /// </summary>
    /// <param name="command">The command containing the message creation details.</param>
    /// <param name="contextUserId">The ID of the user making the request, used for authorization.</param>
    /// <returns>The ID of the created message.</returns>
    /// <exception cref="UserNotFoundException">Thrown if the sender or recipient user does not exist.</exception>
    /// <exception cref="UnauthorizedUserException">Thrown if the context user is not the sender.</exception>
    public async Task<Guid> Create(CreateMessage command, Guid contextUserId)
    {
        if (await _userRepository.GetByIdAsync(command.SenderId) is null)
        {
            throw new UserNotFoundException(command.SenderId.ToString());
        }
        if (await _userRepository.GetByIdAsync(command.RecipientId) is null)
        {
            throw new UserNotFoundException(command.RecipientId.ToString());
        }
        if (contextUserId != command.SenderId)
        {
            throw new UnauthorizedUserException();
        }

        var messageId = Guid.NewGuid();
        var message = new Message(
            messageId,
            command.SenderId,
            command.RecipientId,
            command.SenderMessage,
            command.RecipientMessage,
            command.Status,
            DateTime.UtcNow,
            DateTime.UtcNow
            );

        await _messageRepository.AddAsync(message);

        return messageId;
    }

    /// <summary>
    /// Retrieves a specific message by ID, visible only to the sender or recipient.
    /// </summary>
    /// <param name="id">The unique identifier of the message.</param>
    /// <param name="contextUserId">The ID of the user making the request.</param>
    /// <returns>The message details tailored to the requesting user.</returns>
    /// <exception cref="MessageNotFoundException">Thrown if the message does not exist.</exception>
    /// <exception cref="UnauthorizedUserException">Thrown if the user is neither the sender nor the recipient.</exception>
    public async Task<MessageDto> Get(Guid id, Guid contextUserId)
    {
        var message = await _messageRepository.GetByIdAsync(id);

        if (message is null) throw new MessageNotFoundException(id.ToString());
        if (contextUserId != message.SenderId && contextUserId != message.RecipientId) throw new UnauthorizedUserException();

        var isSender = message.SenderId == contextUserId;
        var user = await _userRepository.GetByIdAsync(isSender ? message.SenderId : message.RecipientId);
        
        return new MessageDto()
        {
            Id = message.Id,
            Username = user.Username,
            Message = isSender ? message.SenderMessage : message.RecipientMessage,
            Status = message.Status,
            UpdatedAt = message.UpdatedAt
        };
    }

    /// <summary>
    /// Updates the status of a message.
    /// </summary>
    /// <param name="id">The ID of the message to update.</param>
    /// <param name="command">The command containing the new status.</param>
    /// <returns>The updated message entity.</returns>

    public async Task<Message> UpdateStatus(Guid id, UpdateMessageStatus command)
    {
        var message = await _messageRepository.GetByIdAsync(id);
        message.UpdateStatus(command.Status);
        await _messageRepository.UpdateAsync(message);

        return message;
    }
}