using WhisperServer.Api.Exceptions.Abstractions.Http;

namespace WhisperServer.Api.Exceptions;

public sealed class MessageNotFoundException : Status404Exception
{
    public string Name { get; }

    public MessageNotFoundException(string name) : base($"Message: {name} not found")
        => Name = name;
}