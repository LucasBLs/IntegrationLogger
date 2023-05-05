﻿// <auto-generated />
using System;
using IntegrationLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Oracle.EntityFrameworkCore.Metadata;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationOracle
{
    [DbContext(typeof(IntegrationLogContextOracle))]
    [Migration("20230504204357_InitialMigrationOracle")]
    partial class InitialMigrationOracle
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            OracleModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("IntegrationLogger.Models.ApiGatewayDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<Guid>("ApiGatewayLogId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("HeaderName")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("HeaderValue")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.HasKey("Id");

                    b.HasIndex("ApiGatewayLogId");

                    b.ToTable("ApiGatewayDetail", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.ApiGatewayLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ClientIp")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("HttpMethod")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<long?>("RequestDuration")
                        .HasColumnType("NUMBER(19)");

                    b.Property<string>("RequestPath")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.HasKey("Id");

                    b.ToTable("ApiGatewayLog", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("DetailIdentifier")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("ErrorMessage")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<Guid>("IntegrationLogId")
                        .HasColumnType("RAW(16)");

                    b.Property<string>("Message")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<int>("Status")
                        .HasColumnType("NUMBER(10)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationLogId");

                    b.HasIndex("Timestamp");

                    b.ToTable("IntegrationDetail", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ErrorMessage")
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

                    b.ToTable("IntegrationItem", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("RAW(16)");

                    b.Property<string>("ExternalSystem")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("IntegrationName")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("Message")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<string>("SourceSystem")
                        .HasColumnType("NVARCHAR2(2000)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("TIMESTAMP(7) WITH TIME ZONE");

                    b.HasKey("Id");

                    b.HasIndex("Timestamp");

                    b.ToTable("IntegrationLog", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.ApiGatewayDetail", b =>
                {
                    b.HasOne("IntegrationLogger.Models.ApiGatewayLog", "ApiGatewayLog")
                        .WithMany("Details")
                        .HasForeignKey("ApiGatewayLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ApiGatewayLog");
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationDetail", b =>
                {
                    b.HasOne("IntegrationLogger.Models.IntegrationLog", "IntegrationLog")
                        .WithMany("Details")
                        .HasForeignKey("IntegrationLogId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IntegrationLog");
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationItem", b =>
                {
                    b.HasOne("IntegrationLogger.Models.IntegrationDetail", "IntegrationDetail")
                        .WithMany("Items")
                        .HasForeignKey("IntegrationDetailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("IntegrationDetail");
                });

            modelBuilder.Entity("IntegrationLogger.Models.ApiGatewayLog", b =>
                {
                    b.Navigation("Details");
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationDetail", b =>
                {
                    b.Navigation("Items");
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationLog", b =>
                {
                    b.Navigation("Details");
                });
#pragma warning restore 612, 618
        }
    }
}