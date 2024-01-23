using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Stream = Domain.Entities.Stream;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class StreamConfiguration : EntityConfiguration<Stream>
{
    public override void Configure(EntityTypeBuilder<Stream> builder)
    {
        base.Configure(builder);

        builder.ToTable("Streams");

        builder.Property(u => u.StreamerId).HasColumnName("StreamerId").IsRequired();
        builder.Property(u => u.StartedAt).HasColumnName("StartedAt").IsRequired();
        builder.Property(u => u.EndedAt).HasColumnName("EndedAt").IsRequired(false);

        builder.HasOne(u => u.Streamer).WithMany(e => e.Streams).HasForeignKey(u => u.StreamerId);
    }
}