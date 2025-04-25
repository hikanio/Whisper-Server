namespace WhisperServer.Api.Commands;

public sealed record SignUp(string Username, string Email, string Password, string PublicKey);