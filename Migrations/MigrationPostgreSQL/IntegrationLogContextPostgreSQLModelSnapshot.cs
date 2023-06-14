﻿// <auto-generated />
using System;
using IntegrationLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationPostgreSQL
{
    [DbContext(typeof(IntegrationLogContextPostgreSQL))]
    partial class IntegrationLogContextPostgreSQLModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("IntegrationLogger.Models.Configuration.EmailConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("CcEmails")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("EmailBody")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<string>("EmailPassword")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("EmailSubject")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<Guid>("LogConfigurationId")
                        .HasColumnType("uuid");

                    b.Property<string>("RecipientEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<int>("SmtpPort")
                        .HasColumnType("integer");

                    b.Property<string>("SmtpServer")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.HasKey("Id");

                    b.HasIndex("LogConfigurationId")
                        .IsUnique();

                    b.HasIndex("RecipientEmail");

                    b.HasIndex("SenderEmail");

                    b.HasIndex("SmtpServer");

                    b.ToTable("EmailConfiguration", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Configuration.LogConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ArchivePath")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("AutoArchive")
                        .HasColumnType("boolean");

                    b.Property<bool>("EmailNotification")
                        .HasColumnType("boolean");

                    b.Property<int>("LogLevel")
                        .HasColumnType("integer");

                    b.Property<int>("LogRetentionPeriod")
                        .HasColumnType("integer");

                    b.Property<string>("LogSource")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<bool>("LogStepByStep")
                        .HasColumnType("boolean");

                    b.HasKey("Id");

                    b.HasIndex("LogSource");

                    b.ToTable("LogConfiguration", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Gateway.GatewayDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ApiGatewayLogId")
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ApiGatewayLogId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("ApiGatewayLogId", "Timestamp");

                    b.ToTable("GatewayDetail", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Gateway.GatewayLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ClientIp")
                        .HasColumnType("text");

                    b.Property<string>("HttpMethod")
                        .HasColumnType("text");

                    b.Property<string>("ProjectName")
                        .HasColumnType("text");

                    b.Property<long?>("RequestDuration")
                        .HasColumnType("bigint");

                    b.Property<string>("RequestPath")
                        .HasColumnType("text");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ProjectName");

                    b.HasIndex("RequestDuration");

                    b.HasIndex("StatusCode");

                    b.HasIndex("Timestamp");

                    b.HasIndex("ProjectName", "Timestamp");

                    b.ToTable("GatewayLog", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("DetailIdentifier")
                        .HasColumnType("text");

                    b.Property<Guid>("IntegrationLogId")
                        .HasColumnType("uuid");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DetailIdentifier");

                    b.HasIndex("IntegrationLogId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Timestamp", "IntegrationLogId");

                    b.HasIndex("Timestamp", "IntegrationLogId", "DetailIdentifier");

                    b.ToTable("IntegrationDetail", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Content")
                        .HasColumnType("TEXT");

                    b.Property<Guid>("IntegrationDetailId")
                        .HasColumnType("uuid");

                    b.Property<string>("ItemIdentifier")
                        .HasColumnType("text");

                    b.Property<int>("ItemStatus")
                        .HasColumnType("integer");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationDetailId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Timestamp", "IntegrationDetailId");

                    b.ToTable("IntegrationItem", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("ExternalSystem")
                        .HasColumnType("text");

                    b.Property<string>("IntegrationName")
                        .HasColumnType("text");

                    b.Property<string>("Message")
                        .HasColumnType("text");

                    b.Property<string>("SourceSystem")
                        .HasColumnType("text");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationName");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Timestamp", "IntegrationName");

                    b.ToTable("IntegrationLog", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Configuration.EmailConfiguration", b =>
                {
                    b.HasOne("IntegrationLogger.Models.Configuration.LogConfiguration", "LogConfiguration")
                        .WithOne("EmailConfiguration")
                        .HasForeignKey("IntegrationLogger.Models.Configuration.EmailConfiguration", "LogConfigurationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("LogConfiguration");
                });

            modelBuilder.Entity("IntegrationLogger.Models.Gateway.GatewayDetail", b =>
                {
                    b.HasOne("IntegrationLogger.Models.Gateway.GatewayLog", "ApiGatewayLog")
                        .WithMany()
                        .HasForeignKey("ApiGatewayLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApiGatewayLog");
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationDetail", b =>
                {
                    b.HasOne("IntegrationLogger.Models.Integration.IntegrationLog", "IntegrationLog")
                        .WithMany("Details")
                        .HasForeignKey("IntegrationLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IntegrationLog");
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationItem", b =>
                {
                    b.HasOne("IntegrationLogger.Models.Integration.IntegrationDetail", "IntegrationDetail")
                        .WithMany("Items")
                        .HasForeignKey("IntegrationDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IntegrationDetail");
                });

            modelBuilder.Entity("IntegrationLogger.Models.Configuration.LogConfiguration", b =>
                {
                    b.Navigation("EmailConfiguration")
                        .IsRequired();
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationDetail", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("IntegrationLogger.Models.Integration.IntegrationLog", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}
