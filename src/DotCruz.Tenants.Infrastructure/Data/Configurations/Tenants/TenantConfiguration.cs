using DotCruz.Tenants.Domain.Entities.Tenants;
using DotCruz.Tenants.Domain.ValueObjects.Communication;
using DotCruz.Tenants.Domain.ValueObjects.Identity;
using DotCruz.Tenants.Domain.ValueObjects.Location;
using DotCruz.Tenants.Domain.ValueObjects.Tenants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DotCruz.Tenants.Infrastructure.Data.Configurations.Tenants;

public class TenantConfiguration : IEntityTypeConfiguration<Tenant>
{
    public void Configure(EntityTypeBuilder<Tenant> builder)
    {
        builder.ToTable("tenants");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .ValueGeneratedNever();

        builder.Property(t => t.Name)
            .HasConversion(n => n.Value, v => Name.Create(v))
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(t => t.Type)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.Status)
            .HasConversion<string>()
            .IsRequired()
            .HasMaxLength(30);

        builder.Property(t => t.Slug)
            .HasConversion(s => s.Value, v => TenantSlug.Create(v))
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(t => t.Slug)
            .IsUnique();

        builder.Property(t => t.SuspensionReason)
            .HasConversion(
                r => r != null ? r.Value : null,
                v => v != null ? SuspensionReason.Create(v) : null
            )
            .HasMaxLength(500)
            .IsRequired(false);

        builder.OwnsOne(t => t.Document, doc =>
        {
            doc.Property(d => d.Number)
                .HasColumnName("document_number")
                .IsRequired()
                .HasMaxLength(14);

            doc.Property(d => d.Type)
                .HasColumnName("document_type")
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(4);

            doc.HasIndex(d => d.Number)
                .IsUnique();
        });

        builder.OwnsOne(t => t.Contact, c =>
        {
            c.Property(co => co.Email)
                .HasColumnName("contact_email")
                .HasColumnType("citext")
                .HasConversion(e => e.Value, v => Email.Create(v))
                .IsRequired()
                .HasMaxLength(255);

            c.OwnsOne(co => co.Phone, p =>
            {
                p.Property(ph => ph.CountryCode)
                    .HasColumnName("contact_phone_country_code")
                    .IsRequired();

                p.Property(ph => ph.NationalNumber)
                    .HasColumnName("contact_phone_national_number")
                    .IsRequired()
                    .HasMaxLength(14);
            });
        });

        builder.OwnsOne(t => t.Address, a =>
        {
            a.ToJson("address");

            a.Property(ad => ad.Street)
                .HasJsonPropertyName("street")
                .IsRequired()
                .HasMaxLength(200);

            a.Property(ad => ad.Number)
                .HasJsonPropertyName("number")
                .IsRequired()
                .HasMaxLength(20);

            a.Property(ad => ad.Complement)
                .HasJsonPropertyName("complement")
                .HasMaxLength(100)
                .IsRequired(false);

            a.Property(ad => ad.Neighborhood)
                .HasJsonPropertyName("neighborhood")
                .IsRequired()
                .HasMaxLength(150);

            a.Property(ad => ad.City)
                .HasJsonPropertyName("city")
                .IsRequired()
                .HasMaxLength(100);

            a.Property(ad => ad.State)
                .HasJsonPropertyName("state")
                .HasConversion(s => s.Value, v => State.Create(v))
                .IsRequired()
                .HasMaxLength(2);

            a.Property(ad => ad.ZipCode)
                .HasJsonPropertyName("zip_code")
                .HasConversion(z => z.Value, v => ZipCode.Create(v))
                .IsRequired()
                .HasMaxLength(8);
        });

        builder.OwnsOne(t => t.Subscription, s =>
        {
            s.Property(sub => sub.Plan)
                .HasColumnName("subscription_plan")
                .HasConversion<string>()
                .IsRequired()
                .HasMaxLength(20);

            s.Property(sub => sub.TrialEndDate)
                .HasColumnName("subscription_trial_end_date")
                .IsRequired(false);

            s.OwnsOne(sub => sub.Duration, d =>
            {
                d.Property(dp => dp.Start)
                    .HasColumnName("subscription_start_date")
                    .IsRequired();

                d.Property(dp => dp.End)
                    .HasColumnName("subscription_end_date")
                    .IsRequired(false);
            });

            s.OwnsOne(sub => sub.Limits, l =>
            {
                l.Property(rl => rl.MaxUsers)
                    .HasColumnName("subscription_max_users")
                    .IsRequired();

                l.Property(rl => rl.MaxEmailsPerMonth)
                    .HasColumnName("subscription_max_emails_per_month")
                    .IsRequired();
            });
        });

        builder.OwnsOne(t => t.Branding, b =>
        {
            b.ToJson("branding");

            b.Property(br => br.LogoUrl)
                .HasJsonPropertyName("logo_url")
                .HasMaxLength(500)
                .IsRequired();

            b.Property(br => br.HeaderBackgroundColor)
                .HasJsonPropertyName("header_background_color")
                .HasMaxLength(20)
                .IsRequired();

            b.Property(br => br.Website)
                .HasJsonPropertyName("website")
                .HasMaxLength(255)
                .IsRequired();

            b.Property(br => br.UnsubscribeUrl)
                .HasJsonPropertyName("unsubscribe_url")
                .HasMaxLength(500)
                .IsRequired();
        });
    }
}
