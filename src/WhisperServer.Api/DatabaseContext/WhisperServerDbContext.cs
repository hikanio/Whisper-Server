using Microsoft.EntityFrameworkCore;
using WhisperServer.Api.Entities;

namespace WhisperServer.Api.DatabaseContext;

public class WhisperServerDbContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Key> Keys { get; set; }
    public DbSet<Message> Messages { get; set; }


    public WhisperServerDbContext(DbContextOptions<WhisperServerDbContext> options) : base(options)
    {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(GetType().Assembly);
    }
}