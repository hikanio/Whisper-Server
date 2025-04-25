using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.Exceptions;

public sealed class InvalidMessageStatusException : CustomException
{
    public InvalidMessageStatusException(string messageStatus)
        : base($"Status: {messageStatus} is invalid. Try {MessageStatus.Sent}, {MessageStatus.Read}.")
    {
    }
}