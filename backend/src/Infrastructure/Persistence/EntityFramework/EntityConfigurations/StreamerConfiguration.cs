using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public sealed class StreamerConfiguration : EntityConfiguration<Streamer>
    {
        public override void Configure(EntityTypeBuilder<Streamer> builder)
        {
            builder.ToTable("Streamers");

            builder.HasKey(u => u.Id);

            builder.Property(u => u.Id).HasColumnName("Id").IsRequired();
            
            builder.Property(u => u.StreamKey).HasColumnName("StreamKey").IsRequired();

            builder.Property(u => u.StreamTitle).HasColumnName("StreamTitle").IsRequired(false);

            builder.Property(u => u.StreamDescription).HasColumnName("StreamDescription").IsRequired(false);

            builder.HasOne(u => u.User).WithOne().HasForeignKey<Streamer>(u => u.Id);
        }
    }
}
