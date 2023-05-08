using IntegrationLogger.Models.ApiGateway;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationLogger.Data.Mapping.ApiGateway;

public class ApiGatewayLogMap : IEntityTypeConfiguration<ApiGatewayLog>
{
    public void Configure(EntityTypeBuilder<ApiGatewayLog> builder)
    {
        builder.ToTable(nameof(ApiGatewayLog))
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.ProjectName);

        builder.Property(x => x.RequestPath);

        builder.Property(x => x.HttpMethod);

        builder.Property(x => x.ClientIp);

        builder.Property(x => x.StatusCode);

        builder.Property(x => x.RequestDuration);

        builder.Property(x => x.Timestamp);

        builder.HasMany(x => x.Details)
            .WithOne(x => x.ApiGatewayLog)
            .HasForeignKey(x => x.ApiGatewayLogId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasIndex(x => x.ProjectName);
        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.StatusCode);
        builder.HasIndex(x => x.RequestDuration);
        builder.HasIndex(x => new { x.ProjectName, x.Timestamp });
    }
}