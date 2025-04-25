namespace WhisperServer.Api.Exceptions.Abstractions.Http;

public abstract class Status401Exception : CustomException
{
    protected Status401Exception(string message) : base(message)
    {
    }
}