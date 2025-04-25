using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;

namespace WhisperServer.Api.Repositories;

public interface IMessageRepository
{
    Task<Message> GetByIdAsync(Guid id);
    Task<IEnumerable<Guid>> GetAllConversationIdsAsync(Guid userId);
    Task<IEnumerable<ConversationDto>> GetAllConversationsAsync(Guid userId);
    Task<IEnumerable<Message>> GetAllMessagesByConversationAsync(Guid senderId, Guid recipientId);
    Task UpdateAsync(Message message);
    Task AddAsync(Message message);
}