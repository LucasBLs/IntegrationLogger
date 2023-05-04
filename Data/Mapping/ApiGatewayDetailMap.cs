using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationLogger.Data.Mapping;

public class ApiGatewayDetailMap : IEntityTypeConfiguration<ApiGatewayDetail>
{
    public void Configure(EntityTypeBuilder<ApiGatewayDetail> builder)
    {
        builder.ToTable("ApiGatewayDetail")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id);

        builder.Property(x => x.HeaderName);

        builder.Property(x => x.HeaderValue);

        builder.Property(x => x.ApiGatewayLogId);
    }
}
