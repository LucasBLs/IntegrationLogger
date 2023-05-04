using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationOracle
{
    /// <inheritdoc />
    public partial class InitialMigrationOracle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "IntegrationLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    IntegrationName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
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
                name: "IntegrationDetail",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "RAW(16)", nullable: false),
                    ActionType = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    EntityName = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Status = table.Column<int>(type: "NUMBER(10)", nullable: false),
                    DetailIdentifier = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    Message = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
                    ErrorMessage = table.Column<string>(type: "NVARCHAR2(2000)", nullable: true),
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
                    ErrorMessage = table.Column<string>(type: "CLOB", nullable: true),
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
                name: "IX_IntegrationDetail_IntegrationLogId",
                table: "IntegrationDetail",
                column: "IntegrationLogId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationDetail_Timestamp",
                table: "IntegrationDetail",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationItem_IntegrationDetailId",
                table: "IntegrationItem",
                column: "IntegrationDetailId");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationItem_Timestamp",
                table: "IntegrationItem",
                column: "Timestamp");

            migrationBuilder.CreateIndex(
                name: "IX_IntegrationLog_Timestamp",
                table: "IntegrationLog",
                column: "Timestamp");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "IntegrationItem");

            migrationBuilder.DropTable(
                name: "IntegrationDetail");

            migrationBuilder.DropTable(
                name: "IntegrationLog");
        }
    }
}
