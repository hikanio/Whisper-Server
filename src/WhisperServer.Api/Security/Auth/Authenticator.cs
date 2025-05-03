using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using WhisperServer.Api.Dto;

namespace WhisperServer.Api.Security.Auth;

/// <summary>
/// Provides authentication-related services, including the generation of JWT tokens.
/// </summary>
public class Authenticator : IAuthenticator
{
    private readonly string _issuer;
    private readonly string _audience;
    private readonly TimeSpan _expiry;
    private readonly SigningCredentials _signingCredentials;
    private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new();


    public Authenticator(IOptions<AuthOptions> options)
    {
        _issuer = options.Value.Issuer;
        _audience = options.Value.Audience;
        _expiry = options.Value.Expiry ?? TimeSpan.FromHours(1);
        _signingCredentials =
            new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.Value.SigningKey)),
                SecurityAlgorithms.HmacSha256);
    }
    
    /// <summary>
    /// Creates a JWT token for a user with the specified user ID, username, and email.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <param name="username">The username associated with the user.</param>
    /// <param name="email">The email address associated with the user.</param>
    /// <returns>A <see cref="JwtDto"/> containing the generated access token.</returns>
    public JwtDto CreateToken(Guid userId, string username, string email)
    {
        var now = DateTime.UtcNow;
        var expires = now.Add(_expiry);
        
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Name, username),
            new(JwtRegisteredClaimNames.UniqueName, email)
        };

        var jwt = new JwtSecurityToken(_issuer, _audience, claims, now, expires, _signingCredentials);
        var accessToken = _jwtSecurityTokenHandler.WriteToken(jwt);

        return new JwtDto()
        {
            UserId = userId.ToString(),
            Username = username,
            AccessToken = accessToken
        };
    }
}