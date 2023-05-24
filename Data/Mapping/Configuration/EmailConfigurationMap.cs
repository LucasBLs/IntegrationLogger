using IntegrationLogger.Models.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IntegrationLogger.Data.Mapping.Configuration;
public class EmailConfigurationMap : IEntityTypeConfiguration<EmailConfiguration>
{
    public void Configure(EntityTypeBuilder<EmailConfiguration> builder)
    {
        builder.ToTable(nameof(EmailConfiguration))
            .HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.SmtpServer)
            .HasMaxLength(100);
        builder.Property(x => x.SmtpPort);
        builder.Property(x => x.SenderName)
            .HasMaxLength(100);
        builder.Property(x => x.SenderEmail)
            .HasMaxLength(100);
        builder.Property(x => x.EmailPassword)
            .HasMaxLength(100);
        builder.Property(x => x.RecipientEmail)
            .HasMaxLength(100);
        builder.Property(x => x.CcEmails);
        builder.Property(x => x.EmailSubject)
            .HasMaxLength(100);
        builder.Property(x => x.EmailBody)
            .HasMaxLength(1000);

        builder.Property(x => x.LogConfigurationId);
        builder.HasOne(x => x.LogConfiguration)
            .WithOne(x => x.EmailConfiguration)
            .HasForeignKey<EmailConfiguration>(x => x.LogConfigurationId);

        builder.HasIndex(x => x.SmtpServer);
        builder.HasIndex(x => x.SenderEmail);
        builder.HasIndex(x => x.RecipientEmail);
    }
}