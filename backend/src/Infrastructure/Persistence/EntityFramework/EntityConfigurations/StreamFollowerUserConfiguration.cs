using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class StreamFollowerUserConfiguration : IEntityTypeConfiguration<StreamFollowerUser>
{
    public void Configure(EntityTypeBuilder<StreamFollowerUser> builder)
    {
        builder.ToTable("StreamFollowerUsers");

        builder.Property(u => u.StreamerId).HasColumnName("StreamerId");
        builder.Property(u => u.UserId).HasColumnName("UserId");

        builder.HasKey(u => new { u.StreamerId, u.UserId });
    }
}