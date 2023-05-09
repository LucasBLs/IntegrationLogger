using IntegrationLogger.Data.Mapping;
using IntegrationLogger.Data.Mapping.Gateway;
using IntegrationLogger.Data.Mapping.Integration;
using IntegrationLogger.Models;
using IntegrationLogger.Models.Gateway;
using IntegrationLogger.Models.Integration;
using Microsoft.EntityFrameworkCore;

namespace IntegrationLogger.Data;
public abstract class IntegrationLogContextBase : DbContext
{
    public IntegrationLogContextBase(DbContextOptions options) : base(options)
    {
    }

    public DbSet<IntegrationLog> IntegrationLogs { get; set; } = default!;
    public DbSet<IntegrationDetail> IntegrationDetails { get; set; } = default!;
    public DbSet<IntegrationItem> IntegrationItems { get; set; } = default!;

    public DbSet<GatewayLog> ApiGatewayLogs { get; set; } = default!;
    public DbSet<GatewayDetail> ApiGatewayDetails { get; set; } = default!;

    public DbSet<LogConfiguration> LogConfigurations { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new IntegrationLogMap());
        modelBuilder.ApplyConfiguration(new IntegrationDetailMap());
        modelBuilder.ApplyConfiguration(new IntegrationItemMap());

        modelBuilder.ApplyConfiguration(new GatewayLogMap());
        modelBuilder.ApplyConfiguration(new GatewayDetailMap());

        modelBuilder.ApplyConfiguration(new LogConfigurationMap());
    }
}

/*
 De dentro do SchedulerWeb
dotnet ef migrations add InitialMigrationOracle --context IntegrationLogContextOracle --output-dir Migrations/MigrationOracle --project ..\IntegrationLogger\
dotnet ef migrations add InitialMigrationSqlServer --context IntegrationLogContextSqlServer --output-dir Migrations/MigrationSqlServer --project ..\IntegrationLogger\
dotnet ef migrations add InitialMigrationPostgreSQL --context IntegrationLogContextPostgreSQL --output-dir Migrations/MigrationPostgreSQL --project ..\IntegrationLogger\

SqlServer:
"Server=localhost,{entity.Port};Database=qualidoc;User Id=sa;Password=Lucas@2022;Trusted_Connection=False;TrustServerCertificate=True;"

PostGreSql:
"Server=localhost;Database={entity.DatabaseName};Port={entity.Port};User Id={entity.UserId};Password={entity.Password};"

dotnet ef migrations add InitialCreate --context IntegrationLogContext --project ..\IntegrationLogger\
dotnet ef database update InitialMigrationPostgreSQL --context IntegrationLogContext --output-dir Migrations/MigrationPostgreSQL --project ..\IntegrationLogger\

 */