using Microsoft.EntityFrameworkCore;
using WhisperServer.Api.Dto;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Exceptions;
using WhisperServer.Api.Repositories;

namespace WhisperServer.Api.DatabaseContext.Repositories;

/// <summary>
/// PostgreSQL repository for <see cref="Message"/>.
/// </summary>
public class MessageRepository : IMessageRepository
{
    private readonly WhisperServerDbContext _dbContext;

    public MessageRepository(WhisperServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves a <see cref="Message"/> by ID.
    /// </summary>
    /// <param name="id">The message ID.</param>
    /// <returns>The <see cref="Message"/> entity, or <c>null</c> if not found.</returns>
    public async Task<Message> GetByIdAsync(Guid id)
        => await _dbContext.Messages.FindAsync(id);
    
    /// <summary>
    /// Gets all conversation participant IDs for the specified user.
    /// </summary>
    /// <param name="userId">The ID of the user whose conversations should be retrieved.</param>
    /// <returns>A list of user IDs that represent conversations.</returns>
    public async Task<IEnumerable<Guid>> GetAllConversationIdsAsync(Guid userId)
        => await _dbContext.Messages
            .Where(x => x.SenderId == userId || x.RecipientId == userId)
            .AsNoTracking()
            .Select(x => x.SenderId == userId ? x.RecipientId : x.SenderId)
            .ToListAsync();

    /// <summary>
    /// Retrieves all conversations for the specified user, including the latest message in each conversation.
    /// </summary>
    /// <param name="userId">The ID of the user whose conversations are to be retrieved.</param>
    /// <returns>A list of <see cref="ConversationDto"/> representing each conversation.</returns>
    /// <exception cref="UserNotFoundException">
    /// Thrown if a user in a conversation could not be found in the database.
    /// </exception>
    public async Task<IEnumerable<ConversationDto>> GetAllConversationsAsync(Guid userId)
    {
        var messages = await _dbContext.Messages.Where(x => x.SenderId == userId || x.RecipientId == userId)
            .OrderByDescending(x => x.UpdatedAt).AsNoTracking().ToListAsync();

        var conversations = messages.GroupBy(x => x.SenderId == userId ? x.RecipientId : x.SenderId).Select(x =>
            new ConversationDto()
            {
                UserId = x.Key,
                Username = _dbContext.Users.Find(x.Key)?.Username ?? throw new UserNotFoundException(x.Key.ToString()),
                PublicKey = _dbContext.Users.Include(u => u.Key).FirstOrDefault(u => u.Id == x.Key)?.Key.PublicKey ??
                            throw new UserNotFoundException(x.Key.ToString()),
                LastMessage = new MessageDto()
                {
                    Id = x.First().Id,
                    Username = _dbContext.Users.Find(x.First().SenderId)?.Username,
                    Message = x.First().SenderId == userId ? x.First().SenderMessage : x.First().RecipientMessage,
                    Status = x.First().Status,
                    UpdatedAt = x.First().UpdatedAt,
                }
            }).ToList();

        return conversations;
    }

    /// <summary>
    /// Retrieves all messages exchanged between two users.
    /// </summary>
    /// <param name="senderId">The ID of one participant.</param>
    /// <param name="recipientId">The ID of the other participant.</param>
    /// <returns>A list of <see cref="Message"/> objects in the conversation.</returns>
    public async Task<IEnumerable<Message>> GetAllMessagesByConversationAsync(Guid senderId, Guid recipientId)
        => await _dbContext.Messages.Where(x =>
                (x.SenderId == senderId && x.RecipientId == recipientId) ||
                (x.RecipientId == senderId && x.SenderId == recipientId))
            .AsNoTracking()
            .ToListAsync();
    
    /// <summary>
    /// Updates an existing <see cref="Message"/> in the database.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> entity with updated values.</param>
    /// <returns></returns>
    public async Task UpdateAsync(Message message)
    {
        _dbContext.Messages.Update(message);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Adds a new <see cref="Message"/> to the database.
    /// </summary>
    /// <param name="message">The <see cref="Message"/> entity to add.</param>
    /// <returns></returns>
    public async Task AddAsync(Message message)
    {
        await _dbContext.Messages.AddAsync(message);
        await _dbContext.SaveChangesAsync();
    }
}