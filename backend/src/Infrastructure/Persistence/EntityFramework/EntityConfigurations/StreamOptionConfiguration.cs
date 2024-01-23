using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public sealed class StreamOptionConfiguration : EntityConfiguration<StreamOption>
    {
        public override void Configure(EntityTypeBuilder<StreamOption> builder)
        {
            builder.ToTable("StreamOptions");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("Id").IsRequired();

            builder.Property(u => u.StreamKey).HasColumnName("StreamKey").IsRequired();

            builder.Property(u => u.StreamTitle).HasColumnName("StreamTitle").IsRequired(false);

            builder.Property(u => u.StreamDescription).HasColumnName("StreamDescription").IsRequired(false);

            builder.Property(u => u.ChatDisabled).HasColumnName("ChatDisabled").IsRequired().HasDefaultValue(false);

            builder.Property(u => u.ChatDelaySecond).HasColumnName("ChatDelaySecond").IsRequired().HasDefaultValue(0);

            builder.HasOne(u => u.Streamer).WithOne().HasForeignKey<StreamOption>(u => u.Id);
        }
    }
}