using IntegrationLogger.Enums;
using IntegrationLogger.Models.Gateway;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping.Gateway;

public class GatewayDetailMap : IEntityTypeConfiguration<GatewayDetail>
{
    public void Configure(EntityTypeBuilder<GatewayDetail> builder)
    {
        builder.ToTable(nameof(GatewayDetail))
            .HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.Type)
                .HasConversion(new EnumToNumberConverter<DetailType, int>());

        builder.Property(x => x.Status)
            .HasConversion(new EnumToNumberConverter<IntegrationStatus, int>());

        builder.Property(x => x.Message);

        builder.Property(x => x.Timestamp);

        builder.Property(x => x.Content);

        builder.Property(x => x.ApiGatewayLogId);

        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.ApiGatewayLogId);
        builder.HasIndex(x => new { x.ApiGatewayLogId, x.Timestamp });
    }
}
