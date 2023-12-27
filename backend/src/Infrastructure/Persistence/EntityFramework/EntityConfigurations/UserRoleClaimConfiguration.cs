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

            builder.Property(u => u.Value).HasMaxLength(50).IsRequired();

            builder.HasKey(u => new { u.UserId, u.RoleId, u.Value });

            builder.HasIndex(r => r.UserId).IsUnique(false);

            builder.HasIndex(r => new { r.UserId, r.RoleId, r.Value }).IsUnique();

            builder.HasOne(u => u.User)
                .WithMany(e=>e.UserRoleClaims)
                .HasForeignKey(u => u.UserId);

            builder.HasOne(u => u.Role).WithOne().HasForeignKey<UserRoleClaim>(uoc => uoc.RoleId);
        }
    }
}