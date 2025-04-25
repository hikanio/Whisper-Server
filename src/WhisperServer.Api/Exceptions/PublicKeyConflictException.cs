using WhisperServer.Api.Exceptions.Abstractions.Http;

namespace WhisperServer.Api.Exceptions;

public sealed class PublicKeyConflictException : Status409Exception
{
    public PublicKeyConflictException() : base("Public key mismatch")
    {
    }
}