using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class UserOperationClaimConfiguration : IEntityTypeConfiguration<UserOperationClaim>
{
    public void Configure(EntityTypeBuilder<UserOperationClaim> builder)
    {
        builder.ToTable("UserOperationClaims");

        builder.Property(u => u.OperationClaimId).HasColumnName("OperationClaimId");
        builder.Property(u => u.UserId).HasColumnName("UserId");

        builder.HasKey(u => new { u.OperationClaimId, u.UserId });

        builder.HasOne(u => u.User)
            .WithOne()
            .HasForeignKey<UserOperationClaim>(u => u.UserId);

        builder.HasOne(u => u.OperationClaim).WithOne().HasForeignKey<UserOperationClaim>(uoc => uoc.OperationClaimId);
    }
}