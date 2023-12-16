using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class RoleOperationClaimConfiguration : IEntityTypeConfiguration<RoleOperationClaim>
{
    public void Configure(EntityTypeBuilder<RoleOperationClaim> builder)
    {
        builder.ToTable("RoleOperationClaims");

        builder.Property(r => r.RoleId).HasColumnName("RoleId");
        builder.Property(r => r.OperationClaimId).HasColumnName("OperationClaimId");

        builder.HasKey(u => new { u.RoleId, u.OperationClaimId });

        builder.HasOne(u => u.OperationClaim).WithOne().HasForeignKey<RoleOperationClaim>(u => u.OperationClaimId);

        builder.HasOne(u => u.Role).WithOne().HasForeignKey<RoleOperationClaim>(u => u.RoleId);
    }
}