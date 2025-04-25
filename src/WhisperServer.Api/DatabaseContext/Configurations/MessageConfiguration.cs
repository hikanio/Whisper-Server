using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using WhisperServer.Api.Entities;
using WhisperServer.Api.ValueObjects;

namespace WhisperServer.Api.DatabaseContext.Configurations;

public sealed class MessageConfiguration : IEntityTypeConfiguration<Message>
{
    public void Configure(EntityTypeBuilder<Message> builder)
    {
        builder.ToTable("Messages");
        builder.HasKey(x => x.Id);
        
        builder.HasIndex(x => x.SenderId);
        builder.HasIndex(x => x.RecipientId);
        builder.HasIndex(x => new { x.SenderId, x.RecipientId });
        
        builder.Property(x => x.SenderMessage)
            .HasConversion(x => x.Value, x => new MessageContent(x));
        builder.Property(x => x.RecipientMessage)
            .HasConversion(x => x.Value, x => new MessageContent(x));
        builder.Property(x => x.Status)
            .HasConversion(x => x.Value, x => new MessageStatus(x))
            .HasMaxLength(30);
        builder.Property(x => x.CreatedAt).IsRequired();
        builder.Property(x => x.UpdatedAt).IsRequired();
    }
}