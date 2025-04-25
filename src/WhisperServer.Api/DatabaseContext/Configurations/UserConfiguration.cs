using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhisperServer.Api.Entities;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.DatabaseContext.Configurations;

public sealed class UserConfiguration : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("Users");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.Username);
        builder.HasIndex(x => x.Email).IsUnique();
        
        builder.Property(x => x.Username)
            .HasConversion(x => x.Value, x => new Username(x))
            .IsRequired()
            .HasMaxLength(50);
        builder.Property(x => x.Email)
            .HasConversion(x => x.Value, x => new Email(x))
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(x => x.Password)
            .HasConversion(x => x.Value, x => new Password(x))
            .IsRequired()
            .HasMaxLength(200);
        builder.Property(x => x.CreatedAt).IsRequired();
        
        builder.HasOne(x => x.Key)
            .WithOne()
            .HasForeignKey<Key>(x => x.UserId)
            .IsRequired();
        builder.HasMany(x => x.SentMessages)
            .WithOne()
            .HasForeignKey(x => x.SenderId)
            .IsRequired();
        builder.HasMany(x => x.ReceivedMessages)
            .WithOne()
            .HasForeignKey(x => x.RecipientId)
            .IsRequired();
    }
}