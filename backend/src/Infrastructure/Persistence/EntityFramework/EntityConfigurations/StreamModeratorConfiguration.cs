using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class StreamModeratorConfiguration : IEntityTypeConfiguration<StreamModerator>
{
    public void Configure(EntityTypeBuilder<StreamModerator> builder)
    {
        builder.ToTable("StreamModerators");

        builder.Property(u => u.StreamerId).HasColumnName("StreamerId");
        builder.Property(u => u.UserId).HasColumnName("UserId");

        builder.HasKey(u => new { u.StreamerId, u.UserId });

        builder.HasOne(u => u.Streamer)
            .WithOne()
            .HasForeignKey<StreamModerator>(u => u.StreamerId);

        builder.HasOne(u => u.User).WithOne().HasForeignKey<StreamModerator>(sm => sm.UserId);

        builder.HasOne(u => u.Streamer)
            .WithOne()
            .HasForeignKey<StreamModerator>(u => u.StreamerId);
    }
}