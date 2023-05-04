using IntegrationLogger.Enums;
using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping;

public class IntegrationItemMap : IEntityTypeConfiguration<IntegrationItem>
{
    public void Configure(EntityTypeBuilder<IntegrationItem> builder)
    {
        builder.ToTable("IntegrationItem")
           .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.ItemType)
            .HasConversion(new EnumToNumberConverter<ItemType, int>());

        builder.Property(x => x.ItemStatus)
            .HasConversion(new EnumToNumberConverter<IntegrationStatus, int>());

        builder.Property(x => x.Message);

        if (ProviderDb.LoggerDatabaseProvider == DatabaseProvider.Oracle)
        {
            builder.Property(x => x.ErrorMessage)
                .HasColumnType("CLOB");
        }
        else if (ProviderDb.LoggerDatabaseProvider == DatabaseProvider.SqlServer)
        {
            builder.Property(x => x.ErrorMessage)
                .HasColumnType("NVARCHAR(MAX)");
        }
        else if (ProviderDb.LoggerDatabaseProvider == DatabaseProvider.PostgreSQL)
        {
            builder.Property(x => x.ErrorMessage)
                .HasColumnType("TEXT");
        }

        builder.Property(x => x.Timestamp);
        builder.Property(x => x.IntegrationDetailId);

        builder.HasIndex(x => x.Timestamp);
    }
}
