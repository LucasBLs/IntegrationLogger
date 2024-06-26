﻿using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationPostgreSQL
{
    public partial class InitialMigrationPostgreSQL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "GatewayLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProjectName = table.Column<string>(type: "text", nullable: true),
                    RequestPath = table.Column<string>(type: "text", nullable: true),
                    HttpMethod = table.Column<string>(type: "text", nullable: true),
                    ClientIp = table.Column<string>(type: "text", nullable: true),
                    StatusCode = table.Column<int>(type: "integer", nullable: true),
                    RequestDuration = table.Column<long>(type: "bigint", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    IntegrationName = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    ExternalSystem = table.Column<string>(type: "text", nullable: true),
                    SourceSystem = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LogConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LogSource = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LogLevel = table.Column<int>(type: "integer", nullable: false),
                    LogStepByStep = table.Column<bool>(type: "boolean", nullable: false),
                    LogRetentionPeriod = table.Column<int>(type: "integer", nullable: false),
                    AutoArchive = table.Column<bool>(type: "boolean", nullable: false),
                    ArchivePath = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EmailNotification = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogConfiguration", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "GatewayDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ApiGatewayLogId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GatewayDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GatewayDetail_GatewayLog_ApiGatewayLogId",
                        column: x => x.ApiGatewayLogId,
                        principalTable: "GatewayLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    DetailIdentifier = table.Column<string>(type: "text", nullable: true),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IntegrationLogId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationDetail_IntegrationLog_IntegrationLogId",
                        column: x => x.IntegrationLogId,
                        principalTable: "IntegrationLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "EmailConfiguration",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SmtpServer = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    SmtpPort = table.Column<int>(type: "integer", nullable: false),
                    SenderName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    SenderEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    EmailPassword = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    RecipientEmail = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    CcEmails = table.Column<string>(type: "text", nullable: false),
                    EmailSubject = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    EmailBody = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    LogConfigurationId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EmailConfiguration", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EmailConfiguration_LogConfiguration_LogConfigurationId",
                        column: x => x.LogConfigurationId,
                        principalTable: "LogConfiguration",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemType = table.Column<int>(type: "integer", nullable: false),
                    ItemIdentifier = table.Column<string>(type: "text", nullable: true),
                    ItemStatus = table.Column<int>(type: "integer", nullable: false),
                    Message = table.Column<string>(type: "text", nullable: true),
                    Content = table.Column<string>(type: "TEXT", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    IntegrationDetailId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_IntegrationItem_IntegrationDetail_IntegrationDetailId",
                        column: x => x.IntegrationDetailId,
                        principalTable: "IntegrationDetail",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfiguration_LogConfigurationId",
                table: "EmailConfiguration",
                column: "LogConfigurationId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfiguration_RecipientEmail",
                table: "EmailConfiguration",
                column: "RecipientEmail");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfiguration_SenderEmail",
                table: "EmailConfiguration",
                column: "SenderEmail");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfiguration_SmtpServer",
                table: "EmailConfiguration",
                column: "SmtpServer");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayDetail_ApiGatewayLogId",
                table: "GatewayDetail",
                column: "ApiGatewayLogId");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayDetail_ApiGatewayLogId_Timestamp",
                table: "GatewayDetail",
                columns: new[] { "ApiGatewayLogId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_GatewayDetail_Timestamp",
                table: "GatewayDetail",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayLog_ProjectName",
                table: "GatewayLog",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayLog_ProjectName_Timestamp",
                table: "GatewayLog",
                columns: new[] { "ProjectName", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_GatewayLog_RequestDuration",
                table: "GatewayLog",
                column: "RequestDuration");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayLog_StatusCode",
                table: "GatewayLog",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_GatewayLog_Timestamp",
                table: "GatewayLog",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationDetail_DetailIdentifier",
                table: "IntegrationDetail",
                column: "DetailIdentifier");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationDetail_IntegrationLogId",
                table: "IntegrationDetail",
                column: "IntegrationLogId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationDetail_Timestamp",
                table: "IntegrationDetail",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationDetail_Timestamp_IntegrationLogId",
                table: "IntegrationDetail",
                columns: new[] { "Timestamp", "IntegrationLogId" });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationDetail_Timestamp_IntegrationLogId_DetailIdentifi~",
                table: "IntegrationDetail",
                columns: new[] { "Timestamp", "IntegrationLogId", "DetailIdentifier" });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationItem_IntegrationDetailId",
                table: "IntegrationItem",
                column: "IntegrationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationItem_Timestamp",
                table: "IntegrationItem",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationItem_Timestamp_IntegrationDetailId",
                table: "IntegrationItem",
                columns: new[] { "Timestamp", "IntegrationDetailId" });

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLog_IntegrationName",
                table: "IntegrationLog",
                column: "IntegrationName");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLog_Timestamp",
                table: "IntegrationLog",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLog_Timestamp_IntegrationName",
                table: "IntegrationLog",
                columns: new[] { "Timestamp", "IntegrationName" });

            migrationBuilder.CreateIndex(
                name: "IX_LogConfiguration_LogSource",
                table: "LogConfiguration",
                column: "LogSource");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EmailConfiguration");

            migrationBuilder.DropTable(
                name: "GatewayDetail");

            migrationBuilder.DropTable(
                name: "IntegrationItem");

            migrationBuilder.DropTable(
                name: "LogConfiguration");

            migrationBuilder.DropTable(
                name: "GatewayLog");

            migrationBuilder.DropTable(
                name: "IntegrationDetail");

            migrationBuilder.DropTable(
                name: "IntegrationLog");
        }
    }
}
