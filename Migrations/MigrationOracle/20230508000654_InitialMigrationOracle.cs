using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationOracle
{
    public partial class InitialMigrationOracle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ApiGatewayLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ProjectName = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    RequestPath = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    HttpMethod = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ClientIp = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    StatusCode = table.Column<int>(type: "NUMBER(10)", nullable: true),
                    RequestDuration = table.Column<long>(type: "NUMBER(19)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiGatewayLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    IntegrationName = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ExternalSystem = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    SourceSystem = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_IntegrationLog", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ApiGatewayDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Type = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Content = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false),
                    ApiGatewayLogId = table.Column<Guid>(type: "RAW(16)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ApiGatewayDetail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ApiGatewayDetail_ApiGatewayLog_ApiGatewayLogId",
                        column: x => x.ApiGatewayLogId,
                        principalTable: "ApiGatewayLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "IntegrationDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DetailIdentifier = table.Column<string>(type: "NVARCHAR2(450)", nullable: true),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false),
                    IntegrationLogId = table.Column<Guid>(type: "RAW(16)", nullable: false)
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
                name: "IntegrationItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ItemType = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    ItemIdentifier = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ItemStatus = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Content = table.Column<string>(type: "CLOB", nullable: true),
                    Timestamp = table.Column<DateTimeOffset>(type: "TIMESTAMP(7) WITH TIME ZONE", nullable: false),
                    IntegrationDetailId = table.Column<Guid>(type: "RAW(16)", nullable: false)
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
                name: "IX_ApiGatewayDetail_ApiGatewayLogId",
                table: "ApiGatewayDetail",
                column: "ApiGatewayLogId");

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayDetail_ApiGatewayLogId_Timestamp",
                table: "ApiGatewayDetail",
                columns: new[] { "ApiGatewayLogId", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayDetail_Timestamp",
                table: "ApiGatewayDetail",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayLog_ProjectName",
                table: "ApiGatewayLog",
                column: "ProjectName");

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayLog_ProjectName_Timestamp",
                table: "ApiGatewayLog",
                columns: new[] { "ProjectName", "Timestamp" });

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayLog_RequestDuration",
                table: "ApiGatewayLog",
                column: "RequestDuration");

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayLog_StatusCode",
                table: "ApiGatewayLog",
                column: "StatusCode");

            migrationBuilder.CreateIndex(
                name: "IX_ApiGatewayLog_Timestamp",
                table: "ApiGatewayLog",
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
                name: "IX_IntegrationDetail_Timestamp_IntegrationLogId_DetailIdentifier",
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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ApiGatewayDetail");

            migrationBuilder.DropTable(
                name: "IntegrationItem");

            migrationBuilder.DropTable(
                name: "ApiGatewayLog");

            migrationBuilder.DropTable(
                name: "IntegrationDetail");

            migrationBuilder.DropTable(
                name: "IntegrationLog");
        }
    }
}
