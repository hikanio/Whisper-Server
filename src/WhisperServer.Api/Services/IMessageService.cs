using WhisperServer.Api.Commands;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;

namespace WhisperServer.Api.Services;

public interface IMessageService
{
    Task<Guid> Create(CreateMessage command, Guid contextUserId);
    Task<MessageDto> Get(Guid id, Guid contextUserId);
    Task<Message> UpdateStatus(Guid id, UpdateMessageStatus command);
}