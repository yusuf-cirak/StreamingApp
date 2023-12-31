﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class RoleOperationClaimConfiguration : IEntityTypeConfiguration<RoleOperationClaim>
{
    public void Configure(EntityTypeBuilder<RoleOperationClaim> builder)
    {
        builder.ToTable("RoleOperationClaims");

        builder.Property(r => r.RoleId).HasColumnName("RoleId");
        builder.Property(r => r.OperationClaimId).HasColumnName("OperationClaimId");

        builder.HasKey(r => new { r.RoleId, r.OperationClaimId });
        builder.HasIndex(r => r.RoleId).IsUnique(false);
        builder.HasIndex(r => new { r.RoleId, r.OperationClaimId }).IsUnique();

        builder.HasOne(u => u.OperationClaim).WithOne().HasForeignKey<RoleOperationClaim>(u => u.OperationClaimId);

        builder.HasOne(u => u.Role).WithOne().HasForeignKey<RoleOperationClaim>(u => u.RoleId);
    }
}