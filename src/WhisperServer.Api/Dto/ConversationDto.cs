namespace WhisperServer.Api.Dto;

public class ConversationDto
{
    public Guid UserId { get; set; }
    public string Username { get; set; }
    public string PublicKey { get; set; }
    public MessageDto LastMessage { get; set; }
}