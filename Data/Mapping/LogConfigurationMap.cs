using IntegrationLogger.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationLogger.Data.Mapping;
public class LogConfigurationMap : IEntityTypeConfiguration<LogConfiguration>
{
    public void Configure(EntityTypeBuilder<LogConfiguration> builder)
    {
        builder.ToTable(nameof(LogConfiguration))
            .HasKey(x => x.Id);

        builder.Property(x => x.Id);
        builder.Property(x => x.LogSource)
            .HasMaxLength(100);
        builder.Property(x => x.LogRetentionPeriod);
        builder.Property(x => x.AutoArchive);
        builder.Property(x => x.ArchivePath)
            .HasMaxLength(100);
        builder.Property(x => x.EmailNotification);
        builder.Property(x => x.EmailRecipients)
            .HasMaxLength(1000);
        builder.Property(x => x.EmailHost);
        builder.Property(x => x.EmailPort);
        builder.Property(x => x.EmailUsername);
        builder.Property(x => x.EmailPassword);
        builder.Property(x => x.EmailUseSSL);

        builder.HasIndex(x => x.LogSource);
        builder.HasIndex(x => x.EmailRecipients);
    }
}