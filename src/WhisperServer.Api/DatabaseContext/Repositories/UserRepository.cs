using Microsoft.EntityFrameworkCore;
using WhisperServer.Api.Entities;
using WhisperServer.Api.Repositories;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.DatabaseContext.Repositories;

/// <summary>
/// PostgreSQL repository for <see cref="User"/>.
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly WhisperServerDbContext _dbContext;

    public UserRepository(WhisperServerDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Get a <see cref="User"/> by ID without an associated <see cref="Key"/>. 
    /// </summary>
    /// <param name="id">The user's ID.</param>
    /// <returns>The <see cref="User"/> object, or <c>null</c> if not found.</returns>
    public async Task<User> GetByIdAsync(Guid id)
        => await _dbContext.Users.FindAsync(id);

    /// <summary>
    /// Retrieves a <see cref="User"/> by their email address.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>The <see cref="User"/> object, or <c>null</c> if not found.</returns>
    public Task<User> GetByEmailAsync(Email email)
        => _dbContext.Users
            .SingleOrDefaultAsync(x => x.Email == email);

    /// <summary>
    /// Retrieves a <see cref="User"/> by their email address, including their associated <see cref="Key"/>.
    /// </summary>
    /// <param name="email">The user's email address.</param>
    /// <returns>The <see cref="User"/> object with included <see cref="Key"/>, or <c>null</c> if not found.</returns>
    public Task<User> GetByEmailWithKeyAsync(Email email)
        => _dbContext.Users
            .Include(x => x.Key)
            .SingleOrDefaultAsync(x => x.Email == email);

    /// <summary>
    /// Adds a new <see cref="User"/> to the database.
    /// </summary>
    /// <param name="user">The user entity to add.</param>
    /// <returns></returns>
    public async Task AddAsync(User user)
    {
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing <see cref="User"/> in the database.
    /// </summary>
    /// <param name="user">The user entity with updated data.</param>
    /// <returns></returns>
    public async Task UpdateAsync(User user)
    {
        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();
    }

    /// <summary>
    /// Searches for users whose username or email contains the specified query string.
    /// </summary>
    /// <param name="query">The partial string to search for in username or email.</param>
    /// <param name="limitResults">Optional limit for the number of results. Defaults to 5 if not provided.</param>
    /// <returns>A list of <see cref="User"/> objects matching the query.</returns>
    public async Task<IEnumerable<User>> GetAllMatchingByQueryAsync(string query, int? limitResults)
    {
        var keyword = $"%{query}%";
        return await _dbContext.Users
            .Include(x => x.Key)
            .AsNoTracking()
            .Where(x =>
                EF.Functions.Like(x.Username, keyword) ||
                EF.Functions.Like(x.Email, keyword))
            .Take(limitResults ?? 5)
            .ToListAsync();
    }
}