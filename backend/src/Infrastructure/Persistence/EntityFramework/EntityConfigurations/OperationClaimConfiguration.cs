using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public sealed class OperationClaimConfiguration : EntityConfiguration<OperationClaim>
    {
        public override void Configure(EntityTypeBuilder<OperationClaim> builder)
        {
            base.Configure(builder);

            builder.ToTable("OperationClaims");

            builder.Property(o => o.Name).HasColumnName("Name");

            builder.HasIndex(o => o.Name).IsUnique();
        }
    }
}