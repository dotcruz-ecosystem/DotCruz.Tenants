using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotCruz.Tenants.Infrastructure.Data.Configurations.Tenants;

public class TenantMemberConfiguration : IEntityTypeConfiguration<TenantMember>
{
    public void Configure(EntityTypeBuilder<TenantMember> builder)
    {
        builder.ToTable("tenant_members");

        builder.HasKey(m => m.Id);

        builder.Property(m => m.Id)
            .ValueGeneratedNever();

        builder.Property(m => m.TenantId)
            .IsRequired();

        builder.Property(m => m.UserId)
            .IsRequired(false);

        builder.Property(m => m.Email)
            .HasConversion(e => e.Value, v => Email.Create(v))
            .HasColumnName("email")
            .HasColumnType("citext")
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(m => m.Role)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(m => m.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(m => m.InvitedAt)
            .IsRequired();

        builder.Property(m => m.JoinedAt)
            .IsRequired(false);

        builder.Property(m => m.CreatedAt)
            .IsRequired();

        builder.Property(m => m.UpdatedAt)
            .IsRequired();

        builder.Property(m => m.DeletedAt)
            .IsRequired(false);

        builder.HasIndex(m => new { m.TenantId, m.Email })
            .IsUnique();

        builder.HasIndex(m => m.UserId);
    }
}
