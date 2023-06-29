using IntegrationLogger.Enums;
using IntegrationLogger.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace IntegrationLogger.Data.Mapping.Configuration;
public class LogConfigurationMap : IEntityTypeConfiguration<LogConfiguration>
{
    public void Configure(EntityTypeBuilder<LogConfiguration> builder)
    {
        builder.ToTable(nameof(LogConfiguration))
            .HasKey(x => x.Id);

        builder.Property(x => x.Id);
        builder.Property(x => x.LogLevel)
            .HasConversion(new EnumToNumberConverter<LogLevel, int>());
        builder.Property(x => x.LogStepByStep);
        builder.Property(x => x.LogRetentionPeriod);
        builder.Property(x => x.AutoArchive);
        builder.Property(x => x.ArchivePath)
            .HasMaxLength(100);
        builder.Property(x => x.EmailNotification);

        builder.HasOne(x => x.EmailConfiguration)
            .WithOne(x => x.LogConfiguration)
            .HasForeignKey<EmailConfiguration>(x => x.LogConfigurationId);
    }
}