using WhisperServer.Api.Exceptions;

namespace WhisperServer.Api.Entities;

public class Key
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public string PublicKey { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public Key(Guid id, Guid userId, string publicKey, DateTime createdAt, DateTime updatedAt)
    {
        Id = id;
        UserId = userId;
        PublicKey = publicKey;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }

    public void Update(string publicKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
        {
            throw new InvalidPublicKeyException();
        }
        PublicKey = publicKey;
        UpdatedAt = DateTime.UtcNow;
    }
}