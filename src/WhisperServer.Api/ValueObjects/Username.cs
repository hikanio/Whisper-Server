using WhisperServer.Api.Exceptions;

namespace WhisperServer.Api.ValueObjects;

public sealed record Username
{
    public string Value { get; }
    
    public const int MaxLength = 40;
    public const int MinLength = 3;
    
    public Username(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidUsernameException(value);
        }
        
        if (value.Length is >= MaxLength or <= MinLength)
        {
            throw new InvalidUsernameException(value);
        }
        
        Value = value;
    }

    public static implicit operator string(Username username) => username.Value;
    
    public static implicit operator Username(string username) => new(username);
    
    public override string ToString() => Value;
}