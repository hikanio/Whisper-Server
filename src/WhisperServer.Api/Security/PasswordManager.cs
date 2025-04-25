using Microsoft.AspNetCore.Identity;
using WhisperServer.Api.Entities;

namespace WhisperServer.Api.Security;

/// <summary>
/// Provides methods for securely hashing and verifying user passwords.
/// </summary>
public sealed class PasswordManager : IPasswordManager
{
    private readonly IPasswordHasher<User> _passwordHasher;

    public PasswordManager(IPasswordHasher<User> passwordHasher)
    {
        _passwordHasher = passwordHasher;
    }

    /// <summary>
    /// Hashes the provided password to securely store it.
    /// </summary>
    /// <param name="password">The plaintext password to be hashed.</param>
    /// <returns>A hashed version of the provided password.</returns>
    public string Secure(string password)
        => _passwordHasher.HashPassword(default, password);

    /// <summary>
    /// Validates a plaintext password against a previously secured (hashed) password.
    /// </summary>
    /// <param name="password">The plaintext password to validate.</param>
    /// <param name="securedPassword">The previously secured (hashed) password to compare against.</param>
    /// <returns><c>true</c> if the passwords match; otherwise, <c>false</c>.</returns>
    public bool Validate(string password, string securedPassword)
        => _passwordHasher.VerifyHashedPassword(default, securedPassword, password) ==
           PasswordVerificationResult.Success;
}