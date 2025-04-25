using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhisperServer.Api.Entities;

namespace WhisperServer.Api.DatabaseContext.Configurations;

public sealed class KeyConfiguration : IEntityTypeConfiguration<Key>
{
    public void Configure(EntityTypeBuilder<Key> builder)
    {
        builder.ToTable("Keys");
        builder.HasKey(x => x.Id);

        builder.HasIndex(x => x.UserId).IsUnique();
        
        builder.Property(x => x.PublicKey).IsRequired();
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
    }
}