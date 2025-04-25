using WhisperServer.Api.Exceptions.Abstractions.Http;

namespace WhisperServer.Api.Exceptions;

public sealed class UnauthorizedUserException : Status401Exception
{
    public UnauthorizedUserException() : base("User is unauthorized to perform action.")
    {
    }
}