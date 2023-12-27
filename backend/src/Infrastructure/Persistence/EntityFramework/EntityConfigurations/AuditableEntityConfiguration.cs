using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SharedKernel;

namespace Infrastructure.Persistence.EntityFramework.EntityConfigurations
{
    public abstract class AuditableEntityConfiguration<T> : EntityConfiguration<T>
        where T : AuditableEntity
    {
        public override void Configure(EntityTypeBuilder<T> builder)
        {
            base.Configure(builder);

            builder.Property(e => e.CreatedDate).HasColumnName("CreatedDate").IsRequired();

            builder.Property(e => e.UpdatedDate).HasColumnName("UpdatedDate").IsRequired(false);
        }
    }
}