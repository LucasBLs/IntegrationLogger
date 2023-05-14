﻿// <auto-generated />
using System;
using IntegrationLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationOracle
{
    [DbContext(typeof(IntegrationLogContextOracle))]
    partial class IntegrationLogContextOracleModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IntegrationLogger.Models.Configuration.EmailConfiguration", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("CcEmails")
                        .IsRequired()
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("EmailBody")
                        .HasMaxLength(1000)
                        .HasColumnType("NVARCHAR2(1000)");

                    b.Property<string>("EmailPassword")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("EmailSubject")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<Guid>("LogConfigurationId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("RecipientEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("SenderEmail")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<string>("SenderName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<int>("SmtpPort")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("SmtpServer")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

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
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ArchivePath")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<bool>("AutoArchive")
                        .HasColumnType("NUMBER(1)");

                    b.Property<bool>("EmailNotification")
                        .HasColumnType("NUMBER(1)");

                    b.Property<int>("LogLevel")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("LogRetentionPeriod")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("LogSource")
                        .HasMaxLength(100)
                        .HasColumnType("NVARCHAR2(100)");

                    b.Property<bool>("LogStepByStep")
                        .HasColumnType("NUMBER(1)");

                    b.HasKey("Id");

                    b.HasIndex("LogSource");

                    b.ToTable("LogConfiguration", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.Gateway.GatewayDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<Guid>("ApiGatewayLogId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Content")
                        .HasColumnType("CLOB");

                    b.Property<string>("Message")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Status")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.Property<int>("Type")
                        .HasColumnType("NUMBER(10)");

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
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ClientIp")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("HttpMethod")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<long?>("RequestDuration")
                        .HasColumnType("NUMBER(19)");

                    b.Property<string>("RequestPath")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

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
                        .HasColumnType("RAW(16)");

                    b.Property<string>("DetailIdentifier")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<Guid>("IntegrationLogId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Message")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Status")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

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
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Content")
                        .HasColumnType("CLOB");

                    b.Property<Guid>("IntegrationDetailId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ItemIdentifier")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("ItemStatus")
                        .HasColumnType("NUMBER(10)");

                    b.Property<int>("ItemType")
                        .HasColumnType("NUMBER(10)");

                    b.Property<string>("Message")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

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
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ExternalSystem")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("IntegrationName")
                        .HasColumnType("NVARCHAR2(450)");

                    b.Property<string>("Message")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("SourceSystem")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

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
