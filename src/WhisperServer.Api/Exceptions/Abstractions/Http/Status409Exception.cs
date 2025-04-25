namespace WhisperServer.Api.Exceptions.Abstractions.Http;

public abstract class Status409Exception : CustomException
{
    protected Status409Exception(string message) : base(message)
    {
    }
}