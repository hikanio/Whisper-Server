using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.Entities;

public class Message
{
    public Guid Id { get; private set; }
    public Guid SenderId { get; private set; }
    public Guid RecipientId { get; private set; }
    public MessageContent SenderMessage { get; private set; }
    public MessageContent RecipientMessage { get; private set; }
    public MessageStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Message(Guid id, Guid senderId, Guid recipientId, MessageContent senderMessage, MessageContent recipientMessage, MessageStatus status, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        SenderId = senderId;
        RecipientId = recipientId;
        SenderMessage = senderMessage;
        RecipientMessage = recipientMessage;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void UpdateStatus(MessageStatus status)
    {
        Status = status;
        UpdatedAt = DateTime.UtcNow;
    }
}