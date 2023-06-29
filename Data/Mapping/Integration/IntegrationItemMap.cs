using IntegrationLogger.Enums;
using IntegrationLogger.Models.Integration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping.Integration;

public class IntegrationItemMap : IEntityTypeConfiguration<IntegrationItem>
{
    public void Configure(EntityTypeBuilder<IntegrationItem> builder)
    {
        builder.ToTable(nameof(IntegrationItem))
           .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.ItemStatus)
            .HasConversion(new EnumToNumberConverter<LogLevel, int>());

        builder.Property(x => x.Message);

        if (ProviderDb.LoggerDatabaseProvider == DatabaseProvider.Oracle)
        {
            builder.Property(x => x.Content)
                .HasColumnType("CLOB");
        }
        else if (ProviderDb.LoggerDatabaseProvider == DatabaseProvider.SqlServer)
        {
            builder.Property(x => x.Content)
                .HasColumnType("NVARCHAR(MAX)");
        }
        else if (ProviderDb.LoggerDatabaseProvider == DatabaseProvider.PostgreSQL)
        {
            builder.Property(x => x.Content)
                .HasColumnType("TEXT");
        }

        builder.Property(x => x.Timestamp);
        builder.Property(x => x.IntegrationDetailId);

        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.IntegrationDetailId);
        builder.HasIndex(x => new { x.Timestamp, x.IntegrationDetailId });
    }
}
