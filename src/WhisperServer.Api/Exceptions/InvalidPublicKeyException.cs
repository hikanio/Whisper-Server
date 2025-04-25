namespace WhisperServer.Api.Exceptions;

public sealed class InvalidPublicKeyException : CustomException
{
    public InvalidPublicKeyException() : base("Public key is invalid.")
    {
    }
}