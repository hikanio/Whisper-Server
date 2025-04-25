using WhisperServer.Api.Exceptions;

namespace WhisperServer.Api.ValueObjects;

public sealed record Password
{
    public string Value { get; }
    
    public Password(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length is > 200 or < 6)
        {
            throw new InvalidPasswordException();
        }
        
        Value = value;
    }

    public static implicit operator string(Password password) => password.Value;
    
    public static implicit operator Password(string password) => new(password);
    
    public override string ToString() => Value;
}