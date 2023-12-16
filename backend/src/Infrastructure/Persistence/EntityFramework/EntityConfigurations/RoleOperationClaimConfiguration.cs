using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations;

public sealed class RoleOperationClaimConfiguration : EntityConfiguration<RoleOperationClaim>
{
    public override void Configure(EntityTypeBuilder<RoleOperationClaim> builder)
    {
        base.Configure(builder);

        builder.ToTable("RoleOperationClaims");

        builder.HasOne(u => u.OperationClaim).WithMany().HasForeignKey(u => u.OperationClaimId);
    }
}