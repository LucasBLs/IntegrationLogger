using IntegrationLogger.Enums;
using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping;

public class IntegrationDetailMap : IEntityTypeConfiguration<IntegrationDetail>
{
    public void Configure(EntityTypeBuilder<IntegrationDetail> builder)
    {
        builder.ToTable("IntegrationDetail")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.ActionType)
            .HasConversion(new EnumToNumberConverter<ActionType, int>());

        builder.Property(x => x.EntityName);

        builder.Property(x => x.Status)
            .HasConversion(new EnumToNumberConverter<IntegrationStatus, int>());

        builder.Property(x => x.Message);
        builder.Property(x => x.ErrorMessage);
        builder.Property(x => x.IntegrationLogId);

        builder.HasMany(x => x.Items)
            .WithOne(x => x.IntegrationDetail)
            .HasForeignKey(x => x.IntegrationDetailId);

        builder.HasIndex(x => x.Timestamp);
    }
}