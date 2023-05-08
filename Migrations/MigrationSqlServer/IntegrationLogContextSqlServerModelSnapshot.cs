﻿// <auto-generated />
using System;
using IntegrationLogger.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationSqlServer
{
    [DbContext(typeof(IntegrationLogContextSqlServer))]
    partial class IntegrationLogContextSqlServerModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.16")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("IntegrationLogger.Models.ApiGatewayDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("ApiGatewayLogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.Property<int>("Type")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApiGatewayLogId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("ApiGatewayLogId", "Timestamp");

                    b.ToTable("ApiGatewayDetail", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.ApiGatewayLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ClientIp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HttpMethod")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProjectName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<long?>("RequestDuration")
                        .HasColumnType("bigint");

                    b.Property<string>("RequestPath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("StatusCode")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("ProjectName");

                    b.HasIndex("RequestDuration");

                    b.HasIndex("StatusCode");

                    b.HasIndex("Timestamp");

                    b.HasIndex("ProjectName", "Timestamp");

                    b.ToTable("ApiGatewayLog", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationDetail", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("DetailIdentifier")
                        .HasColumnType("nvarchar(450)");

                    b.Property<Guid>("IntegrationLogId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("DetailIdentifier");

                    b.HasIndex("IntegrationLogId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Timestamp", "IntegrationLogId");

                    b.HasIndex("Timestamp", "IntegrationLogId", "DetailIdentifier");

                    b.ToTable("IntegrationDetail", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationItem", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("NVARCHAR(MAX)");

                    b.Property<Guid>("IntegrationDetailId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ItemIdentifier")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("ItemStatus")
                        .HasColumnType("int");

                    b.Property<int>("ItemType")
                        .HasColumnType("int");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationDetailId");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Timestamp", "IntegrationDetailId");

                    b.ToTable("IntegrationItem", (string)null);
                });

            modelBuilder.Entity("IntegrationLogger.Models.IntegrationLog", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("ExternalSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("IntegrationName")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Message")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SourceSystem")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Timestamp")
                        .HasColumnType("datetimeoffset");

                    b.HasKey("Id");

                    b.HasIndex("IntegrationName");

                    b.HasIndex("Timestamp");

                    b.HasIndex("Timestamp", "IntegrationName");

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
