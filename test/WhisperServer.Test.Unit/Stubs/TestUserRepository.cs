using WhisperServer.Api.Entities;
using WhisperServer.Api.Repositories;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Test.Unit.Stubs;

public class TestUserRepository : IUserRepository
{
    private readonly List<User> _users = [];
    public async Task<User> GetByIdAsync(Guid id)
    {
        await Task.CompletedTask;
        return _users.SingleOrDefault(x => x.Id == id);
    }

    public async Task<User> GetByEmailAsync(Email email)
    {
        await Task.CompletedTask;
        return _users.SingleOrDefault(x => x.Email == email);
    }

    public async Task<User> GetByEmailWithKeyAsync(Email email)
    {
        await Task.CompletedTask;
        return _users.SingleOrDefault(x => x.Email == email);
    }

    public async Task AddAsync(User user)
    {
        _users.Add(user);
        await Task.CompletedTask;
    }

    public async Task UpdateAsync(User user)
    {
        var oldUser = _users.SingleOrDefault(x => x.Id == user.Id);
        _users.Remove(oldUser);
        _users.Add(user);
        
        await Task.CompletedTask;
    }

    public async Task<IEnumerable<User>> GetAllMatchingByQueryAsync(string query, int? limitResults)
    {
        var users = _users.FindAll(x =>
            x.Username.ToString().Contains(query) ||
            x.Email.ToString().Contains(query))[..(limitResults ?? 0)];
        
        await Task.CompletedTask;
        return users;
    }
}