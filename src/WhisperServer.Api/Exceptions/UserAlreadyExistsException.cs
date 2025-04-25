using WhisperServer.Api.Exceptions.Abstractions.Http;

namespace WhisperServer.Api.Exceptions;

public sealed class UserAlreadyExistsException : Status409Exception
{
    public string Email { get; }

    public UserAlreadyExistsException(string email) : base($"User: {email} already exists.")
        => Email = email;
}