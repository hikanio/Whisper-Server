using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.Entities;

public class User
{
    public Guid Id { get; private set; }
    public Username Username { get; private set; }
    public Email Email { get; private set; }
    public Password Password { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public Key Key { get; private set; }
    public ICollection<Message> SentMessages { get; private set; } = new List<Message>();
    public ICollection<Message> ReceivedMessages { get; private set; } = new List<Message>();
    
    public User(Guid id, Username username, Email email, Password password, Key key, DateTime createdAt)
        : this (id, username, email, password, createdAt)
    {
        Key = key;
    }
    
    // EF constructor
    private User(Guid id, Username username, Email email, Password password, DateTime createdAt)
    {
        Id = id;
        Username = username;
        Email = email;
        Password = password;
        CreatedAt = createdAt;
    }
}