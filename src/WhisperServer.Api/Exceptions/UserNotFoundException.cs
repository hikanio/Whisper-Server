using WhisperServer.Api.Exceptions.Abstractions.Http;

namespace WhisperServer.Api.Exceptions;

public sealed class UserNotFoundException : Status404Exception
{
    public string Name { get; }

    public UserNotFoundException(string name) : base($"User: {name} not found")
        => Name = name;
}