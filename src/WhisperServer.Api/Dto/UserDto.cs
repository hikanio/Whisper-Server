namespace WhisperServer.Api.Dto;

public class UserDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Email { get; set; }
    public string PublicKey { get; set; }
}