using Domain.Entities;
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
            builder.HasKey(u => new { u.RoleId, u.UserId });

            builder.HasOne(u => u.User).WithOne().HasForeignKey<UserRoleClaim>(u => u.UserId);

            builder.HasOne(u => u.Role).WithOne().HasForeignKey<UserRoleClaim>(u => u.RoleId);
        }
    }
}