using WhisperServer.Api.Entities;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.Repositories;

public interface IUserRepository
{
    Task<User> GetByIdAsync(Guid id);
    Task<User> GetByEmailAsync(Email email);
    Task<User> GetByEmailWithKeyAsync(Email email);
    Task AddAsync(User user);
    Task UpdateAsync(User user);
    Task<IEnumerable<User>> GetAllMatchingByQueryAsync(string query, int? limitResults);
}