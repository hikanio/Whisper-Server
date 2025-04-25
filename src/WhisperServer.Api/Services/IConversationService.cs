using WhisperServer.Api.Dto;

namespace WhisperServer.Api.Services;

public interface IConversationService
{
    Task<IEnumerable<ConversationDto>> GetAll(Guid contextUserId);
    Task<IEnumerable<MessageDto>> GetAllMessages(Guid contextUserId, Guid recipientId);
}