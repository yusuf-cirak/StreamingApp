using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class StreamBlockedUserConfiguration : IEntityTypeConfiguration<StreamBlockedUser>
{
    public void Configure(EntityTypeBuilder<StreamBlockedUser> builder)
    {
        builder.ToTable("StreamBlockedUsers");

        builder.Property(u => u.StreamerId).HasColumnName("StreamerId");
        builder.Property(u => u.UserId).HasColumnName("UserId");

        builder.HasKey(u => new { u.StreamerId, u.UserId });
    }
}