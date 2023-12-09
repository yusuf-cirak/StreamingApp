using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public sealed class RefreshTokenConfiguration : EntityConfiguration<RefreshToken>
    {
        public override void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            base.Configure(builder);

            builder.ToTable("RefreshTokens");

            builder.Property(rt => rt.UserId).HasColumnName("UserId").IsRequired();

            builder.Property(rt => rt.Token).HasColumnName("Token").IsRequired();

            builder.Property(rt => rt.ExpiresAt).HasColumnName("ExpiresAt").IsRequired();

            builder.Property(rt => rt.CreatedByIp).HasColumnName("CreatedByIp").IsRequired();

            builder.HasOne(rt => rt.User).WithMany(u => u.RefreshTokens).HasForeignKey(rt => rt.UserId);
        }
    }
}
