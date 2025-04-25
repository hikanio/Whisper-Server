namespace WhisperServer.Api.Dto;

public class MessageDto
{
    public Guid Id { get; set; }
    public string Username { get; set; }
    public string Message { get; set; }
    public string Status { get; set; }
    public DateTime UpdatedAt { get; set; }
}