using IntegrationLogger.Enums;
using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping;

public class ApiGatewayDetailMap : IEntityTypeConfiguration<ApiGatewayDetail>
{
    public void Configure(EntityTypeBuilder<ApiGatewayDetail> builder)
    {
        builder.ToTable("ApiGatewayDetail")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.Type)
            .HasConversion(new EnumToNumberConverter<DetailType, int>());

        builder.Property(x => x.Message);

        builder.Property(x => x.Timestamp);

        builder.Property(x => x.Content);

        builder.Property(x => x.ApiGatewayLogId);

        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.ApiGatewayLogId);
    }
}
