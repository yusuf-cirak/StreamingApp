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

        builder.Property(u => u.Value).HasMaxLength(50).IsRequired();

        builder.HasKey(u => new { u.OperationClaimId, u.UserId, u.Value });

        builder.HasIndex(r => r.UserId).IsUnique(false);
        
        builder.HasIndex(r => r.OperationClaimId).IsUnique(false);


        builder.HasIndex(r => new { r.OperationClaimId, r.UserId, r.Value }).IsUnique();


        builder.HasOne(u => u.User)
            .WithMany(u => u.UserOperationClaims)
            .HasForeignKey(u => u.UserId);

        builder.HasOne(u => u.OperationClaim).WithOne().HasForeignKey<UserOperationClaim>(uoc => uoc.OperationClaimId);
    }
}