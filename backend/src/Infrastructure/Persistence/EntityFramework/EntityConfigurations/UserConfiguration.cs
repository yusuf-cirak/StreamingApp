using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public sealed class UserConfiguration : AuditableEntityConfiguration<User>
    {
        public override void Configure(EntityTypeBuilder<User> builder)
        {
            base.Configure(builder);

            builder.ToTable("Users");

            builder.Property(u => u.Username).HasColumnName("Username").IsRequired();

            builder.HasIndex(u => u.Username).IsUnique();

            builder.Property(u => u.PasswordSalt).HasColumnName("PasswordSalt").IsRequired();

            builder.Property(u => u.PasswordHash).HasColumnName("PasswordHash").IsRequired();

            builder.HasMany(u => u.UserRoleClaims).WithOne(uop => uop.User).HasForeignKey(uop => uop.UserId);

            builder.HasMany(u => u.RefreshTokens).WithOne(rt => rt.User).HasForeignKey(rt => rt.UserId);
        }
    }
}