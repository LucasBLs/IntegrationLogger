using IntegrationLogger.Enums;
using IntegrationLogger.Models.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping.Integration;

public class IntegrationDetailMap : IEntityTypeConfiguration<IntegrationDetail>
{
    public void Configure(EntityTypeBuilder<IntegrationDetail> builder)
    {
        builder.ToTable(nameof(IntegrationDetail))
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.DetailIdentifier);

        builder.Property(x => x.Status)
            .HasConversion(new EnumToNumberConverter<IntegrationStatus, int>());

        builder.Property(x => x.Message);
        builder.Property(x => x.IntegrationLogName);
        builder.Property(x => x.IntegrationLogId);
        builder.HasMany(x => x.Items)
            .WithOne(x => x.IntegrationDetail)
            .HasForeignKey(x => x.IntegrationDetailId);

        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.IntegrationLogId);
        builder.HasIndex(x => x.DetailIdentifier);
        builder.HasIndex(x => new { x.Timestamp, x.IntegrationLogId });
        builder.HasIndex(x => new { x.Timestamp, x.IntegrationLogId, x.DetailIdentifier });
    }
}