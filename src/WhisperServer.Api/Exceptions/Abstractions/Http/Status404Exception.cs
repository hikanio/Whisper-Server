namespace WhisperServer.Api.Exceptions.Abstractions.Http;

public abstract class Status404Exception : CustomException
{
    protected Status404Exception(string message) : base(message)
    {
    }
}