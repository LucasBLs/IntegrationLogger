using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace IntegrationLogger.Migrations.MigrationOracle
{
    public partial class InitialMigrationOracleFieldNameDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "IntegrationLogName",
                table: "IntegrationDetail",
                type: "NVARCHAR2(2000)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IntegrationLogName",
                table: "IntegrationDetail");
        }
    }
}
