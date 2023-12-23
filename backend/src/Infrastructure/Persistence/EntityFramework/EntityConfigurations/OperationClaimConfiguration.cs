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

            builder.Property(o => o.Id).HasColumnName("Id");

            builder.HasKey(o => o.Id);

            builder.Property(o => o.Name).HasColumnName("Name");
        }
    }
}