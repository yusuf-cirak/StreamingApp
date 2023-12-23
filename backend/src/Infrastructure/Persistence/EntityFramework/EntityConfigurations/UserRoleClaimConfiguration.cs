using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public sealed class UserRoleClaimConfiguration : IEntityTypeConfiguration<UserRoleClaim>
    {
        public void Configure(EntityTypeBuilder<UserRoleClaim> builder)
        {
            builder.ToTable("UserRoleClaims");

            builder.Property(u => u.RoleId).HasColumnName("RoleId");
            builder.Property(u => u.UserId).HasColumnName("UserId");

            builder.HasKey(u => new { u.UserId, u.RoleId });
        }
    }
}