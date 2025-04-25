﻿namespace WhisperServer.Api.Exceptions;

public sealed class InvalidPasswordException : CustomException
{
    public InvalidPasswordException() : base($"Password is invalid.")
    {
    }
}