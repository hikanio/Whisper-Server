namespace WhisperServer.Api.Commands;

public sealed record SignIn(string Email, string Password, string PublicKey, bool OverwriteKey);