using WhisperServer.Api.Exceptions;

namespace WhisperServer.Api.ValueObjects;

public sealed record MessageStatus
{
    public string Value { get; }

    public const string Sent = nameof(Sent);
    public const string Read = nameof(Read);

    public MessageStatus(string value)
    {
        if (!string.Equals(value, Sent) && !string.Equals(value, Read))
        {
            throw new InvalidMessageStatusException(value);
        }
        
        Value = value;
    }
    
    public static implicit operator string(MessageStatus status) => status.Value;

    public static implicit operator MessageStatus(string status) => new(status);
}