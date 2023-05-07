using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationLogger.Data.Mapping;

public class IntegrationLogMap : IEntityTypeConfiguration<IntegrationLog>
{
    public void Configure(EntityTypeBuilder<IntegrationLog> builder)
    {
        builder.ToTable("IntegrationLog")
            .HasKey(x => x.Id);

        builder.Property(x => x.Id)
            .IsRequired();

        builder.Property(x => x.IntegrationName);
        builder.Property(x => x.Timestamp);

        builder.Property(x => x.Message);
        builder.Property(x => x.ExternalSystem);
        builder.Property(x => x.SourceSystem);

        builder.HasMany(x => x.Details)
           .WithOne(x => x.IntegrationLog)
           .HasForeignKey(x => x.IntegrationLogId);

        builder.HasIndex(x => x.Timestamp);
        builder.HasIndex(x => x.IntegrationName);
    }
}