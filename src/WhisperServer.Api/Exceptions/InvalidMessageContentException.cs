namespace WhisperServer.Api.Exceptions;

public class InvalidMessageContentException : CustomException
{
    public InvalidMessageContentException() : base("Invalid message content.")
    {
    }
}