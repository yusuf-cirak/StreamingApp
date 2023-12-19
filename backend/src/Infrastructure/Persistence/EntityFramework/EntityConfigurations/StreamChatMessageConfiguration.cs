using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stream = Domain.Entities.Stream;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class StreamChatMessageConfiguration : EntityConfiguration<StreamChatMessage>
{
    public override void Configure(EntityTypeBuilder<StreamChatMessage> builder)
    {
        base.Configure(builder);

        builder.ToTable("StreamChatMessages");

        builder.Property(u => u.StreamerId).HasColumnName("StreamerId");

        builder.Property(u => u.Message).HasColumnName("Message").IsRequired();

        builder.Property(u => u.CreatedDate).HasColumnName("CreatedAt").IsRequired();

        builder.HasOne(u => u.Stream).WithOne().HasForeignKey<StreamChatMessage>(u => u.StreamerId);

        builder.HasOne(u => u.User).WithOne().HasForeignKey<StreamChatMessage>(u => u.UserId);

        builder.HasOne(u => u.Streamer).WithOne().HasForeignKey<StreamChatMessage>(u => u.StreamerId);
    }
}