using WhisperServer.Api.Exceptions;

namespace WhisperServer.Api.ValueObjects;

public sealed record MessageContent
{
    public string Value { get; }
    
    public MessageContent(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidMessageContentException();
        }
        
        Value = value;
    }

    public static implicit operator string(MessageContent message) => message.Value;
    
    public static implicit operator MessageContent(string message) => new(message);
    
    public override string ToString() => Value;
}