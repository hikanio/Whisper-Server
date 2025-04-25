namespace WhisperServer.Api.Exceptions;

public class InvalidQueryParameterException : CustomException
{
    public InvalidQueryParameterException(string message) : base(message)
    {
    }
}