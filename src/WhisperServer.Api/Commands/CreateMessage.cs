namespace WhisperServer.Api.Commands;

public sealed record CreateMessage(Guid SenderId, Guid RecipientId, string SenderMessage, string RecipientMessage, string Status);