﻿namespace WhisperServer.Api.Exceptions;

public sealed class InvalidEmailException : CustomException
{
    public string Email { get; }

    public InvalidEmailException(string email) : base($"Email: {email} is invalid.")
        => Email = email;
}